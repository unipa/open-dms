using OpenDMS.Domain.Enumerators;

namespace OpenDMS.Domain.Models;

public class DocumentNotification
{
//    public string TenantId { get; set; } = "";

    public List<int> Documents { get; set; } = new List<int>();
    public string Sender { get; set; } = "";
    public int CompanyId { get; set; } = 0;
    //    public int Priority { get; set; } = 0;
    public bool Exception { get; set; }
    public bool AssignToAllUsers { get; set; } = false;

    public DateTime NotificationDate { get; set; } = DateTime.UtcNow;
    public ActionRequestType RequestType { get; set; } = ActionRequestType.None;

    public string TemplateId { get; set; } = "";
    public string Title { get; set; } = "";
    public string Message { get; set; } = "";

    public List<ProfileInfo> To { get; set; } = new List<ProfileInfo>();
    public List<ProfileInfo> CC { get; set; } = new List<ProfileInfo>();
//    public List<ProfileInfo> CCr { get; set; } = new List<ProfileInfo>();

}