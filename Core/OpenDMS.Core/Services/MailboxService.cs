using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Entities.Mails;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Repositories;
using OpenDMS.Infrastructure.Repositories;
/// <summary>
/// Descrizione di riepilogo per BancheDatiProvider
/// </summary>
/// 
namespace OpenDMS.Core.BusinessLogic
{
    public class MailboxService : IMailboxService
    {
        private readonly IMailboxRepository _repository;

        public MailboxService(IMailboxRepository repository)
        {
            this._repository = repository;
        }

        public async Task<IList<Mailbox>> GetAll(UserProfile u)
        {
            var maillist = (await _repository.GetAll(u.userId))
                .Where(mbox =>

                        mbox.UserId == u.userId
                        ||
                        (mbox.ReadOnlyProfiles != null &&
                            (mbox.ReadOnlyProfiles
                            .Split(',')
                            .Any(m =>
                                ("0" + m.ToLower() == u.userId)
                                ||
                                u.Roles.Any(r => "2" + r.Id.ToLower() == m.ToLower())
                                ||
                                u.Groups.Any(r => "1" + r.Id.ToLower() == m.ToLower())
                                )
                            )
                        )
                        ||
                        (mbox.DraftEnabledProfiles != null &&
                            (mbox.DraftEnabledProfiles
                            .Split(',')
                            .Any(m =>
                                ("0" + m.ToLower() == u.userId)
                                ||
                                u.Roles.Any(r => "2" + r.Id.ToLower() == m.ToLower())
                                ||
                                u.Groups.Any(r => "1" + r.Id.ToLower() == m.ToLower())
                                )
                            )
                        )
                        ||
                        (mbox.SendEnabledProfiles != null &&
                            (mbox.SendEnabledProfiles
                            .Split(',')
                            .Any(m =>
                                ("0" + m.ToLower() == u.userId)
                                ||
                                u.Roles.Any(r => "2" + r.Id.ToLower() == m.ToLower())
                                ||
                                u.Groups.Any(r => "1" + r.Id.ToLower() == m.ToLower())
                                )
                            )
                        )
                    );
            return maillist.ToList();
        }

        public async Task<Mailbox> GetById(int MailboxId)
        {
            return await _repository.GetById(MailboxId);
        }
        
        public async Task<Mailbox> GetByAddress(string Address)
        {
            return await _repository.GetByAddress(Address);
        }

        public async Task<Mailbox> TakeNext(string WorkerId, DateTime NextUpdate)
        {
            return await _repository.TakeNext(WorkerId, NextUpdate);

        }
        public async Task Release(Mailbox mailbox)
        {
            mailbox.ReaderWorkerId = "";
            mailbox.NextReaderDate = DateTime.UtcNow.AddSeconds(5);
            await _repository.Update (mailbox);
        }


        public async Task<int> Create(Mailbox mailbox)
        {
            if (mailbox.MailServerId == null) throw new ArgumentNullException(nameof(mailbox.MailServerId));

            mailbox.Validated = false;

            return await _repository.Insert(mailbox);
        }

        public async Task<int> Update(Mailbox mailbox)
        {
            var c = await GetById(mailbox.Id);
            if (!((c.Account ?? "").Equals((mailbox.Account ?? ""))) || !((c.Password ?? "").Equals((mailbox.Password ?? ""))))
            {
                mailbox.Validated = false;
                mailbox.LastCredentialUpdate = DateTime.UtcNow;
            }
            return await _repository.Update(mailbox);
        }

        public async Task<int> Delete(int mailboxId)
        {
            var m = await _repository.GetById(mailboxId);
            //if ((await _repository.GetAll(m.UserId)).Count < 1) throw new InvalidOperationException();
            return await _repository.Delete(m);
        }

    }
}