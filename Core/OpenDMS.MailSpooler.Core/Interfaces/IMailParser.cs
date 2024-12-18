using OpenDMS.Domain.Entities.Settings;
using OpenDMS.MailSpooler.Core.Archiver;

namespace OpenDMS.MailSpooler.Core.Interfaces
{
    public interface IMailParser
    {
        List<LookupTable> GetCCList();
        List<LookupTable> GetCCrList();
        int[] GetDocuments();
        string GetMessageId();
        LookupTable GetSender();
        List<LookupTable> GetToList();
        MailParser Read(Stream message);
    }
}