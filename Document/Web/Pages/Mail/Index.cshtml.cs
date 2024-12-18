using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MimeKit;
using OpenDMS.Core.DTOs;
using OpenDMS.Core.Filters;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Entities;
using OpenDMS.Domain.Entities.Mails;
using OpenDMS.Domain.Entities.Settings;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Services;
using OpenDMS.MailSpooler.Core.Interfaces;

namespace Web.Pages.MailConsole
{
    [Authorization(OpenDMS.Domain.Constants.PermissionType.MailConsole)]

    public class MailIndexModel : PageModel
    {
        public class MailFolder
        {
            public int MailboxId { get; set; }
            public string Id { get; set; }
            public string Icon { get; set; }
            public string Name { get; set; }
            public int Badge { get; set; }
            public string BadgeClass { get; set; }
        }

        public class MailboxInfo
        {
            public int Id { get; set; }
            public string Icon { get; set; }
            public string Address { get; set; }
            public string DisplayName { get; set; }
            public int Badge { get; set; }
            public bool Validated { get; set; }
            public string LastInbound { get; set; }
            public string LastOutbound { get; set; }
            public string InboxMessage { get; set; }
            public string OutboxMessage { get; set; }

            public MailServerStatus Status { get; set; }
            public LookupTable Company { get; set; }

            public List<MailFolder> Folders { get; set; } = new List<MailFolder>();
        }

        public bool CanSendMail { get; set; } = false;

        public List<MailboxInfo> Mailboxes { get; set; } = new List<MailboxInfo>();

        private string _contactId = "";
        private string _userId = "";
        private readonly ILoggedUserProfile loggedUserProfile;
        private readonly ICompanyService companyService;
        private readonly IUserService userService;
        private readonly IACLService aCLService;
        private readonly IMailReaderService mailReaderService;
        private readonly IMailEntryService mailEntryService;
        private readonly IMailboxService mailboxService;

        public MailIndexModel(
            ILoggedUserProfile loggedUserProfile,
            ICompanyService companyService,
            IUserService userService,
            IACLService aCLService,
            IMailReaderService mailReaderService,
            IMailEntryService mailEntryService,
            IMailboxService mailboxService
            )
        {
            this.loggedUserProfile = loggedUserProfile;
            this.companyService = companyService;
            this.userService = userService;
            this.aCLService = aCLService;
            this.mailReaderService = mailReaderService;
            this.mailEntryService = mailEntryService;
            this.mailboxService = mailboxService;
        }

