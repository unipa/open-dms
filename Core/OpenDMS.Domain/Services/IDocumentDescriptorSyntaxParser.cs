using OpenDMS.Domain.Entities;
using OpenDMS.Domain.Entities.Documents;

namespace OpenDMS.Domain.Services;


public interface IDocumentDescriptorSyntaxParser
{
    public IList<Document> Parse(string FileDescriptor);

}
