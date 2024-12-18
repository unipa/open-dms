using Web.Model.Admin;

namespace Web.BL.Interface
{
    public interface IMailServerBL
    {
        Task Delete(int Id);
        Task<IEnumerable<MailServer_DTO>> GetAllMailServer();
        Task<MailServer_DTO> GetMailServerById(int Id);
        Task Insert(MailServer_DTO bd);
        Task Update(MailServer_DTO bd);
    }
}