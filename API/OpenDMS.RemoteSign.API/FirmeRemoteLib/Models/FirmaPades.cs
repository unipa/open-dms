using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirmeRemoteLib.Models
{
    public class FirmaPades
    {
        public int Pagina { get; set; }
        public int StartX { get; set; }
        public int StartY { get; set; }
        public int EndX { get; set; }
        public int EndY { get; set; }
        public String? MotivoFirma { get; set; }
        public String? LabelMotivoFirma { get; set; }
        public String? LabelFirma { get; set; }
        public String? LabelData { get; set; }
        public String? SignField { get; set; }
    }
}
