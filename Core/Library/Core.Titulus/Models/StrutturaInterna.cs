using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.TitulusIntegration.Models
{
    public class StruttureInterne
    {
        public List<struttura_interna> strutture { get; set; }
    }

    // NOTA: con il codice generato potrebbe essere richiesto almeno .NET Framework 4.5 o .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class struttura_interna
    {

        private struttura_internaNome nomeField;

        private ushort physdocField;

        private string nrecordField;

        private string cod_uffField;

        private string cod_ammField;

        private string cod_aooField;

        private ushort cod_responsabileField;

        private string cod_padreField;

        /// <remarks/>
        public struttura_internaNome nome
        {
            get
            {
                return this.nomeField;
            }
            set
            {
                this.nomeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort physdoc
        {
            get
            {
                return this.physdocField;
            }
            set
            {
                this.physdocField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nrecord
        {
            get
            {
                return this.nrecordField;
            }
            set
            {
                this.nrecordField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string cod_uff
        {
            get
            {
                return this.cod_uffField;
            }
            set
            {
                this.cod_uffField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string cod_amm
        {
            get
            {
                return this.cod_ammField;
            }
            set
            {
                this.cod_ammField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string cod_aoo
        {
            get
            {
                return this.cod_aooField;
            }
            set
            {
                this.cod_aooField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort cod_responsabile
        {
            get
            {
                return this.cod_responsabileField;
            }
            set
            {
                this.cod_responsabileField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string cod_padre
        {
            get
            {
                return this.cod_padreField;
            }
            set
            {
                this.cod_padreField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class struttura_internaNome
    {

        private string spaceField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://www.w3.org/XML/1998/namespace")]
        public string space
        {
            get
            {
                return this.spaceField;
            }
            set
            {
                this.spaceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }



}
