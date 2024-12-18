using OpenDMS.Domain.Models;

namespace OpenDMS.Core.DTOs
{
    public class DocumentEventMessage :  EventMessage
    {
        public DocumentEventMessage(DocumentInfo document, UserProfile userInfo, string applicationEvent, Dictionary<string, object> variables)
            : base (userInfo, applicationEvent, variables)
        {
            if (!this.Variables.ContainsKey("ProcessId")) this.Variables.Add("ProcessId", document.Id);
            if (!this.Variables.ContainsKey("DocumentId")) this.Variables.Add("DocumentId", document.Id);
            if (!this.Variables.ContainsKey("Document")) this.Variables.Add("Document", document);
        }

        public DocumentEventMessage():base()
        {
        }




    }
}
