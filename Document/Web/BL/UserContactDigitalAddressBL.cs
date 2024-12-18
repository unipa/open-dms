using AutoMapper;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Entities;
using OpenDMS.Domain.Entities.Mails;
using OpenDMS.Domain.Entities.Settings;
using OpenDMS.Domain.Entities.Users;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Repositories;
using OpenDMS.Domain.Services;
using System.Reflection;
using Web.BL.Interface;
using Web.DTOs;
using Web.Model.Customize;

namespace Web.BL
{
    public class UserContactDigitalAddressBL : IUserContactDigitalAddressBL
    {
        private readonly IConfiguration _config;
        private readonly string AdminHost;
        private readonly string IdentityHost;
        private IHttpContextAccessor _accessor;
        private readonly IUserService _userService;
        private readonly ILoggedUserProfile _userContext;
        private readonly IMailboxService _mailboxService;
        private readonly ICompanyService _companyRepo;
        private readonly IMailServerRepository _mailServerRepo;

        public UserContactDigitalAddressBL(IConfiguration config, IHttpContextAccessor accessor, IUserService userService, ILoggedUserProfile userContext, IMailboxService mailboxService, IMailServerRepository mailServerRepo, ICompanyService companyRepo)
        {
            _config = config;
            AdminHost = (string)_config.GetValue(typeof(string), "Endpoint:AdminService");
            IdentityHost = (string)_config.GetValue(typeof(string), "Endpoint:UserService");
            _accessor = accessor;
            _userService = userService;
            _userContext = userContext;
            _mailboxService = mailboxService;
            _mailServerRepo = mailServerRepo;
            _companyRepo = companyRepo;
        }

        public async Task<List<ContactDigitalAddress_DTO>> GetDigitalAddresses()
        {
            var u = _userContext.Get();
            var list = await _userService.GetAllContactDigitalAddress(u.userId);
            List<ContactDigitalAddress_DTO> castedList = new List<ContactDigitalAddress_DTO>();

            var config = new MapperConfiguration(cfg => cfg.CreateMap<ContactDigitalAddress, ContactDigitalAddress_DTO>());
            Mapper mp = new Mapper(config);

            foreach (var contact in list)
            {
                if (!contact.Deleted) 
                    castedList.Add(mp.Map<ContactDigitalAddress_DTO>(contact));
            }

            var mb_list = await _mailboxService.GetAll(u);

            foreach (var item in castedList)
            {

                var mb = mb_list.FirstOrDefault(mb => mb.MailAddress.Equals(item.Address));
                if (mb == null) continue;
                item.Validated = mb.Validated;
                item.EnableDownload = mb.EnableDownload;
            }

            return castedList;
        }

        public async Task<User> GetUserInfo()
        {
            return await _userService.GetById(_userContext.Get().userId);
        }

        public async Task<IEnumerable<MailServer>> GetAllMailServer()
        {
            return await _mailServerRepo.GetAll();
        }

        public async Task<int> GetMailServerByDomain(string domain)
        {
            var msList = await GetAllMailServer();
            return msList.FirstOrDefault(ms => ms.Domain.Equals(domain)).Id;
        }

        public async Task<MailServer> GetMailServerById(int Id)
        {
            return await _mailServerRepo.GetById(Id);
        }

        public async Task<List<Company>> GetCompanies()
        {
            return (await _companyRepo.GetAll()).ToList();
        }

        public async Task<Mailbox> GetMailboxById(int mailboxId)
        {
            return await _mailboxService.GetById(mailboxId);
        }

