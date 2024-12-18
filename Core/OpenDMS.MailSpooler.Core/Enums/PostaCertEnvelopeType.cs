using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDMS.MailSpooler.Core.Enums
{
    public enum PostaCertEnvelopeType
    {
        [Description("")]
        NONPRESENTE = 0,
        [Description("completa")]
        COMPLETA = 1,
        [Description("breve")]
        BREVE = 2,
        [Description("sintetica")]
        SINTETICA = 3
    }
}
