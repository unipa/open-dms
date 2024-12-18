using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.TitulusIntegration.Models
{
    public class PersoneInterne
    {
        public List<persona_interna> persone { get; set; }
    }

    // NOTA: con il codice generato potrebbe essere richiesto almeno .NET Framework 4.5 o .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class persona_interna
    {

        private persona_internaLogin[] loginField;

        private long physdocField;

        private string nrecordField;

        private string matricolaField;

        private string nomeField;

        private string cognomeField;

        private string cod_ammField;

        private string cod_aooField;

        private string cod_uffField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("login")]
        public persona_internaLogin[] login
        {
            get
            {
                return this.loginField;
            }
            set
            {
                this.loginField = value;
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
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class persona_internaLogin
    {

        private string nameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }


}
