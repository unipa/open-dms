using Microsoft.AspNetCore.Mvc.Rendering;
using OpenDMS.Workflow.API.DTOs.Maximo;

namespace OpenDMS.Workflow.API.BusinessLogic.MaximoBL.Interfacce
{
    public interface IMaximoSRBL
    {
        Task<string> PostSrToMaximo(SR sr);
    }
}