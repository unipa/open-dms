using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.TitulusIntegration.Models
{
    public class StruttureEsterne
    {
        public List<struttura_esterna> strutture { get; set; }
    }

    // NOTA: con il codice generato potrebbe essere richiesto almeno .NET Framework 4.5 o .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class struttura_esterna
    {

        private string nomeField;

        private struttura_esternaIndirizzo indirizzoField;

        private struttura_esternaEmail emailField;

        private struttura_esternaTelefono telefonoField;

        private ushort physdocField;

        private string nrecordField;

        private string cod_uffField;

        private uint partita_ivaField;

        /// <remarks/>
        public string nome
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
        public struttura_esternaIndirizzo indirizzo
        {
            get
            {
                return this.indirizzoField;
            }
            set
            {
                this.indirizzoField = value;
            }
        }

        /// <remarks/>
        public struttura_esternaEmail email
        {
            get
            {
                return this.emailField;
            }
            set
            {
                this.emailField = value;
            }
        }

        /// <remarks/>
        public struttura_esternaTelefono telefono
        {
            get
            {
                return this.telefonoField;
            }
            set
            {
                this.telefonoField = value;
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
        public uint partita_iva
        {
            get
            {
                return this.partita_ivaField;
            }
            set
            {
                this.partita_ivaField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class struttura_esternaIndirizzo
    {

        private string spaceField;

        private string comuneField;

        private uint capField;

        private string provField;

        private string nazioneField;

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
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string comune
        {
            get
            {
                return this.comuneField;
            }
            set
            {
                this.comuneField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint cap
        {
            get
            {
                return this.capField;
            }
            set
            {
                this.capField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string prov
        {
            get
            {
                return this.provField;
            }
            set
            {
                this.provField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nazione
        {
            get
            {
                return this.nazioneField;
            }
            set
            {
                this.nazioneField = value;
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

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class struttura_esternaEmail
    {

        private string addrField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string addr
        {
            get
            {
                return this.addrField;
            }
            set
            {
                this.addrField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class struttura_esternaTelefono
    {

        private string tipoField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string tipo
        {
            get
            {
                return this.tipoField;
            }
            set
            {
                this.tipoField = value;
            }
        }
    }

}
