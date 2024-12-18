using OpenDMS.Domain.Models;
using OpenDMS.Domain.Entities;
using Newtonsoft.Json.Linq;
using OpenDMS.Domain.Events.Types;

namespace OpenDMS.Core.DTOs
{
    public class UserTaskEventMessage : EventMessage
    {
        public UserTaskEventMessage(UserTaskInfo task, UserProfile userInfo, string applicationEvent, string FormData, bool EventRaised=false)
            : base(userInfo, applicationEvent, null)
        {
            if (applicationEvent == EventType.UserTaskExecuted)
            {
                if (task.TaskItemInfo.Event.Id == EventType.Approval)
                {
                    // Verifico quale sia stata la scelta dell'utente
                    // Poichè la variabile "Choise" potrebbe assumere vari valori, verifico solo se è uguale a zero
                    // perche Zero indica un rifiuto.
                    string Choise = "";
                    try
                    {
                        Choise = JToken.Parse(FormData)["choise"]?.ToString();
                    }
                    catch (Exception)
                    {
                        try
                        {
                            Choise = JToken.Parse(FormData)["Choise"]?.ToString();
                        }
                        catch (Exception)
                        {
                        }
                    }
                    if (Choise == "0" || string.IsNullOrWhiteSpace(Choise))
                    {
                        // Ho rifiutato
                        applicationEvent = EventType.Refuse;
                    }
                    else
                    {
                        // Ho approvato
                        applicationEvent = EventType.Approval;
                    }
                }
            }
            Variables.Add("UserTaskId", task.Id);
            Variables.Add("TaskId", task.TaskItemInfo.Id);
            Variables.Add("UserTask", task);
            Variables.Add("FormData", FormData);
            Variables.Add("eventRaised", EventRaised ? "1" : "0");
            Variables.Add("EventRaised", EventRaised ? "1" : "0");
            Variables.Add("choise", EventRaised ? "1" : "0");
            Variables.Add("Choise", EventRaised ? "1" : "0");
            Variables.Add("TaskUser", userInfo.UserInfo);
            if (!Variables.ContainsKey("ProcessId")) Variables.Add("ProcessId", task.TaskItemInfo.ProcessDataId);
            if (task.TaskItemInfo.Attachments.Count > 0)
            {
                Variables.Add("Attachments", task.TaskItemInfo.Attachments.Select(t => t.Id).ToArray());
            }
        }

        public UserTaskEventMessage():base()
        {
        }



    }
}
