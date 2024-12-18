namespace OpenDMS.Domain.Entities
{
    public enum MailStatus
    {
        // Stati per l'outbound
        Draft = 0,
        WaitingForApproval = 1,
        // Da Elaborare (in ricezione o invio)
        Sending = 2,
        Sent = 3,
        Failed = 4,

        // inbox 
        Received = 5,
        Claimed = 6,
        Spam = 7,
        Archived = 8,

        Deleted = 9, // <- da cancellare
        Purged = 10, // <-cancellato
    }
}