using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FirmeRemoteLib.Models
{
    [System.Xml.Serialization.XmlRoot("response")]
    public class InfocertResponse
    {
        [System.Xml.Serialization.XmlElementAttribute("status")]
        public string status { get; set; }
        [System.Xml.Serialization.XmlElementAttribute("error")]
        public responseError error { get; set; }

        public override string ToString()
        {
            if (error != null)
                return String.Format("INFOCERT: {4} -> {0} - {1} (Proxy: {2} - {3})", error.errorcode, error.errordescription, error.proxysignerrorcode, error.proxysignerrordescription, status);
            else
                return String.Format("INFOCERT: {0}", status);
        }
    }
    public class responseError
    {
        [System.Xml.Serialization.XmlElementAttribute("error-code")]
        public string errorcode { get; set; }
        [System.Xml.Serialization.XmlElementAttribute("error-description")]
        public string errordescription { get; set; }
        [System.Xml.Serialization.XmlElementAttribute("error-code-signature")]
        public object errorcodesignature { get; set; }
        [System.Xml.Serialization.XmlElementAttribute("proxysign-error-code")]
        public string proxysignerrorcode { get; set; }
        [System.Xml.Serialization.XmlElementAttribute("proxysign-error-description")]
        public string proxysignerrordescription { get; set; }
    }

}
