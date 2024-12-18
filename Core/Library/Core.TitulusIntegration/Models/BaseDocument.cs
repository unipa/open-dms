using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TitulusIntegration.Titulus;

namespace Core.TitulusIntegration.Models
{
    public class BaseDocument
    {
        public string Oggetto { get; set; } = "";
        public string Tipologia { get; set; } = "";
        public string MezzoTrasmissione { get; set; } = "";
        public string MezzoTrasmissioneCosto { get; set; } = "";
        public string MezzoTrasmissioneValuta { get; set; } = "";
        public string Repertorio { get; set; } = "";
        public string NumeroRepertorio { get; set; } = "";
        public string Note { get; set; } = "";
        public string VoceIndice { get; set; } = "";

        public List<AttachmentBean> Attachments { get; set; } = new List<AttachmentBean>();


    }
}
