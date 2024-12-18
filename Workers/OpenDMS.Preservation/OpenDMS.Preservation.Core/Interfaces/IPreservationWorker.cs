using OpenDMS.Preservation.Core.Models;
using OpenDMS.Preservation.Core.Models.API_Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDMS.Preservation.Core.Interfaces
{
    public interface IPreservationWorker
    {
        public  Task<LoginResponse> Login(string utentiResponsabili);
        public void Test();
        public  Task<bool> Preservation(string SessionID, string pdvCode, List<int> DocTypes, TypeConf conf, bool fromWS = false);
        public  Task<bool> Logout(string SessionID, string utentiResponsabili);
    }
}
