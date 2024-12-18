using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.TitulusIntegration.Models
{
    public class PersoneEsterne
    {
        public List<persona_esterna> persone { get; set; } = new List<persona_esterna>();
    }


    // NOTA: con il codice generato potrebbe essere richiesto almeno .NET Framework 4.5 o .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class persona_esterna
    {

        private persona_esternaRecapito recapitoField;

        private persona_esternaRecapito_personale recapito_personaleField;

        private long physdocField;

        private string nrecordField;

        private string matricolaField;

        private string cognomeField;

        private string nomeField;

        /// <remarks/>
        public persona_esternaRecapito recapito
        {
            get
            {
                return this.recapitoField;
            }
            set
            {
                this.recapitoField = value;
            }
        }

        /// <remarks/>
        public persona_esternaRecapito_personale recapito_personale
        {
            get
            {
                return this.recapito_personaleField;
            }
            set
            {
                this.recapito_personaleField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public long physdoc
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
        public string matricola
        {
            get
            {
                return this.matricolaField;
            }
            set
            {
                this.matricolaField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string cognome
        {
            get
            {
                return this.cognomeField;
            }
            set
            {
                this.cognomeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
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
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class persona_esternaRecapito
    {

        private persona_esternaRecapitoEmail_certificata email_certificataField;

        private persona_esternaRecapitoEmail emailField;

        private persona_esternaRecapitoTelefono telefonoField;

        /// <remarks/>
        public persona_esternaRecapitoEmail_certificata email_certificata
        {
            get
            {
                return this.email_certificataField;
            }
            set
            {
                this.email_certificataField = value;
            }
        }

        /// <remarks/>
        public persona_esternaRecapitoEmail email
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
        public persona_esternaRecapitoTelefono telefono
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
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class persona_esternaRecapitoEmail_certificata
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
    public partial class persona_esternaRecapitoEmail
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
    public partial class persona_esternaRecapitoTelefono
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

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class persona_esternaRecapito_personale
    {

        private persona_esternaRecapito_personaleTelefono telefonoField;

        /// <remarks/>
        public persona_esternaRecapito_personaleTelefono telefono
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
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class persona_esternaRecapito_personaleTelefono
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
