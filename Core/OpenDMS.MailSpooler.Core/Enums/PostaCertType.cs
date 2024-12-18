using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDMS.MailSpooler.Core.Enums
{
    public enum PostaCertType
    {
        [Description("accettazione")]
        ACCETTAZIONE = 1,
        [Description("non-accettazione")]
        NON_ACCETTAZIONE = 2,
        [Description("presa-in-carico")]
        PRESA_IN_CARICO = 3,
        [Description("avvenuta-consegna")]
        AVVENUTA_CONSEGNA = 4,
        [Description("posta-certificata")]
        POSTA_CERTIFICATA = 5,
        [Description("errore-consegna")]
        ERRORE_CONSEGNA = 6,
        [Description("preavviso-errore-consegna")]
        PREAVVISO_ERRORE_CONSEGNA = 7,
        [Description("rilevazione-virus")]
        RILEVAZIONE_VIRUS = 8,
        [Description("unknow")]
        UNKNOW = 9
    }

}
