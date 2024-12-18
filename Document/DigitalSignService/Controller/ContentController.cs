using DigitalSignService.Interfaces;
using DigitalSignService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenDMS.Core.DTOs;
using System.ComponentModel;

namespace DigitalSignService.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContentController : ControllerBase
    {
        public IDMSWrapper Wrapper { get; }
        public ICheckOutService CheckOutService { get; }


        public ContentController(
            IDMSWrapper wrapper,
            ICheckOutService checkOutService
            )
        {
            Wrapper = wrapper;
            CheckOutService = checkOutService;
        }

        [HttpGet("Open")]
        public async Task<ActionResult> Get(String Host, int documentId)
        {
            try { 
                var processGuid = await CheckOutService.AddFile(Host, documentId);
                return Ok(processGuid);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("Check")]
        public async Task<ActionResult> Check(string processGuid)
        {
            try
            {
                if (await CheckOutService.Alive(processGuid))
                    return Ok(await CheckOutService.Changed(processGuid));
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

    }
}
