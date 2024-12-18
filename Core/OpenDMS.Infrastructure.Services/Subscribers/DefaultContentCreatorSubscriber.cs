using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenDMS.Core.DTOs;
using OpenDMS.Core.Interfaces;
using OpenDMS.Core.Utilities;
using OpenDMS.Domain.Entities;
using OpenDMS.Domain.Entities.Users;
using OpenDMS.Domain.Events.Types;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Services;
using System.Security;

namespace OpenDMS.Infrastructure.Services.Subscribers
{

    public class DefaultContentCreatorSubscriber : IEventSubscriber
    {

        private readonly ILogger<DefaultContentCreatorSubscriber> logger;
        private readonly IConfiguration config;
        private readonly IDocumentService documentService;

        public DefaultContentCreatorSubscriber(ILogger<DefaultContentCreatorSubscriber> logger,
            IConfiguration config,
            IDocumentService documentService)
        {
            this.logger = logger;
            this.config = config;
            this.documentService = documentService;
        }

        public async Task Invoke(IEvent ApplicationEvent)
        {
            if (ApplicationEvent.EventName == EventType.DefaultContent)
            {
                //entry.DeputyUserId = ApplicationEvent.UserInfo.LoggedUser;
                //entry.UserId = ApplicationEvent.UserInfo.userId;
                //entry.EventType = ApplicationEvent.EventName;
                var V = ApplicationEvent.Variables;
                var user = UserProfile.SystemUser();
                DocumentInfo document = (DocumentInfo)ApplicationEvent.Get<DocumentInfo>("Document");
                var documentId = document.Id;// int.Parse(V.ContainsKey("ObjectId") ? V["ObjectId"]?.ToString() : "0");
                if (documentId > 0)
                {
                    string DefaultData = "";
                    bool IsBase64 = false;
                    string FileName = "";
                    string Ext = "";
                    var docType = document.DocumentType.ContentType;
                    switch (docType)
                    {
                        case Domain.Enumerators.ContentType.Workflow:
                            DefaultData = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot", "templates", "BPMN_Documentale.bpmn"));
                            var inputVariables = ObjectExtensions.GetJson(
                                new(typeof(Domain.Entities.Documents.Document), "Document"),
                                //new(typeof(int), "DocumentId"),
                                new(typeof(string), "UserId"),
                                new(typeof(string), "EventName"),
                                new(typeof(UserProfile), "UserProfile")
                            );
                            inputVariables = new System.Xml.Linq.XText(inputVariables).ToString();
                            DefaultData = DefaultData.Replace("{inputVariables}", inputVariables);
                            IsBase64 = false;
                            Ext = ".bpmn";
                            break;
                        //case Domain.Enumerators.ContentType.Workflow:
                            DefaultData = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot", "templates", "BPMN_Manuale.bpmn"));
                            var inputVariables2 = ObjectExtensions.GetJson(
                                new(typeof(Domain.Entities.Documents.Document), "Document"),
                                new(typeof(Domain.Entities.Documents.Document), "ProcessInfo"),
                                new(typeof(User), "User"),
                                new(typeof(string), "UserId"),
                                new(typeof(UserProfile), "UserProfile")
                            );
                            inputVariables2 = SecurityElement.Escape(inputVariables2).ToString();
                            DefaultData = DefaultData.Replace("{inputVariables}", inputVariables2);
                            IsBase64 = false;
                            Ext = ".bpmn";
                            break;
                    case Domain.Enumerators.ContentType.Form:
                            DefaultData = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot", "templates", "Form_Base.form"));
                            IsBase64 = false;
                            Ext = ".form";
                            break;
                        //case "$FORM-IO$":
                        //    DefaultData = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot", "templates", "Form_Base.formio"));
                        //    IsBase64 = false;
                        //    Ext = ".formio";
                        //    break;
                        //case "$FORM-HTML$":
                        //    DefaultData = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot", "templates", "Form_Base.html"));
                        //    IsBase64 = false;
                        //    Ext = ".html";
                        //    break;
                        case Domain.Enumerators.ContentType.DMN:
                            DefaultData = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot", "templates", "DMN_Base.dmn"));
                            IsBase64 = false;
                            Ext = ".dmn";
                            break;
                        case Domain.Enumerators.ContentType.Template:
                            // Verifico se esiste un template
                            //if (Document.DocumentType.TemplateId > 0)
                            var templateId = await documentService.FindByUniqueId("", document.DocumentType.Id, Domain.Enumerators.ContentType.Document);
                            if (templateId > 0)
                            {
                                var template = await documentService.Load(templateId, user);
                                if (template != null && template.Image != null && template.Image.Id > 0)
                                {
                                    byte[] data = await documentService.GetContent(template.Image.Id);
                                    DefaultData = Convert.ToBase64String(data);
                                    IsBase64 = true;
                                    FileName = template.Image.FileName;
                                }
                            }
                            break;
                        default:
                            break;
                    }
                    if (!string.IsNullOrWhiteSpace(DefaultData))
                    {
                        FileName = document.DocumentType.Name + Ext;
                        DefaultData = DefaultData.Parse(document, "Document");

                        FileContent FC = new();
                        FC.FileData = DefaultData;
                        FC.LinkToContent = false;
                        FC.FileName = FileName;
                        FC.DataIsInBase64 = IsBase64;
                        await documentService.AddContent(documentId, user, FC, true);
                    }
                }
                //TODO: Gestire la creazione di contenuto iniziale di un documento
            }
        }
    }
}
