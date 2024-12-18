using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDMS.Domain.Enumerators
{
    public enum ActionRequestType
    {
        None = 0,
        Generic = 1,

        View_Document = 2,
        Approve_Document = 3,
        Check_Document = 4,
        Sign_Document = 5,
        FEA_Document = 6,
        DigitalSign_Document = 7,

        Folder_Document = 8,
        Protocol_Document = 9,

        Warning = 254,
        Error = 255,
    }
}
