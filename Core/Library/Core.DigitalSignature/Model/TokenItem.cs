using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DigitalSignature.Model
{
    public class TokenItem
    {
        public string Serial { get; set; }
        public string Label { get; set; }
        public int Slot{ get; set; }
        public string ManufacturerId { get; set; }
        public string Model { get; set; }
    }
}
