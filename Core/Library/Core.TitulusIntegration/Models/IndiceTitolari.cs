namespace Core.TitulusIntegration.Models
{
    public class IndiceTitolari
    {
        public List<indice_titolario> Titolari { get; set; }
    }


    // NOTA: con il codice generato potrebbe essere richiesto almeno .NET Framework 4.5 o .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class indice_titolario
    {

        private indice_titolarioCompilazione_automatica compilazione_automaticaField;

        private indice_titolarioValidita validitaField;

        private long physdocField;

        private string nrecordField;

        private string voceField;

        /// <remarks/>
        public indice_titolarioCompilazione_automatica compilazione_automatica
        {
            get
            {
                return this.compilazione_automaticaField;
            }
            set
            {
                this.compilazione_automaticaField = value;
            }
        }

        /// <remarks/>
        public indice_titolarioValidita validita
        {
            get
            {
                return this.validitaField;
            }
            set
            {
                this.validitaField = value;
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
        public string voce
        {
            get
            {
                return this.voceField;
            }
            set
            {
                this.voceField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class indice_titolarioCompilazione_automatica
    {

        private indice_titolarioCompilazione_automaticaClassif classifField;

        /// <remarks/>
        public indice_titolarioCompilazione_automaticaClassif classif
        {
            get
            {
                return this.classifField;
            }
            set
            {
                this.classifField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class indice_titolarioCompilazione_automaticaClassif
    {

        private string codField;

        private string spaceField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string cod
        {
            get
            {
                return this.codField;
            }
            set
            {
                this.codField = value;
            }
        }

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

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class indice_titolarioValidita
    {

        private string tipodocField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string tipodoc
        {
            get
            {
                return this.tipodocField;
            }
            set
            {
                this.tipodocField = value;
            }
        }
    }


    // NOTA: con il codice generato potrebbe essere richiesto almeno .NET Framework 4.5 o .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class voce_indice
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
