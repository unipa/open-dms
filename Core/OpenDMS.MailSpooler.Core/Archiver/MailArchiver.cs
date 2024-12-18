//using MimeKit;
//using OpenDMS.Core.DTOs;
//using OpenDMS.Core.Interfaces;
//using OpenDMS.Domain.Constants;
//using OpenDMS.Domain.Entities;
//using OpenDMS.Domain.Models;
//using OpenDMS.Domain.Repositories;
//using OpenDMS.Domain.Services;
//using OpenDMS.MailSpooler.Core.Interfaces;

//namespace OpenDMS.MailSpooler.Core.Archiver
//{
//    public class MailArchiver : IMailArchiver
//    {
//        private readonly IDocumentService documentService;
//        private readonly IDocumentTypeService documentTypeService;
//        private readonly IUserService userService;
//        private readonly IMailEntryService mailEntryService;
//        private readonly IMailboxService mailboxService;
//        private readonly IWorkflowEngine workflowInterface;

//        public MailArchiver(IDocumentService documentService,
//            IDocumentTypeService documentTypeService,
//            IUserService userService,
//            IMailEntryService mailEntryService,
//            IMailboxService mailboxService,
//            IWorkflowEngine workflowInterface
//            )
//        {
//            this.documentService = documentService;
//            this.documentTypeService = documentTypeService;
//            this.userService = userService;
//            this.mailEntryService = mailEntryService;
//            this.mailboxService = mailboxService;
//            this.workflowInterface = workflowInterface;
//        }

//        public async Task<MailEntry> Archive(MailEntry entry, string userId, MimeMessage message, string workerId = "Interactive")
//        {
//            if (await mailEntryService.Take(entry.Id, workerId, DateTime.UtcNow.AddMinutes(5)))
//            {
//                CreateOrUpdateDocument d = new CreateOrUpdateDocument();
//                {
//                    // In caso di errore archivio la mail con il template predefinito
//                    string mtype = entry.MailType == MailType.Mail ? "$MAIL" : "$PEC";
//                    string doctype = mtype + "-INBOUND$";
//                    if (entry.Direction == MailDirection.Outbound)
//                    {
//                        doctype = mtype + "-OUTBOUND$";
//                    }

//                    var tp = await documentTypeService.GetById(doctype);
//                    d = new CreateOrUpdateDocument();
//                    d.Status = Domain.Enumerators.DocumentStatus.Active;
//                    d.DocumentDate = entry.ArchivingDate;
//                    d.ContentType = Domain.Enumerators.ContentType.Document;
//                    d.Description = entry.MessageTitle;
//                    // memorizzo la casella destinataria/mittente + uid
//                    d.ExternalId = entry.InternalMailAddress + "_" + entry.MessageUID;
//                    d.DocumentTypeId = tp.Id;
//                    if (entry.MailType == MailType.Mail)
//                    {
//                        d.Icon = "fa fa-envelope-o";
//                    }
//                    else
//                    {
//                        d.Icon = "fa fa-envelope";
//                        d.IconColor = "firebrick";
//                    }
//                    d.Reserved = tp.Reserved;
//                    d.PersonalData = tp.PersonalData;
//                }

//                var mbox = await mailboxService.GetByAddress(entry.InternalMailAddress);
//                d.CompanyId = mbox.CompanyId;
//                d.Owner = mbox.UserId;

//                d.Content = new FileContent();
//                d.Content.FileName = entry.FilePath;
//                using (var M = new MemoryStream())
//                {
//                    await message.WriteToAsync(M);
//                    d.Content.FileData = System.Text.Encoding.Default.GetString(M.ToArray());
//                }
//                //d.Content.FileData = await fmanager.ReadAllText(entry.FilePath);
//                d.Content.DataIsInBase64 = false;

//                d.Content.ExtractAttachment = false; // true;

//                //TODO: Leggere le intestazioni per trovare il fascicolo di riferimento
//                //TODO: Collegare gli allegati archiviati al documento mail principale
//                //TODO: Archiviare gli indirizzi mittente e destinatari come DocumentRecipient

//                var u = await userService.GetUserProfile(SpecialUser.SystemUser);
//                try
//                {
//                    //TODO: Mettere in transazione
//                    var di = await documentService.Create(d, u);
//                    if (di != null)
//                    {
//                        await CheckRelations(entry.Id, entry, u);
//                        entry.DocumentId = di.Id;
//                        entry.ImageId= di.Image.Id;
//                        if (entry.Direction == MailDirection.Inbound)
//                        {
//                            entry.Status = MailStatus.Claimed;
//                            entry.ClaimDate = DateTime.UtcNow;
//                            entry.ClaimUser = userId;
//                        }
//                    }
//                }
//                catch (Exception ex)
//                {
//                    //TODO: Memorizzare nell'entry il messaggi di errore
//                    entry.LastException = ex.Message;
//                }
//                entry.WorkerId = "";

//                await mailEntryService.Update(entry);
//                //}
//                //else
//                //{
//                //    entry.LastException = $"File {entry.FilePath} non trovato";
//                //    entry.WorkerId = "";
//                //    await mailEntryRepository.Update(entry);
//                //}
//            }
//            return entry;
//        }

//        //private Task SaveAttachments(MailEntry entry, int id, UserProfile u)
//        //{
//        //    //throw new NotImplementedException();
//        //    return Task.CompletedTask;
//        //}

//        private async Task CheckRelations(int id, MailEntry entry, UserProfile u)
//        {
//            if (entry.ParentId.HasValue)
//            {
//                MailEntry parent = await mailEntryService.GetById(entry.ParentId.Value);
//                var masterdocumentid = parent.DocumentId;
//                // Collego il documento di risposta a quello iniziale
//                // Lo registro come allegato perchè quasi sicuramente, il master è incluso nel msg
//                await documentService.AddLink(id, masterdocumentid, u, true);
//            }
//        }

//        //private Task SaveRecipients(MailEntry entry, int id, UserProfile u)
//        //{
//        //    documentService.rec

//        //    return Task.CompletedTask;
//        //}
//    }
//}
