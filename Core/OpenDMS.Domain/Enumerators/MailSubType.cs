using System.ComponentModel;

namespace OpenDMS.Domain.Entities
{
    public enum MailSubType
    {
        Message= 0,
        AcceptanceReceipt = 1,
        NoAcceptanceReceipt = 2,
        DeliveryReceipt = 3,
        Take = 4,
        Pec = 5,
        Error = 6,
        PreDeliveryReceipt = 7,
        Virus = 8,
        Unknown = 9

    }
}