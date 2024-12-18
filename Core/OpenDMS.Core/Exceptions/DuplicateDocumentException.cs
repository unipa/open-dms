using OpenDMS.Domain.Entities;

namespace OpenDMS.Core.Exceptions
{
    public class DuplicateDocumentException : Exception
    {
        public DuplicateDocumentException(int documentId, string externalId) : base ($"Il documento #{externalId} è già presente in archivio")
        {
            DocumentId = documentId;
            ExternalId = externalId;
        }

        public int DocumentId { get; }
        public string ExternalId { get; }
    }
}
