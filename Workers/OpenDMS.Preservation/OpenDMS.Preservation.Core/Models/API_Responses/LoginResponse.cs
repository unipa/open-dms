using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDMS.Preservation.Core.Models.API_Responses
{
    public class LoginResponse
    {
        public string code { get; set; }
        public string ldSessionId { get; set; }
        public string pdv { get; set; }

    }
}
