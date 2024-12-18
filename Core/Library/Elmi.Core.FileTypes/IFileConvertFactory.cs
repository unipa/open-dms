using Elmi.Core.PreviewGenerators;

namespace Elmi.Core.FileConverters
{
    public interface IFileConvertFactory
    {
        Task<IFileConverter> Get (string FromExtension, string ToExtension);
        string GetFileSignature(Stream dataStream);
    }
}