        public async Task<MailboxInfo> GetMailboxInfo(Mailbox mbox, UserProfile u)
        {
            //            var mbox = await mailboxService.GetByAddress(M.Address);
            if (mbox != null)
            {
                var server = mbox.MailServer; // await MailServerRepository.GetById(mbox.MailServerId);
                var LastInbound = mbox.LastReceivingDate.HasValue ? mbox.LastReceivingDate.Value : mbox.EnableDownload ? DateTime.MaxValue :  DateTime.MinValue;
                var LastOutbound = mbox.LastSendingDate.HasValue ? mbox.LastSendingDate.Value : DateTime.MinValue;

                if (server != null)
                {
                    MailboxInfo MI = new()
                    {
                        Status = server.Status,
                        DisplayName = mbox.DisplayName,
                        Company = new LookupTable() { Id = "", Description = "" },
                        InboxMessage = mbox.LastReceivingError,
                        OutboxMessage = mbox.LastSendingError,
                        LastInbound = LastInbound == DateTime.MaxValue ? "" : LastInbound > DateTime.MinValue ? LastInbound.ToString("dd/MM/yyyy HH:mm:ss") : "Mai",
                        LastOutbound = LastOutbound > DateTime.MinValue ? LastOutbound.ToString("dd/MM/yyyy HH:mm:ss") : "Mai",
                        Validated = mbox.Validated,
                        Address = mbox.MailAddress,
                        Id = mbox.Id,
                        Icon = mbox.MailServer.MailType == MailType.Mail ? "fa fa-envelope-o" : "fa fa-envelope"
                    };
                    var downloadEnabled =
                        !String.IsNullOrEmpty(server.InboxServer) &&
                        mbox.EnableDownload &&
                        (
                            mbox.UserId == _userId
                            || (mbox.ReadOnlyProfiles != null &&
                                mbox.ReadOnlyProfiles.Split(',').Any(m => ("0" + m.ToLower() == _userId) || u.Roles.Any(r => "2" + r.Id.ToLower() == m.ToLower()) || u.Groups.Any(r => "1" + r.Id.ToLower() == m.ToLower()))
                               )
                        );
                    var draftEnabled =
                        //                       !String.IsNullOrEmpty(server.SMTPServer) &&
                        (
                            mbox.UserId == _userId
                            || (mbox.DraftEnabledProfiles != null &&
                                mbox.DraftEnabledProfiles.Split(',').Any(m => ("0" + m.ToLower() == _userId) || u.Roles.Any(r => "2" + r.Id.ToLower() == m.ToLower()) || u.Groups.Any(r => "1" + r.Id.ToLower() == m.ToLower()))
                               )
                        );

                    var sendEnabled =
                        //                       !String.IsNullOrEmpty(server.SMTPServer) &&
                        (
                            mbox.UserId == _userId
                            || (mbox.SendEnabledProfiles != null &&
                                mbox.SendEnabledProfiles.Split(',').Any(m => ("0"+m.ToLower() == _userId) || u.Roles.Any(r => "2"+r.Id.ToLower() == m.ToLower()) || u.Groups.Any(r => "1" + r.Id.ToLower() == m.ToLower()))
                               )
                        );


                    if (downloadEnabled)
                    {
                        MI.Folders.Add(new MailFolder { MailboxId=mbox.Id, Id = "INBOX", Icon = "fa fa-inbox", Name = "Ricevuta" });
                        //                        MI.Folders.Add(new MailFolder { Id = "VIRUS", Icon = "fa fa-bug", Name = "Infetta", BadgeClass = "warning" });
                        MI.Folders.Add(new MailFolder { MailboxId = mbox.Id, Id = "CLAIMED", Icon = "fa fa-envelope-open", Name = "Presa In Carico" });
                        MI.Folders.Add(new MailFolder { MailboxId = mbox.Id, Id = "STORED", Icon = "fa fa-check-circle", Name = "Gestita" });
                        MI.Folders.Add(new MailFolder { MailboxId = mbox.Id, Id = "SPAM", Icon = "fa fa-ban", Name = "Ignorata" });
                    }
                    if (draftEnabled || sendEnabled)
                    {
                        MI.Folders.Add(new MailFolder { MailboxId = mbox.Id, Id = "DRAFT", Icon = "fa fa-pencil", Name = "Bozze" });
                    }
//                    if (mbox.UserId == _userId)
//                        MI.Folders.Add(new MailFolder { MailboxId = mbox.Id, Id = "WAITING", Icon = "fa fa-hourglass", Name = "Da Approvare", BadgeClass = "warning" });
                    if (sendEnabled)
                    {
                        MI.Folders.Add(new MailFolder { MailboxId = mbox.Id, Id = "SENDING", Icon = "fa fa-rocket", Name = "In Spedizione", BadgeClass = "warning" });
                        MI.Folders.Add(new MailFolder { MailboxId = mbox.Id, Id = "SENT", Icon = "fa fa-paper-plane", Name = "Spedita" });
                        //                    if (M.ContactId == _contactId)
                        //                        MI.Folders.Add(new MailFolder { Id = "STORED", Icon = "fa fa-briefcase", Name = "Archivio" });

                        MI.Folders.Add(new MailFolder { MailboxId = mbox.Id, Id = "NOTSENT", Icon = "fa fa-bug", Name = "Non Spedita" });
                        //MI.Folders.Add(new MailFolder { Id = "ERRORS", Icon = "fa fa-exclamation-circle", Name = "In Errore", BadgeClass="error" });
                    }

                    if (downloadEnabled || sendEnabled)
                    {
                        MI.Folders.Add(new MailFolder { MailboxId = mbox.Id, Id = "DELETED", Icon = "fa fa-trash-o", Name = "Cestino" });
                    }
                    if (downloadEnabled || sendEnabled || draftEnabled)
                    {
                        await GetBadges(MI, u);
                        return MI;
                    }
                }
            }
            return null;
        }

