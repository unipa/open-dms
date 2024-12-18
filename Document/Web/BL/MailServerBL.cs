using AutoMapper;
using OpenDMS.Domain.Entities.Mails;
using OpenDMS.Domain.Repositories;
using Web.BL.Interface;
using Web.Model.Admin;

namespace Web.BL
{
    public class MailServerBL : IMailServerBL
    {
        private readonly IMailServerRepository mailServerRepo;

        public MailServerBL(IMailServerRepository mailServerRepo)
        {
            this.mailServerRepo = mailServerRepo;
        }

        public async Task<IEnumerable<MailServer_DTO>> GetAllMailServer()
        {
            var res = await mailServerRepo.GetAll();

            var config = new MapperConfiguration(cfg => cfg.CreateMap<MailServer, MailServer_DTO>());
            Mapper mp = new Mapper(config);

            var result = new List<MailServer_DTO>();

            foreach (var mailServer in res)
            {
                result.Add(mp.Map<MailServer_DTO>(mailServer));
            }

            return result;

        }

        public async Task<MailServer_DTO> GetMailServerById(int Id)
        {
            var res = await mailServerRepo.GetById(Id);

            var config = new MapperConfiguration(cfg => cfg.CreateMap<MailServer, MailServer_DTO>());
            Mapper mp = new Mapper(config);
            return mp.Map<MailServer_DTO>(res);
        }

        public async Task Insert(MailServer_DTO bd)
        {
            if (await mailServerRepo.GetById(bd.Id) == null)
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<MailServer_DTO, MailServer>());
                var mp = new Mapper(config);
                var insert = mp.Map<MailServer>(bd);
                insert.Id = 0;
                var r = await mailServerRepo.Insert(insert);
                if (r == 0) throw new Exception("L'inserimento non è andato a buon fine.");
            }
            else throw new Exception("Non può essere inserita un MailServer con un Id già esistente, usa un altro Id oppure se vuoi modificarla usa il metodo PUT.");
        }

        public async Task Update(MailServer_DTO bd)
        {
            var exist = await mailServerRepo.GetById(bd.Id);
            if (exist != null)
            {
                exist.Domain = bd.Domain;
                exist.InboxServer = bd.InboxServer;
                exist.InboxServerPort = bd.InboxServerPort;
                exist.InboxSSL = bd.InboxSSL;
                exist.InboxProtocol = bd.InboxProtocol;
                exist.SMTPServer = bd.SMTPServer;
                exist.SMTPServerPort = bd.SMTPServerPort;
                exist.SMTPServerSSL = bd.SMTPServerSSL;
                exist.Status = bd.Status;
                exist.MailType = bd.MailType;
                exist.AuthenticationType = bd.AuthenticationType;
                exist.TenantID = bd.TenantID;
                exist.ClientID = bd.ClientID;
                exist.ClientSecret = bd.ClientSecret;

                if (await mailServerRepo.Update(exist) == 0) throw new Exception("L'aggiornamento non è andato a buon fine.");
            }
            else throw new Exception("Il MailServer indicato non è stato trovato");
        }

        public async Task Delete(int Id)
        {
            await mailServerRepo.Delete(Id);
        }

    }
}
