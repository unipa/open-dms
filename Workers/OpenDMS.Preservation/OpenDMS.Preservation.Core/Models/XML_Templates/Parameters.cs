using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDMS.Preservation.Core.Models
{
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class parameters
    {
        private string policy_idField;
        private parametersIndex_file index_fileField;
        private parametersData_file data_fileField;
        private string pathField;
        private string encrypted_by_ownerField;

        /// <remarks/>
        public string policy_id
        {
            get
            {
                return this.policy_idField;
            }
            set
            {
                this.policy_idField = value;
            }
        }

        /// <remarks/>
        public parametersIndex_file index_file
        {
            get
            {
                return this.index_fileField;
            }
            set
            {
                this.index_fileField = value;
            }
        }

        /// <remarks/>
        public parametersData_file data_file
        {
            get
            {
                return this.data_fileField;
            }
            set
            {
                this.data_fileField = value;
            }
        }

        /// <remarks/>
        public string path
        {
            get
            {
                return this.pathField;
            }
            set
            {
                this.pathField = value;
            }
        }

        /// <remarks/>
        public string encrypted_by_owner
        {
            get
            {
                return this.encrypted_by_ownerField;
            }
            set
            {
                this.encrypted_by_ownerField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class parametersIndex_file
    {
        private string index_nameField;
        private string index_hashField;
        private string index_mimetypeField;

        /// <remarks/>
        public string index_name
        {
            get
            {
                return this.index_nameField;
            }
            set
            {
                this.index_nameField = value;
            }
        }

        /// <remarks/>
        public string index_hash
        {
            get
            {
                return this.index_hashField;
            }
            set
            {
                this.index_hashField = value;
            }
        }

        /// <remarks/>
        public string index_mimetype
        {
            get
            {
                return this.index_mimetypeField;
            }
            set
            {
                this.index_mimetypeField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class parametersData_file
    {
        private string data_nameField;
        private string data_hashField;
        private string data_mimetypeField;

        /// <remarks/>
        public string data_name
        {
            get
            {
                return this.data_nameField;
            }
            set
            {
                this.data_nameField = value;
            }
        }

        /// <remarks/>
        public string data_hash
        {
            get
            {
                return this.data_hashField;
            }
            set
            {
                this.data_hashField = value;
            }
        }

        /// <remarks/>
        public string data_mimetype
        {
            get
            {
                return this.data_mimetypeField;
            }
            set
            {
                this.data_mimetypeField = value;
            }
        }
    }
}
