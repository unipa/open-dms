using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDMS.Preservation.Core.Models
{
    public class DA_Settings
    {
        public TypeConf[] TypeCons { get; set; }
    }

    public class TypeConf
    {
        public string DocType { get; set; }
        public string TypeName { get; set; }
        public string UserResp { get; set; }
        public string MimeType { get; set; }
        public string Gap { get; set; }
        public Dictionary<string, string> Metadati { get; set; }
    }
}
