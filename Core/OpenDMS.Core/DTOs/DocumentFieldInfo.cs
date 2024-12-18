using OpenDMS.Domain.Entities.Schemas;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDMS.Core.DTOs
{
    public class DocumentFieldInfo
    {
        public int FieldIndex { get; set; } = 0;
        public string FieldIdentifier { get; set; }
        public string FieldTypeId { get; set; } = "";
        public string Title { get; set; } = "";
        public string DataType { get; set; } = "";
        public string ControlType { get; set; } = "textbox";
        public string CustomProperties { get; set; } = "";

        public int Tags { get { return Values.Length; } }

        public string Value { get { return _Value; } set { _Value = value; _Values = value.Split(',', StringSplitOptions.TrimEntries); } }
        public string FormattedValue { get { return _FormattedValue; } set { _FormattedValue = value; _FormattedValues = value.Split(',', StringSplitOptions.TrimEntries); } }
        public string LookupValue { get { return _Lookup; } set { _Lookup = value; _LookupValues = value.Split(',', StringSplitOptions.TrimEntries); } }

        public bool Encrypted { get; set; }
        public bool Mandatory { get; set; }
        public bool Customized { get; set; }
        public bool Tag { get; set; }
        public DateTime LastUpdate { get; set; }
        public string LastUpdateUser { get; set; } = "";

        public string[] Values { get { return _Values; } }
        public string[] FormattedValues { get { return _FormattedValues; } }
        public string[] LookupValues { get { return _LookupValues; } }

        public FieldType FieldType { get; set; }

        private string _Value = "";
        private string _FormattedValue = "";
        private string _Lookup = "";
        private string[] _Values = new string[1] { "" };
        private string[] _FormattedValues = new string[1] { "" };
        private string[] _LookupValues = new string[1] { "" };

        //public int? BlobId { get; set; }
    }
}
