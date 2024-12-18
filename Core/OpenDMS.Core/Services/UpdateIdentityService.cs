using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Entities;
using OpenDMS.Domain.Entities.Mails;
using OpenDMS.Domain.Entities.OrganizationUnits;
using OpenDMS.Domain.Entities.Settings;
using OpenDMS.Domain.Entities.Users;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Repositories;
using OpenDMS.Domain.Services;
using OpenDMS.Infrastructure.Repositories;
using System.Data;
using System.Security.Claims;


namespace OpenDMS.Core.BusinessLogic
{
    public class UpdateIdentityService : IUpdateIdentityService
    {
        // private readonly IUserRepository _repository;
        private readonly IUserService userService;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserSettingsRepository userSettings;
        private readonly IMailboxRepository mailboxRepository;
        private readonly IMailServerRepository mailServerRepository;
        private readonly ICompanyRepository companyRepository;
        private readonly IOrganizationUnitService _organizationService;

        public UpdateIdentityService(
            //            IUserRepository _repository,
            IUserService userService,
            IRoleRepository roleRepository,
            IUserSettingsRepository userSettings,
            IMailboxRepository mailboxRepository,
            IMailServerRepository mailServerRepository,
            ICompanyRepository companyRepository,
            IOrganizationUnitService organizationService)
        {
            //            this._repository = _repository;
            this.userService = userService;
            this._roleRepository = roleRepository;
            this.userSettings = userSettings;
            this.mailboxRepository = mailboxRepository;
            this.mailServerRepository = mailServerRepository ?? throw new ArgumentNullException(nameof(mailServerRepository));
            this.companyRepository = companyRepository;
            this._organizationService = organizationService;
        }


        public async Task<UserProfile> Update(ClaimsPrincipal Claim)
        {

            var uid = Claim.Identity.Name;
            var name = Claim.Claims.FirstOrDefault(c => c.Type == "name")?.Value;
            if (string.IsNullOrEmpty(name)) name = uid;
            var mail = Claim.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
            var cid = Claim.Claims.FirstOrDefault(c => c.Type == "sid")?.Value;
            try
            {
                var U = await userService.GetById(uid);

                var Exists = U != null;
                if (!Exists)
                {
                    if (string.IsNullOrEmpty(cid)) cid = Guid.NewGuid().ToString();
                    //aggiungo l'utente all'archivio
                    U = new User();
                    U.Id = uid;
                    U.ContactId = cid;
                    U.Email = mail;
                    U.Contact = new Contact() { Id = cid, FullName = name, FriendlyName = name, SearchName = name, Parent = null };
                    U = await userService.AddOrUpdate(U);
                    Exists = U != null;
                }
                if (Exists)
                {
                    if (!String.IsNullOrEmpty(mail) && mail.Contains("@"))
                    {
                        // Verifico se la mail è censita nel profilo
                        var addresses = (await userService.GetAllContactDigitalAddress(uid)).FirstOrDefault(m => m.Address.Equals(mail, StringComparison.InvariantCultureIgnoreCase));
                        if (addresses == null)
                        {
                            U.Contact.DigitalAddresses.Add(new ContactDigitalAddress()
                            {
                                ContactId = cid,
                                Contact = U.Contact,
                                Address = mail,
                                Name = mail,
                                SearchName = mail,
                                CreationUser = uid,
                                DigitalAddressType = Domain.Enumerators.DigitalAddressType.Email
                            });
                            U = await userService.AddOrUpdate(U);
                        }

                        // Verifico se la mail è censita nella tabella delle mailbox
                        // Potrebbe non essere presente se il corrispondente MailServer non è stato censito
                        var mailbox = await mailboxRepository.GetByAddress(mail);
                        if (mailbox == null)
                        {
                            var mailserver = await mailServerRepository.GetByDomain(mail.Split("@")[1]);
                            if (mailserver == null)
                            {
                                mailserver = new MailServer()
                                {
                                    Domain = mail.Split("@")[1],
                                    MailType = MailType.Mail,
                                    Status = MailServerStatus.Disabled
                                };
                                await mailServerRepository.Insert(mailserver);
                            };
                            if (mailserver != null)
                            {
                                Mailbox M = new Mailbox() { UserId = uid, CompanyId = 1, DisplayName = name, FirstReceivingMessageDate = DateTime.UtcNow, MailAddress = mail, MailServerId = mailserver.Id };
                                await mailboxRepository.Insert(M);
                            }
                        }
                        var notificationType = (await userSettings.Get(cid, UserAttributes.CONST_NOTIFICATION_TYPE))?.ToLower();
                        if (notificationType != "no")
                        {
                            // Verifico se la mailbox è ancora presente
                            var NotificationAddress = await userSettings.Get(cid, UserAttributes.CONST_NOTIFICATION_MAIL);
                            if (!String.IsNullOrEmpty(NotificationAddress))
                            {
                                var mbox = await mailboxRepository.GetByAddress(NotificationAddress);
                                if (mailbox == null)
                                {
                                    NotificationAddress = "";
                                }
                            }
                            if (String.IsNullOrEmpty(NotificationAddress))
                                if (mailbox != null)
                                    if (mailbox.UserId.Equals(uid, StringComparison.InvariantCultureIgnoreCase))
                                        await userSettings.Set(cid, UserAttributes.CONST_NOTIFICATION_MAIL, mailbox.Id.ToString());
                        }
                    }

                    string RolesAdPersonamId = null;
                    var groups = await _organizationService.GetGroupsByUser(uid);
                    //ruoli generici per l'utente
                    var roles = groups.Where(g => g.UserGroupId == RolesAdPersonamId).ToList();

                    // Verifico l'esistenza dei ruoli
                    foreach (var role in Claim.Claims.Where(c => c.Type == "roles"))
                    {
                        var r = await _roleRepository.GetById(role.Value);
                        if (r == null)
                        {
                            r = new Role();
                            r.Id = role.Value;
                            r.RoleName = role.Value;
                            await _roleRepository.Insert(r);
                        }
                        var g = roles.FirstOrDefault(g => g.RoleId == role.Value);
                        if (g == null)
                        {
                            // Inserisco l'utente nel gruppo "$ROLES$"
                            UserInGroup UG = new UserInGroup();
                            UG.UserGroupId = RolesAdPersonamId;
                            UG.UserGroup = new UserGroup() { ShortName = "", Name = "" };
                            UG.RoleId = role.Value;
                            UG.RoleName = role.Value;
                            UG.UserId = uid;
                            UG.UserName = name;
                            UG.StartDate = DateTime.UtcNow;
                            await _organizationService.AddUser(UG);
                            groups.Add(UG);
                        }
                        else
                        {
                            roles.Remove(g);
                        }
                    }
                    // se è rimasto un ruolo precedentemente assegnato (roles) lo tolgo perchè l'utente non lo ha più
                    // tra i claim
                    if (roles.Count > 0)
                    {
                        // Rimuovo i ruoli non più attivi
                        foreach (var UG in roles)
                        {
                            if (UG.EndDate == null)
                            {
                                UG.EndDate = DateTime.UtcNow;
                                await _organizationService.EditUser(UG);
                            }
                        }
                    }
                }
                return await userService.GetUserProfile(uid);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }



    }
}
