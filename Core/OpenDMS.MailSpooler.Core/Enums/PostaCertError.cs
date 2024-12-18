using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDMS.MailSpooler.Core.Enums
{
    public enum PostaCertError
    {
        [Description("nessuno")]
        NESSUNO = 1,
        [Description("no-dest")]
        NO_DEST = 2,
        [Description("no-dominio")]
        NO_DOMINIO = 3,
        [Description("virus")]
        VIRUS = 4,
        [Description("altro")]
        ALTRO = 5
    }
}
