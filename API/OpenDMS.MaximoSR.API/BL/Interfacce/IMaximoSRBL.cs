using Microsoft.AspNetCore.Mvc.Rendering;
using OpenDMS.MaximoSR.API.Models;

namespace OpenDMS.MaximoSR.API.BL.Interfacce
{
    public interface IMaximoSRBL
    {
        Task<string> GetToken();
        Task<List<ASSET>> GetAssetListFromMaximo();
        void StartBpmnProcess(SR sr);
    }
}