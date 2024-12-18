using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenDMS.Core.Filters;
using OpenDMS.Domain.Models;
using System.Diagnostics;
using Web.BL.Interface;
using Web.Model.Admin;

namespace Web.Controllers.Admin
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorization(OpenDMS.Domain.Constants.PermissionType.CanAdminGroups)]
    public class OrganigrammaController : Controller
    {
        private readonly ILogger<OrganigrammaController> _logger;
        private readonly IOrganigrammaBL _bl;
        private readonly IConfiguration config;

        public OrganigrammaController(ILogger<OrganigrammaController> logger, IOrganigrammaBL bl, IConfiguration config = null)
        {
            _logger = logger;
            _bl = bl;
            this.config = config;
        }

        [HttpGet("Admin/[controller]/Index")]
        public async Task<IActionResult> Index(int StartISODate = 0)
        {
            try
            {
                //INIZIALIZZO I DATI

                var vm = new OrganigrammaViewModel();
                vm.StartISODate = StartISODate;
                //vm.host = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}" + config["PATH_BASE"] + "/Organigramma/";
                //vm.ApiHost = config["Endpoint:AdminService"];
                //vm.token = await HttpContext.GetTokenAsync("access_token");


                var ErrorMessage = "";
                var Icon = "";

                //ottengo l'albero dell'organizzazione
                try { vm.Tree = (await _bl.GetOrganizationTree(StartISODate)).ToList(); }
                catch (Exception ex)
                {
                    ErrorMessage += "Non è stato trovato l'albero dell'organizzazione. Errore: " + ex.Message + " ; ";
                    vm.Tree = new List<OrganizationNodeTree>();
                }

                vm.ErrorMessage = ErrorMessage;
                vm.Icon = Icon;

                return View(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError("OrganigrammaController -> Index -> THROW an Exception : " + ex.Message);
                throw ex;
            }

        }

        [HttpPost("Admin/[controller]/MoveNode")]
        public async Task<ActionResult> MoveNode([FromBody] MoveOrganizationNode_DTO bd)
        {

            //sposto il Nodo
            try
            {
                await _bl.MoveOrganizationNode(bd);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok("");
        }

        [HttpPost("Admin/[controller]/CreateNode")]
        public async Task<ActionResult> CreateNode([FromBody] CreateOrUpdateOrganizationNode_DTO bd)
        {
            try
            {
                return Ok((await _bl.CreateOrganizationNode(bd)).UserGroupId);
            }
            catch (Exception ex)
            {
                return BadRequest("Errore: " + ex.Message);
            }
        }

        [HttpPut("Admin/[controller]/UpdateNode/{userGroupId}")]
        public async Task<ActionResult> UpdateNode(string userGroupId, [FromBody] CreateOrUpdateOrganizationNode_DTO bd)
        {
            try
            {
                await _bl.UpdateOrganizationNode(userGroupId, bd);
            }
            catch (Exception ex)
            {
                return BadRequest("Errore: " + ex.Message);
            }
            return Ok("");
        }

        [HttpDelete("Admin/[controller]/RemoveNode/{userGroupId}/{StartISODate}/{EndDate}")]
        public async Task<ActionResult> RemoveNode(string userGroupId, DateTime EndDate, int StartISODate)
        {
            try
            {
                await _bl.RemoveOrganizationNode(userGroupId, EndDate, StartISODate);
            }
            catch (Exception ex)
            {
                return BadRequest("Errore: " + ex.Message);
            }
            return Ok("");
        }

        [HttpDelete("Admin/[controller]/RemoveUser")]
        public async Task<IActionResult> RemoveUser([FromBody] UserInGroup_DTO bd)
        {
            try
            {
                if (string.IsNullOrEmpty(bd.UserId)) throw new Exception("Non è stato selezionato nessun utente da aggiungere al gruppo.");
                if (string.IsNullOrEmpty(bd.RoleId)) throw new Exception("Non è stato selezionato nessun ruolo da assegnare all'utente.");
                await _bl.RemoveUser(bd);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok("");
        }

        [HttpPut("Admin/[controller]/ChangeRoleUser")]
        public async Task<IActionResult> ChangeRoleUser([FromBody] UserInGroup_DTO bd)
        {
            try
            {
                if (string.IsNullOrEmpty(bd.UserId)) throw new Exception("Non è stato selezionato nessun utente da aggiungere al gruppo.");
                if (string.IsNullOrEmpty(bd.RoleId)) throw new Exception("Non è stato selezionato nessun ruolo da assegnare all'utente.");
                await _bl.ChangeRoleUser(bd);
            }
            catch (Exception ex)
            {
                return BadRequest("Errore: " + ex.Message);
            }
            return Ok("");
        }

        [HttpPut("Admin/[controller]/AddUser")]
        public async Task<IActionResult> AddUser([FromBody] UserInGroup_DTO bd)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(bd);  
                if (string.IsNullOrEmpty(bd.UserId)) throw new Exception("Non è stato selezionato nessun utente da aggiungere al gruppo.");
                if (string.IsNullOrEmpty(bd.RoleId)) throw new Exception("Non è stato selezionato nessun ruolo da assegnare all'utente.");
                await _bl.AddUser(bd);
            }
            catch (Exception ex)
            {
                return BadRequest("Errore: " + ex.Message);
            }
            return Ok("");
        }


        [HttpGet("Admin/[controller]/GetUsersByRoleId/{roleId}")]

        public async Task<IActionResult> GetAllByRole(string roleId, string? filter)
        {
            return Ok(await _bl.GetAllByRole(roleId, filter));
        }



        [HttpGet]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}