        public async Task<Mailbox> SetRecapito(Mailbox_DTO bd, int ContactDigitalAddressId)
        {
            var user = _userContext.Get().userId;
            // verifico che nesusn altro abbia questa mail in carico
            var mailbox = await _mailboxService.GetByAddress(bd.MailAddress);
            if (mailbox != null &&  mailbox.UserId != null && !mailbox.UserId.Equals(user, StringComparison.InvariantCultureIgnoreCase))
                throw new Exception("Questa casella è già intestata ad un altro utente");
            if (bd.MailServerId <= 0)
            {
                throw new Exception("non hai indicato un dominio valido");
            };


            var cda = await _userService.GetDigitalAddressById(ContactDigitalAddressId);
            //cerco tra i CDA eliminati nel caso in cui lo trovo, lo aggiorno e lo riabilito
            ContactDigitalAddress ContactDigitalAddress = (await _userService.GetAllDeletedContactDigitalAddress(user)).FirstOrDefault(i => i.Address.Equals(bd.MailAddress));

            //Set ContactDigitalAddress
            if (ContactDigitalAddress == null)
                ContactDigitalAddress = new ContactDigitalAddress();
            else
            {
                await _userService.DeleteContactDigitalAddress(ContactDigitalAddressId, user);
                ContactDigitalAddressId = ContactDigitalAddress.Id;
                cda = ContactDigitalAddress;
            }

            ContactDigitalAddress.Id = ContactDigitalAddressId;
            ContactDigitalAddress.ContactId = (await GetUserInfo()).ContactId;

            ContactDigitalAddress.DigitalAddressType = (await GetMailServerById(bd.MailServerId)).MailType == MailType.Mail ? DigitalAddressType.Email : DigitalAddressType.Pec;
            ContactDigitalAddress.Name = bd.DisplayName;
            ContactDigitalAddress.SearchName = bd.DisplayName;
            ContactDigitalAddress.Address = bd.MailAddress;
            ContactDigitalAddress.Deleted = false;
            var SetContactDigitalAddress_res = await SetContactDigitalAddress(ContactDigitalAddress);
            // SetContactDigitalAddress_res = Item1 : Oggetto aggiornato  , Item2 : oggetto vecchio (null nel caso in cui non esisteva)

            if (SetContactDigitalAddress_res.Item1 != null)
            {
                //una volta creato o aggiornato il ContactDigitalAddress setto l'oggetto Mailbox
                try { 
                    return await SetMailbox(bd, user); 
                }
                catch (Exception ex)
                {
                    ////nel caso in cui il set del Mailbox fallisce elimino il ContactDigitalAddress appena creato per non avere un record non accoppiato a una mailbox
                    //if(SetContactDigitalAddress_res.Item2 == null) await DeleteContactDigitalAddress(SetContactDigitalAddress_res.Item1.Id);
                    //else await SetContactDigitalAddress(SetContactDigitalAddress_res.Item2);
                    throw;
                }
            }
            else throw new Exception("Errore sul server durante il salvataggio del ContactDigitalAddress.");

        }

        public async Task DeleteRecapito(int DigitalAddressId, string MailboxAddress)
        {
            //Delete ContactDigitalAddress
            if (await DeleteContactDigitalAddress(DigitalAddressId) > 0)
                await DeleteMailbox(MailboxAddress);//una volta cancellato il ContactDigitalAddress cancello l'oggetto Mailbox
        }


        private async Task<Mailbox> GetMailboxByAddress(string address)
        {
            return await _mailboxService.GetByAddress(address);
        }

        private async Task<Tuple<ContactDigitalAddress, ContactDigitalAddress>> SetContactDigitalAddress(ContactDigitalAddress bd)
        {
            ContactDigitalAddress_DTO exist = bd.Id != 0 ? (await GetDigitalAddresses()).FirstOrDefault(x => x.Id == bd.Id) : null; // se 0 non c'è bisogno di cercare

            //controllo se esiste fra i cancellati
            if (exist == null)
            {
                var DeletedExist = (await _userService.GetAllDeletedContactDigitalAddress(_userContext.Get().userId))
                                    .FirstOrDefault(i => i.Address.Equals(bd.Address));
                var config = new MapperConfiguration(cfg => cfg.CreateMap<ContactDigitalAddress, ContactDigitalAddress_DTO>());
                Mapper mp = new Mapper(config);
                exist = mp.Map<ContactDigitalAddress_DTO>(DeletedExist);
            }

            ContactDigitalAddress old_CDA = null;

            if (exist != null)
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<ContactDigitalAddress_DTO, ContactDigitalAddress>());
                Mapper mp = new Mapper(config);
                old_CDA = mp.Map<ContactDigitalAddress>(exist);
            }

            ContactDigitalAddress r = null;
            if (await _userService.AddOrUpdateAddress(bd, _userContext.Get().userId) > 0)
                r = await _userService.GetDigitalAddressById(bd.Id);

            return new Tuple<ContactDigitalAddress, ContactDigitalAddress>(r, old_CDA); //ritorno anche l'oggetto vecchio in caso mi serva per ripristinarlo a prima della modifica

        }

        private async Task<Mailbox> SetMailbox(Mailbox_DTO bd, string uid)
        {
            var mb = await GetMailboxById(bd.Id);

            var config = new MapperConfiguration(cfg => cfg.CreateMap<Mailbox_DTO, Mailbox>());
            Mapper mp = new Mapper(config);
            var mbd = mp.Map<Mailbox>(bd);
            mbd.UserId = uid;
            if (mb == null)
            {
                mbd.Id = 0;
                await _mailboxService.Create(mbd);
            }
            else
            {
                await _mailboxService.Update(mbd);
            }
            return await _mailboxService.GetById(mbd.Id);

        }

        private async Task<int> DeleteContactDigitalAddress(int DigitalAddressId)
        {
            return await _userService.DeleteContactDigitalAddress(DigitalAddressId, _userContext.Get().userId);
        }

        private async Task<int> DeleteMailbox(string MailboxAddress)
        {
            var mb = await _mailboxService.GetByAddress(MailboxAddress);
            if (mb != null)
                return await _mailboxService.Delete(mb.Id);
            else
                return 0;
        }

    }
}
