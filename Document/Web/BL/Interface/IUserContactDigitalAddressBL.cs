using OpenDMS.Domain.Entities.Mails;
using OpenDMS.Domain.Entities.Settings;
using OpenDMS.Domain.Entities.Users;
using Web.DTOs;
using Web.Model.Customize;

namespace Web.BL.Interface
{
    public interface IUserContactDigitalAddressBL
    {
        Task DeleteRecapito(int DigitalAddressId, string MailboxAddress);
        Task<IEnumerable<MailServer>> GetAllMailServer();
        Task<List<Company>> GetCompanies();
        Task<List<ContactDigitalAddress_DTO>> GetDigitalAddresses();
        Task<Mailbox> GetMailboxById(int mailboxId);
        Task<int> GetMailServerByDomain(string domain);
        Task<MailServer> GetMailServerById(int Id);
        Task<User> GetUserInfo();
        Task<Mailbox> SetRecapito(Mailbox_DTO bd, int ContactDigitalAddressId);
    }
}