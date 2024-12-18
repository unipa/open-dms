using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenDMS.MultiTenancy;
using OpenDMS.MultiTenancy.Exceptions;
using OpenDMS.MultiTenancy.Interfaces;
using OpenDMS.TenantManager.API.Controllers;
using OpenDMS.TenantManager.API.DTOs;
using System.Net;

namespace IdentiyMultiTenant.Pages
{
    //[Authorize("admin")]
    public class VuotoModel : PageModel
    {

        public VuotoModel()
        {
        }

        public async Task OnGetAsync(string id)
        {
        }

    }
}