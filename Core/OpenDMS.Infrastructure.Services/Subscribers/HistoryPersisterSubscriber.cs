using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using OpenDMS.Core.DTOs;
using OpenDMS.Domain.Entities.Documents;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Repositories;
using OpenDMS.Domain.Services;
using static thredds.catalog.ThreddsMetadata;

namespace OpenDMS.Infrastructure.Services.Subscribers
{
    public class HistoryPersisterSubscriber : IEventSubscriber
    {
        private readonly ILogger<ProcessStarterSubscriber> logger;
        private readonly IHistoryRepository historyRepo;
        private readonly IConfiguration config;

        public HistoryPersisterSubscriber(ILogger<ProcessStarterSubscriber> logger, IHistoryRepository historyRepo, IConfiguration config)
        {
            this.logger = logger;
            this.historyRepo = historyRepo;
            this.config = config;
        }

        public async Task Invoke(IEvent ApplicationEvent)
        {
            var V = ApplicationEvent.Variables;

            var props = (Dictionary<string, string>)(V.ContainsKey("PropertiesChanged") ? V["PropertiesChanged"] : null);
            var folder = (Document)(V.ContainsKey("Folder") ? V["Folder"] : null);
            var document  = V.ContainsKey("Document") ? ((DocumentInfo)V["Document"]) : null;
            var content = (DocumentImage)(V.ContainsKey("Content") ? V["Content"] : null);
            var attachment = (Document)(V.ContainsKey("Attachment") ? V["Attachment"] : null);
            var profile = (string)(V.ContainsKey("Profile") ? V["Profile"] : null);
            var permission = (string)(V.ContainsKey("Permission") ? V["Permission"] : null);
            var authorization = (AuthorizationType)(V.ContainsKey("Authorization") ? V["Authorization"] : AuthorizationType.None);

            var task = V.ContainsKey("Task")
                    ? (TaskItemInfo)V["Task"]
                    : V.ContainsKey("UserTask")
                        ? ((UserTaskInfo)V["UserTask"]).TaskItemInfo
                        : null;
            var process = (Document)(V.ContainsKey("Process") ? V["Process"] : null);
            var moved = (bool)(V.ContainsKey("Moved") ? V["Moved"] : false);
            var description = (string)(V.ContainsKey("Description") ? V["Description"].ToString() : "");
            var justification = (string)(V.ContainsKey("Justification") ? V["Justification"]?.ToString() ?? "" : "");
            if (string.IsNullOrEmpty(description)) description = justification;

            var details = new Dictionary<string, string>();
            if (profile != null)
            {
                var type =(ProfileType)(Convert.ToInt32(profile.Substring(0, 1)));
                var desc = type == ProfileType.User ? "Utente" : type == ProfileType.Role ? "Ruolo" : "Gruppo"; 
                profile = profile.Substring(1);
                details.Add(desc, profile.ToString());
            }
            if (permission != null)
            {
                details.Add(permission, authorization.ToString());
            }

            if (props != null)
            {
                foreach (var kv in props)
                    details.Add(kv.Key, kv.Value);
            }
            if (content != null)
            {
                var contentDescription = Path.GetFileName(content.FileName);
                details.Add("Nome File", contentDescription);

                var contentVer = content.VersionNumber.ToString() + "." + content.RevisionNumber.ToString("00");
                details.Add("Versione", contentVer);
            }
            if (folder != null)
            {
                var folderDescription = folder.Description;
                if (moved)
                {
                    folderDescription += " (spostato qui)";
                }
                details.Add("Fascicolo", folderDescription);
            }
            if (attachment != null)
            {
                var linkDescription = attachment.Description;
                details.Add(ApplicationEvent.EventName.ToLower().Contains("link") ? "Collegato" : "Allegato", linkDescription);
            }
            if (task != null)
            {
                var ToList = string.Join(",", task.ToList.Select(t => t.Description).ToArray());
                //if (!String.IsNullOrEmpty(task.Title))
                //{
                //    details.Add("Titolo", task.Title);
                //}
                details.Add("Da", task.SenderName);
                if (!String.IsNullOrEmpty(ToList))
                {
                    details.Add("A", ToList);
                }
                var CcList = string.Join(",", task.CCList.Select(t => t.Description).ToArray());
                if (!String.IsNullOrEmpty(CcList))
                {
                    details.Add("CC", CcList);
                }
                if (!String.IsNullOrEmpty(task.Event.Id))
                    details.Add("Evento", task.Event.Description);
                if (!String.IsNullOrEmpty(task.Process.Id))
                    details.Add("Processo", task.Process.Description);
                if (!String.IsNullOrEmpty(task.Description))
                {
                    details.Add("Messaggio", task.Description);
                }
            }

            //entry.Recipients ;
            HistoryEntry entry = new HistoryEntry();
            entry.CreationDate = DateTime.UtcNow;
            entry.DeputyUserId = ApplicationEvent.UserInfo.LoggedUser;
            entry.UserId = ApplicationEvent.UserInfo.userId;
            entry.EventType = ApplicationEvent.EventName;
            if (document != null)// && ApplicationEvent.EventName.StartsWith("Document."))
            {
                entry.Documents.Add(new HistoryDocument() { DocumentId = document.Id, ImageId = document.Image != null ? document.Image.Id : 0 });
            }
            else
            {
                if (V.ContainsKey("Attachments"))
                {
                    int[] docs = (int[])(V["Attachments"]);
                    foreach (var did in docs)
                    {
                        entry.Documents.Add(new HistoryDocument() { DocumentId = did, ImageId = 0 });
                    }
                } 
            }
            entry.Description = description;
            entry.Details = System.Text.Json.JsonSerializer.Serialize(details);
            entry.TaskId = task != null ? task.Id.ToString() : "";
            entry.WorkflowId = process != null ? process.Id.ToString() : task != null ? task.Process.Id : "";
            await historyRepo.Insert(entry);

            await Task.CompletedTask;
        }
    }
}
