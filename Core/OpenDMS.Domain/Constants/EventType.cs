namespace OpenDMS.Domain.Events.Types;


public class EventType
{
    /* DOCUMENTI */
    public const string Creation = "Document.Creation";
    public const string CreationAsDraft = "Document.CreationAsDraft";
    public const string Update = "Document.Update";
    public const string Classify = "Document.Classification";
    public const string Protocol = "Document.Protocol";
    public const string ChangeStatus = "Document.StatusChanged";
    public const string Delete = "Document.Delete";
    public const string UnDelete = "Document.Restore";
    public const string Expiration = "Document.Expiration";
    public const string EMail = "Document.Send";
    public const string Share = "Document.Share";
    public const string Authorize = "Document.PermissionChanged";
    public const string Comment = "Document.CommentAdded";
    public const string RunProcess = "Document.ProcessStarted";
    public const string AddToFolder = "Document.AddToFolder";
    public const string RemoveFromFolder = "Document.RemoveFromFolder";
    public const string DefaultContent = "Document.DefaultContent";
    public const string ExpirationUpdated = "Document.ExpirationDateUpdated";

    /* CONTENUTO */
    public const string View = "Document.View";
    public const string Download = "Document.Download";
    public const string Print = "Document.Print";
    public const string AddVersion = "Document.AddVersion";
    public const string AddRevision = "Document.AddRevision";
    public const string RemoveVersion = "Document.RemoveVersion";
    public const string RemoveRevision = "Document.RemoveRevision";
    public const string NoContentUpdate = "Document.NoContentUpdate";
    public const string Convert = "Document.Convert";
    public const string EraseContent = "Document.EraseContent";
    public const string ObscureContent = "Document.ObscureContent";
    public const string HighlightContent = "Document.HighlightContent";
    public const string AddTextToContent = "Document.AddTextToContent";
    public const string AddProtocolSign = "Document.AddProtocolSign";
    public const string AddStamp = "Document.AddStamp";
    public const string AddSignatureField = "Document.AddSignatureField";
    public const string AddCheckSign = "Document.AddCheckSign";
    public const string AddUserSignature = "Document.AddUserSignature";
    public const string AddDigitalSignature = "Document.AddDigitalSignature";
    public const string AddBiometricalSignature = "Document.AddBiometricalSignature";
    public const string AddRemoteDigitalSignature = "Document.AddRemoteDigitalSignature";
    public const string Preservation = "Document.Preservation";
    public const string AddPreservationSignature = "Document.AddPreservationSignature";
    public const string Publish = "Document.Publish";
    public const string PrepareForSending = "Document.PrepareForSending";
    public const string ExcludeFromSending = "Document.ExcludeFromSending";
    public const string CheckIn = "Document.CheckIn";
    public const string CheckOut = "Document.CheckOut";
    public const string AddLink = "Document.AddLink";
    public const string LinkRemoved = "Document.RemoveLink";
    public const string AddAttach = "Document.AddAttach";
    public const string AttachRemoved = "Document.RemoveAttach";
    public const string Copy = "Document.Copy";

    /* APPLICATION */
    public const string Timer = "Application.Timer";
    public const string Exception = "Application.Exception";

    /* TASKS */
    public const string TaskCreated = "Task.Created";
    public const string TaskRunning = "Task.Running";
    public const string TaskExecuted = "Task.Executed";
    public const string TaskExpired = "Task.Expired";
    public const string TaskValidated = "Task.Validated";
    public const string TaskDeleted = "Task.Deleted";

    /* USER TASKS */
    public const string UserTaskViewed = "UserTask.Viewed";
    public const string UserTaskClaimed = "UserTask.Claimed";
    public const string UserTaskReleased = "UserTask.Released";
    public const string UserTaskRejected = "UserTask.Rejected";
    public const string UserTaskReassigned = "UserTask.Reassigned";
    public const string UserTaskExecuted = "UserTask.Executed";

    public const string Request = "UserTask.Request";
    public const string Approval = "UserTask.Approve";
    public const string Refuse = "UserTask.Refuse";
}