        private async Task GetBadges(MailboxInfo MI, UserProfile u)
        {
            int total = 0;
            foreach (var M in MI.Folders)
            {
                MailMessagesFilter F = new();
                F.mailboxId = new[] { MI.Id };
                F.mailDirection = (M.Id == "INBOX" || M.Id == "VIRUS" || M.Id == "SPAM" || M.Id == "CLAIMED")
                    ? MailDirection.Inbound
                    : (M.Id == "DELETED" || M.Id == "STORED")
                        ? null
                        : MailDirection.Outbound;
                F.mailStatus = M.Id == "DRAFT" ? MailStatus.Draft :
                               M.Id == "WAITING" ? MailStatus.WaitingForApproval :
                               M.Id == "SPAM" ? MailStatus.Spam :
                               M.Id == "CLAIMED" ? MailStatus.Claimed :
                               M.Id == "SENDING" ? MailStatus.Sending :
                               M.Id == "SENT" ? MailStatus.Sent :
                               M.Id == "NOTSENT" ? MailStatus.Failed :
                               M.Id == "STORED" ? MailStatus.Archived :
                               M.Id == "DELETED" ? MailStatus.Deleted :
                               MailStatus.Received;
                if (M.Id != "SENT" && M.Id != "SPAM" && M.Id != "SPAM")
                {
                    var entries = await mailEntryService.Count(F, u);
                    M.Badge = entries;
                    if (F.mailDirection == MailDirection.Inbound)
                        total += entries;
                }
            }
            MI.Badge = total;
        }

        private async Task<List<MailMessageEntry>> GetEntries(MailboxInfo MI, string FolderId, int pageIndex, int pageSize, string Filter)
        {
            var u = loggedUserProfile.Get();
            MailMessagesFilter F = new();
            F.descriptionText = Filter;
            F.mailDirection = (FolderId == "INBOX" || FolderId == "VIRUS" || FolderId == "SPAM" || FolderId == "CLAIMED")
                ? MailDirection.Inbound
                : (FolderId == "DELETED" || FolderId == "STORED")
                    ? null
                    : MailDirection.Outbound;
            F.mailboxId = new[] { MI.Id };
            F.mailStatus = FolderId == "DRAFT" ? MailStatus.Draft :
                            FolderId == "WAITING" ? MailStatus.WaitingForApproval :
                            FolderId == "SPAM" ? MailStatus.Spam :
                            FolderId == "CLAIMED" ? MailStatus.Claimed :
                            FolderId == "SENDING" ? MailStatus.Sending :
                            FolderId == "SENT" ? MailStatus.Sent :
                            FolderId == "NOTSENT" ? MailStatus.Failed :
                            FolderId == "STORED" ? MailStatus.Archived :
                            FolderId == "DELETED" ? MailStatus.Deleted :
                            MailStatus.Received;

            List<MailMessageEntry> entries = new();
            F.pageIndex = pageIndex;
            F.pageSize = pageSize;
            foreach (var e in await mailEntryService.GetEntries(F, u))
            {
                var ME = new MailMessageEntry()
                {
                    Id = e.Id,
                    DocumentId = e.DocumentId,
                    ExternalMailAddress = e.Direction == MailDirection.Inbound ? e.ExternalMailAddress : e.InternalMailAddress,
                    InternalMailAddress = e.Direction == MailDirection.Inbound ? e.InternalMailAddress : e.ExternalMailAddress,
                    IsInfected = e.IsInfected,
                    IsSPAM = e.IsSPAM,
                    MessageDate = e.MessageDate.Value.ToString("dd/MM/yyyy HH:mm").Replace(DateTime.Now.ToString("dd/MM/yyyy"), "Oggi"),
                    NumberOfAttachments = e.NumberOfAttachments,
                    ParentId = e.ParentId,
                    Status = e.Status,
                    SubType = e.SubType,
                    Title = e.MessageTitle,
                    ClaimDate = e.ClaimDate.HasValue ? e.ClaimDate.Value.ToString("dd/MM/yyyy HH:mm") : "",
                    ArchiveDate = e.ArchivingDate.HasValue ? e.ArchivingDate.Value.ToString("dd/MM/yyyy HH:mm") : "",
                    DeleteDate = e.DeletionDate.HasValue ? e.DeletionDate.Value.ToString("dd/MM/yyyy HH:mm") : "",
                };
                var next = await mailEntryService.GetByParentId(e.Id);
                if (next.Any(m => m.SubType == MailSubType.DeliveryReceipt))
                    ME.DeliveryStatus = MailSubType.DeliveryReceipt;
                else
                if (next.Any(m => m.SubType == MailSubType.AcceptanceReceipt))
                    ME.DeliveryStatus = MailSubType.AcceptanceReceipt;

                //TODO: Archiviuser e DeletionUser
                ME.ClaimUser = string.IsNullOrEmpty(e.ClaimUser) ? "" : await userService.GetName(e.ClaimUser);
                ME.ArchiveUser = string.IsNullOrEmpty(e.ClaimUser) ? "" : await userService.GetName(e.ClaimUser);
                ME.DeleteUser = string.IsNullOrEmpty(e.DeletionUser) ? "" : await userService.GetName(e.DeletionUser);
                if (!String.IsNullOrEmpty(ME.InternalMailAddress))
                {
                    try
                    {
                        MailboxAddress A = MailboxAddress.Parse(ME.InternalMailAddress);
                        ME.InternalName = A.Name;
                        ME.InternalMailAddress = A.Address;
                    }
                    catch (Exception)
                    {
                        ME.InternalMailAddress = ME.InternalMailAddress;
                    }
                    if (string.IsNullOrEmpty(ME.InternalName)) ME.InternalName = ME.InternalMailAddress;
                }
                var a = await mailboxService.GetById(e.MailboxId);
                if (a != null)
                {
                    ME.UserId = a.UserId;
                    var company = await companyService.GetById(a.CompanyId);
                    if (company != null)
                        ME.Company = company.Description;
                    else
                        ME.Company = a.CompanyId.ToString();
                }

                if (!string.IsNullOrEmpty(ME.ExternalMailAddress))
                {
                    try
                    {
                        MailboxAddress B = MailboxAddress.Parse(ME.ExternalMailAddress);
                        ME.ExternalName = B.Name;
                        ME.ExternalMailAddress = B.Address;
                    }
                    catch (Exception)
                    {
                        ME.ExternalMailAddress = ME.InternalMailAddress;
                    }
                    //MailboxAddress B = MailboxAddress.Parse(ME.ExternalMailAddress);
                    //ME.ExternalMailAddress = B.Address;
                    //ME.ExternalName = B.Name;
                    if (string.IsNullOrEmpty(ME.ExternalName)) ME.ExternalName = ME.ExternalMailAddress;

                    var ce = await userService.FindMailAddress(ME.ExternalName, ME.ExternalMailAddress);
                    if (ce != null)
                    {
                        ME.ExternalContactId = ce.ContactId;
                    }
                }

                entries.Add(ME);
            }
            return entries;
        }

