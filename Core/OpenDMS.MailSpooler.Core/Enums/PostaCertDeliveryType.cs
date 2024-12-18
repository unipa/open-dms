using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDMS.MailSpooler.Core.Enums
{


    public enum PostaCertDeliveryType
    {
        [Description("certificato")]
        CERTIFICATO = 1,
        [Description("esterno")]
        ESTERNO = 2
    }
}
