using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenDMS.Core.BusinessLogic;
using OpenDMS.Core.DTOs;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Entities.Mails;
using OpenDMS.Domain.Entities.Settings;
using OpenDMS.Domain.Entities.Users;
using OpenDMS.Domain.Entities.V2;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Services;
using OpenDMS.UserManager.API.DTOs;
using System.Text;
using System.Text.RegularExpressions;

namespace OpenDMS.UserManager.API.Controllers
{
    [Authorize]
    [Route("api/identity/[controller]")]
    [ApiController]
    public class UserManagerController : ControllerBase
    {
        private readonly ILogger<AvatarController> logger;
        private readonly IOrganizationUnitService organization;
        private readonly IRoleService roleService;
        private readonly IUserService userService;
        private readonly IMailboxService mailboxService;


        public UserManagerController(
            ILogger<AvatarController> logger,
            IOrganizationUnitService organization,
            IRoleService roleService,
            IUserService userService,
            IMailboxService mailboxService)
        {
            this.logger = logger;
            this.organization = organization;
            this.roleService = roleService;
            this.userService = userService;
            this.mailboxService = mailboxService;
        }

        [Authorize]
        [HttpGet("Groups")]

        public async Task<List<LookupTable>> GetGroups()
        {
            return await GetGroups(User.Identity.Name);
        }

        [Authorize]
        [HttpGet("Roles")]
        public async Task<List<LookupTable>> GetRoles()
        {
            return await GetRoles(User.Identity.Name);
        }
        [Authorize]
        [HttpGet("Avatar")]
        public async Task<FileResult> GetImage()
        {
            return await GetImage(User.Identity.Name);
        }




        [HttpGet("{userId}/Groups")]
        public async Task<List<LookupTable>> GetGroups(string userId)
        {
            return await userService.GetGroups(userId);
        }

        [HttpGet("{userId}/Roles")]
        public async Task<List<LookupTable>> GetRoles(string userId)
        {
            return await userService.GetRoles(userId);
        }

        [HttpGet("{subjectId}/Avatar")]
        public async Task<FileResult> GetImage(string subjectId)
        {
            ProfileType profileType = (ProfileType)int.Parse(subjectId.Substring(0, 1));
            string profileId = subjectId.Substring(1);
            var data = await userService.GetAvatar(profileId, profileType);
            return File(data, System.Net.Mime.MediaTypeNames.Image.Jpeg, subjectId + ".png");
        }

        /// <summary>
        /// Restituisce l'oggetto User contenente del informazioni dell'utente associato all'utente loggato
        /// </summary>
        /// <returns>Oggetto User dell'utente loggato</returns>
        [Authorize]
        [HttpGet("UserInfo")]
        public async Task<User> GetUser()
        {
            return await userService.GetById(User.Identity.Name);
        }

        /// <summary>
        /// Restituisce tutti le Mailbox associati all'utente loggato
        /// </summary>
        /// <returns>Lista di oggetti Mailbox</returns>
        [Authorize]
        [HttpGet("User/Contact/DigitalAddress")]
        public async Task<ActionResult<List<Mailbox>>> GetAllContactDigitalAddressesByUserId()
        {
            var u = await userService.GetUserProfile(User.Identity.Name);
            return Ok(await mailboxService.GetAll(u));
        }

        /// <summary>
        /// Restituisce il ContactDigitalAddress identificato tramite Id e associato all'utente loggato
        /// </summary>
        /// <returns>Lista di oggetti Mailbox</returns>
        [Authorize]
        [HttpGet("User/Contact/DigitalAddress/{Id}")]
        public async Task<ActionResult<Mailbox>> GetContactDigitalAddressesByUserId(int Id)
        {
            return await mailboxService.GetById(Id);
        }

        /// <summary>
        /// Aggiunge un MailBox all'utente loggato
        /// </summary>
        /// <returns>Restituisce oggetto ContactDigitalAddress aggiunto </returns>
        [Authorize]
        [HttpPost("User/Contact/DigitalAddress")]
        public async Task<ActionResult<Mailbox>> AddContactDigitalAddress(Mailbox bd)
        {
            try
            {
                int r = await mailboxService.Create (bd);
                return r > 0 ? await mailboxService.GetById(bd.Id) : BadRequest("L'inserimento non è andato a buon fine.");
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.Message.Contains("Duplicate entry")) return Conflict(ex.InnerException.Message);
                }

                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Aggiorna un Mailbox dell'utente loggato
        /// </summary>
        /// <returns>Restituisce oggetto Mailbox aggiornato </returns>
        [Authorize]
        [HttpPut("User/Contact/DigitalAddress/{Id}")]
        public async Task<ActionResult<Mailbox>> UpdateContactDigitalAddress(Mailbox bd, int Id)
        {
            try
            {
                if (await mailboxService.GetById(Id) != null)
                {
                    var r = await mailboxService.Update (bd);
                    return r > 0 ? await mailboxService.GetById(Id) : BadRequest("L'aggiornamento non è andato a buon fine.");
                }
                else return BadRequest("Il ContactDigitalAddress indicato non è stato trovato");
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.Message.Contains("Duplicate entry")) return Conflict(ex.InnerException.Message);
                }

                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Elimina un ContactDigitalAddress dell'utente loggato
        /// </summary>
        /// <returns>Ritorna 200 in caso di successo oppure 400 in caso di fallimento.</returns>
        [Authorize]
        [HttpDelete("User/Contact/DigitalAddress/{Id}")]
        public async Task<ActionResult> DeleteContactDigitalAddress(int Id)
        {
            var result = await mailboxService.Delete (Id);
            return result > 0 ? Ok() : BadRequest("Eliminazione non è andata a buon fine.");
        }

    }
}
