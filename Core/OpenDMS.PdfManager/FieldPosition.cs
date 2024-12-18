using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDMS.PdfManager
{
    public struct FieldPosition
    {
        public Int32 Page;
        public Decimal Left;
        public Decimal Top;
        public Decimal Right;
        public Decimal Bottom;
        public string Name;
        public string Description;
    }
}