        public async Task OnGet()
        {
            var u = loggedUserProfile.Get();
            _userId = u.userId.ToLower();
            var user = await userService.GetById(_userId);
            _contactId = user.ContactId;
            CanSendMail = (await aCLService.GetAuthorization("", u, PermissionType.Profile_CanSendMail)) == OpenDMS.Domain.Enumerators.AuthorizationType.Granted;
            //var mails = (await mailboxService.GetAll(_userId)).Where(m => m.UserId == u.userId || m.SendEnabledProfiles.Split(",").Contains(_userId) || m.ReadOnlyProfiles.Split(",").Contains(_userId) || m.DraftEnabledProfiles.Split(",").Contains(_userId));
            //foreach (var M in mails)
            //{
            //    var MI = await GetMailboxInfo(M, u);
            //    if (MI != null)
            //        Mailboxes.Add(MI);
            //}
        }

        public async Task<ActionResult> OnGetBadges()
        {
            var u = loggedUserProfile.Get();
            _userId = u.userId;
            var user = u.UserInfo; // await userService.GetById(_userId);
            _contactId = user.ContactId;

            var mails = (await mailboxService.GetAll(u));
            foreach (var M in mails)
            {
                var MI = await GetMailboxInfo(M, u);
                if (MI != null)
                    Mailboxes.Add(MI);
            }
            return  new JsonResult( Mailboxes);
        }


        public async Task<ActionResult> OnGetCheckInBox(int mailboxId, string folderId)
        {
            var u = loggedUserProfile.Get();
            _userId = u.userId;
            var user = await userService.GetById(_userId);
            _contactId = user.ContactId;
            var mail = await mailboxService.GetById(mailboxId);
            await mailReaderService.Read(mail, u);
            return new OkResult();
        }

        public async Task<ActionResult> OnGetEntries(int mailboxId, string folderId, int pageIndex = 0, int pageSize = 5, string Filter = "")
        {
            List<MailMessageEntry> Entries = new();
            var u = loggedUserProfile.Get();
            _userId = u.userId;
            var user = await userService.GetById(_userId);
            _contactId = user.ContactId;
            var mail = await mailboxService.GetById(mailboxId);
            if (mail != null)
            {
                var MI = await GetMailboxInfo(mail, u);
                Entries = await GetEntries(MI, folderId, pageIndex, pageSize, Filter);
            }
            return new JsonResult(Entries);
        }
    }
}
