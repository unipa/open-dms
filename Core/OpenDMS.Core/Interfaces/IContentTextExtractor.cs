using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDMS.Core.Interfaces
{
    public interface IContentTextExtractor
    {
        Task<string> GetText(Stream stream);

        bool FileTypeSupported(string Extension);

    }
}
