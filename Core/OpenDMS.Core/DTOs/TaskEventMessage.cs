using OpenDMS.Domain.Models;
using Newtonsoft.Json.Linq;

namespace OpenDMS.Core.DTOs
{
    public class TaskEventMessage : EventMessage
    {

        public TaskEventMessage(TaskItemInfo task, UserProfile userInfo, string applicationEvent, string FormData, bool EventRaised = false)
                        : base(userInfo, applicationEvent, null)

        {
            var json = JObject.Parse(FormData);
            if (json.ContainsKey("ExitCode") && !EventRaised && (!Variables.ContainsKey("ExitCode")))
                Variables.Add("ExitCode", json["ExitCode"]?.ToString() ?? "");
            if (EventRaised && (!Variables.ContainsKey("ExitCode")))
                Variables.Add("ExitCode", "1");

            if (!Variables.ContainsKey("EventRaised")) Variables.Add("EventRaised", EventRaised ? "1" : "0");
            var justification = "";

            if (json.ContainsKey("justification"))
            {
                justification = json["justification"]?.ToString() ?? "";
                json.Remove("justification");
            }
            if (json.ContainsKey("Justification"))
            {
                justification = json["Justification"]?.ToString() ?? "";
                json.Remove("Justification");
            }
            if (json.ContainsKey("ExitCode"))
                json.Remove("ExitCode");

            if (!Variables.ContainsKey("Justification")) Variables.Add("Justification", justification);
            if (!Variables.ContainsKey("justification")) Variables.Add("justification", justification);
            if (!Variables.ContainsKey("TaskId")) Variables.Add("TaskId", task.Id);
            if (!Variables.ContainsKey("Task")) Variables.Add("Task", task);
            if (!Variables.ContainsKey("FormData")) Variables.Add("FormData", json.ToString());
            if (!Variables.ContainsKey("TaskUser")) Variables.Add("TaskUser", userInfo.UserInfo);
            if (!Variables.ContainsKey("ProcessId")) Variables.Add("ProcessId", task.ProcessDataId);
            if (task.Attachments.Count > 0)
            {
                if (!Variables.ContainsKey("Attachments"))  Variables.Add("Attachments", task.Attachments.Select(t => t.Id).ToArray());
            }
        }

        public TaskEventMessage() : base()
        {
        }


    }
}
