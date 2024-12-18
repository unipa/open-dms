namespace OpenDMS.Domain.Enumerators;

public enum ExpirationStrategy
{
    None = 0,
    DataEntry = 1,
    CreationDate = 2,
    DocumentDate = 3,
    ProtocolDate = 4,
    Content = 5,
}
