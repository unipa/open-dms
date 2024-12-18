using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using MimeKit;
using System.Text;

namespace Elmi.Core.PreviewGenerators
{
    public interface IFileConverter
    {
        string[] FromFile { get; }
        string ToFile { get;  }

        Task<Stream> Convert(string Extension, Stream data);


    }
}