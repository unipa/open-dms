namespace OpenDMS.Domain.Enumerators;

public enum RecipientType
{
    // mittente
    Sender = 1,
    // destinatario
    To = 2,
    // destinatario in cc
    CC = 3,
    // destinatario nascosto
    CCr = 4,
    // soggetto o contatto associato
    Customized = 255
}
