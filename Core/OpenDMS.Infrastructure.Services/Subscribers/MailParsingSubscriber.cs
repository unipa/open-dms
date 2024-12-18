using MessageBus.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenDMS.Core.DTOs;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Entities;
using OpenDMS.Domain.Entities.Documents;
using OpenDMS.Domain.Entities.Settings;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Events.Types;
using OpenDMS.Domain.Infrastructure;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Repositories;
using OpenDMS.Domain.Services;
using OpenDMS.Infrastructure.Services.BusinessLogic;
using OpenDMS.MailSpooler.Core.Interfaces;

namespace OpenDMS.Infrastructure.Services.Subscribers
{
    public class MailParsingSubscriber : IEventSubscriber
    {
        private readonly ILogger<MailParsingSubscriber> logger;
        private readonly IServiceProvider serviceProvider;
        private readonly IMessaging<string> messageBus;
        private readonly IMailParser parser;
        private readonly IUserService userService;
        private readonly IMailServerRepository mailServer;
        private readonly IMailEntryRepository mailEntryRepository;
        private readonly IDocumentRepository documentService;
        private readonly IVirtualFileSystemProvider virtualFileSystemProvider;
        private readonly IConfiguration config;
        private readonly string queueName;

        public MailParsingSubscriber(ILogger<MailParsingSubscriber> logger,
                        IServiceProvider serviceProvider,
                        IMessaging<string> messageBus,
                        IMailParser mailParser,
                        IUserService userService,
                        IMailServerRepository mailServer,
                        IMailEntryRepository mailEntryRepository,
                        IDocumentRepository documentService,
                        IVirtualFileSystemProvider virtualFileSystemProvider,
                        IConfiguration config)
        {
            this.logger = logger;
            this.serviceProvider = serviceProvider;
            this.messageBus = messageBus;
            this.parser = mailParser;
            this.userService = userService;
            this.mailServer = mailServer;
            this.mailEntryRepository = mailEntryRepository;
            this.documentService = documentService;
            this.virtualFileSystemProvider = virtualFileSystemProvider;
            this.config = config;
            queueName = config[StaticConfiguration.CONST_MAILSERVICE_QUEUE];
        }

        public async Task Invoke(IEvent ApplicationEvent)
        {
            var e = ApplicationEvent.EventName;
            if (e == EventType.AddVersion || e == EventType.AddRevision)
            {
                var doc = (DocumentInfo)ApplicationEvent.Get<DocumentInfo>("Document");
                var documentId = doc.Id; 
                if (doc.Image != null && doc.Image.FileName.EndsWith(".eml"))
                {
                    var msg = "PARSE:" + documentId.ToString();

 //                   if (string.IsNullOrEmpty(queueName))
                    {
                        //logger.LogError("StartParsingMail", "<EmptyQueue>:" + msg);
                        _ = Task.Run(async () =>
                        {
                            await ParseMail(doc);
                        });

                    }

                    //try
                    //{
                    //    messageBus.PushMessage(msg, queueName);
                    //    logger.LogDebug("StartParsingMail", queueName + ":" + msg);
                    //}
                    //catch (Exception Ex)
                    //{
                    //    logger.LogError(Ex, "StartParsingMail", queueName + ":" + msg);
                    //    throw;
                    //}
                }
            }
            await Task.CompletedTask;

        }

        private async Task ParseMail(DocumentInfo MailDocument)
        {
            try
            {
                var fmanager = await virtualFileSystemProvider.InstanceOf(MailDocument.Image.FileManager);
                using (var msg = await fmanager.ReadAsStream(MailDocument.Image.FileName))
                    parser.Read(msg);

                // ESTRAZIONE MITTENTE E DESTINATARI //
                try
                {
                    List<DocumentRecipient> Recipients = new();
                    var Sender = parser.GetSender();
                    var sa = await GetAddress(Sender, MailDocument.Id, Domain.Enumerators.RecipientType.Sender);
                    if (sa != null) Recipients.Add(sa);
                    foreach (var address in parser.GetToList())
                    {
                        var a = await GetAddress(address, MailDocument.Id, Domain.Enumerators.RecipientType.To);
                        if (a != null) Recipients.Add(a);
                    }
                    foreach (var address in parser.GetCCList())
                    {
                        var a = (await GetAddress(address, MailDocument.Id, Domain.Enumerators.RecipientType.CC));
                        if (a != null) Recipients.Add(a);
                    }
                    foreach (var address in parser.GetCCrList())
                    {
                        var a = (await GetAddress(address, MailDocument.Id, Domain.Enumerators.RecipientType.CCr));
                        if (a != null) Recipients.Add(a);
                    }
                    await documentService.SaveRecipients(MailDocument.Id, Recipients);

                }
                catch (Exception ex)
                {
                    //TODO: In caso di errore di lettura della mail... chi avviso ?
                }

                // COLLEGAMENTO CON MAIL PRECEDENTE //
                try
                {
                    var uid = parser.GetMessageId();
                    foreach (var entry in await mailEntryRepository.GetMessagesById(uid))
                    {
                        if (entry.ParentId.HasValue)
                        {
                            var parent = await mailEntryRepository.GetById(entry.ParentId.Value);
                            if (parent.DocumentId > 0)
                            {
                                await documentService.AddLink(MailDocument.Id, parent.DocumentId, SpecialUser.SystemUser);
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    //TODO: In caso di errore di lettura della mail... chi avviso ?
                }

                // COLLEGAMENTO CON DOCUMENTI PRE-ARCHIVIATI //
                try
                {
                    // Verifico se nelle intestazioni del messaggio sono presenti riferimenti (id) 
                    // di documenti allegati nel messaggio 
                    foreach (var documentId in parser.GetDocuments())
                    {
                        await documentService.AddLink(MailDocument.Id, documentId, SpecialUser.SystemUser, true);
                    }

                    //NOTA: forse è meglio non estrarre in automatico tutti gli allegati di un messaggio
                    // ma lasciare all'utente questa facoltà, attraverso un'apposita funzione di estrazione

                }
                catch (Exception ex)
                {
                    //TODO: In caso di errore di lettura della mail... chi avviso ?
                }
            }
            catch (Exception ex)
            {
                //TODO: In caso di errore di lettura della mail... chi avviso ?
            }
        }


        private async Task<DocumentRecipient> GetAddress(LookupTable Sender, int documentId, RecipientType type)
        {
            string domain = Sender.Annotation;
            var mserver = await mailServer.GetByDomain(domain);
            var Mtype = Domain.Enumerators.DigitalAddressType.Email;
            if (mserver != null)
                Mtype = mserver.MailType == MailType.Mail ? Domain.Enumerators.DigitalAddressType.Email : Domain.Enumerators.DigitalAddressType.Pec;
            var address = await userService.GetOrCreateAddress(Sender.Description, Sender.Id, Mtype);

            DocumentRecipient dr = new();
            dr.RecipientType = type;
            dr.DocumentId = documentId;
            dr.ProfileType = Domain.Enumerators.ProfileType.MailAddress;
            dr.ProfileId = address.Id.ToString();

            return dr;
        }
    }
}