using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenDMS.Core.DTOs;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Entities.Settings;
using OpenDMS.Domain.Entities.Users;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Services;
using Web.DTOs;

namespace Web.Controllers.Identity
{
    [Authorize]
    [ApiController]
    [Route("internalapi/[controller]")]
    public class UserManagerController : ControllerBase
    {
        private readonly ILogger<UserManagerController> logger;
        private readonly IOrganizationUnitService organization;
        private readonly IRoleService roleService;
        private readonly IUserService userService;
        private readonly IMailboxService mailboxService;
        private readonly IWebHostEnvironment _appEnvironment;


        public UserManagerController(
            ILogger<UserManagerController> logger,
            IOrganizationUnitService organization,
            IRoleService roleService,
            IUserService userService,
            IMailboxService mailboxService,
            IWebHostEnvironment appEnvironment)
        {
            this.logger = logger;
            this.organization = organization;
            this.roleService = roleService;
            this.userService = userService;
            this.mailboxService = mailboxService;
            this._appEnvironment = appEnvironment;
        }
        
        [HttpGet("GetAll/ByGroup/{userGroupId}")]

        public async Task<List<User>> GetAllByGroup(string userGroupId, string? filter)
        {
            return await userService.GetAllByRole(userGroupId, filter);
        }

        [HttpGet("GetAll/ByRole/{roleId}")]

        public async Task<List<User>> GetAllByRole(string roleId, string? filter)
        {
            return await userService.GetAllByRole(roleId, filter);
        }



        [Authorize]
        [HttpGet("GetAll/Deleted")]

        public async Task<List<User>> GetAllDeleted(string filter)
        {
            return await userService.GetAllDeleted(filter);
        }



        [Authorize]
        [HttpGet()]

        public async Task<List<User>> GetAll(bool IncludesDeleted)
        {
            return await userService.GetAll(IncludesDeleted);
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
            var u = User.Identity.Name;
            return await GetRoles(u);
        }
        [Authorize]
        [HttpGet("Avatar")]
        public async Task<FileResult> GetImage()
        {
            return await GetImage(User.Identity.Name);
        }

        [HttpGet("Images/{name}")]
        public async Task<IActionResult> GetCustomImage(string name)
        {
            byte[] imageBytes = name.StartsWith("-") ? System.IO.File.ReadAllBytes(Path.Combine(_appEnvironment.WebRootPath, "images", "previews", name.Substring(1)+".png")) : await userService.GetUserStamp(User.Identity.Name, ProfileType.User, name);
            return (imageBytes != null) ? File(imageBytes, "image/png") : NotFound();
        }


        [HttpGet("HandWrittenSign")]
        public async Task<IActionResult> GetHandWrittenSign()
        {
            byte[] imageBytes = await userService.GetUserStamp(User.Identity.Name, ProfileType.User, "Sign");
            return (imageBytes != null) ? File(imageBytes, "image/png") : NotFound();
        }


        [HttpGet("Visto")]
        public async Task<IActionResult> GetVisto()
        {
            byte[] imageBytes = await userService.GetUserStamp(User.Identity.Name, ProfileType.User, "Visto");
            return (imageBytes != null) ? File(imageBytes, "image/png") : NotFound();
        }

        [HttpGet("CampoFirmaPreview")]
        public async Task<IActionResult> GetCampoFirmaPreview()
        {
            var imagePath = Path.Combine(_appEnvironment.WebRootPath, "images", "previews", "PDFSign.png");

            if (!System.IO.File.Exists(imagePath))
                throw new FileNotFoundException("File non trovato", imagePath);

            byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);
            return (imageBytes != null) ? File(imageBytes, "image/png") : NotFound();
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
        /// Restituisce tutti i ContactDigitalAddresses associati all'utente loggato
        /// </summary>
        /// <returns>Lista di oggetti ContactDigitalAddress</returns>
        [Authorize]
        [HttpGet("User/Contact/DigitalAddress")]
        public async Task<ActionResult<List<ContactDigitalAddress_DTO>>> GetAllContactDigitalAddressesByUserId()
        {
            var u = await userService.GetUserProfile(User.Identity.Name);
            var list = await userService.GetAllContactDigitalAddress(User.Identity.Name);
            List<ContactDigitalAddress_DTO> castedList = new List<ContactDigitalAddress_DTO>();

            var config = new MapperConfiguration(cfg => cfg.CreateMap<ContactDigitalAddress, ContactDigitalAddress_DTO>());
            Mapper mp = new Mapper(config);

            foreach (var contact in list)
            {
                castedList.Add(mp.Map<ContactDigitalAddress_DTO>(contact));
            }


            var mb_list = await mailboxService.GetAll(u);

            foreach (var item in castedList)
            {

                var mb = mb_list.FirstOrDefault(mb => mb.MailAddress.Equals(item.Address));
                if (mb == null) continue;
                item.Validated = mb.Validated;
                item.EnableDownload = mb.EnableDownload;
            }

            return castedList;
        }

        /// <summary>
        /// Restituisce il ContactDigitalAddress identificato tramite Id e associato all'utente loggato
        /// </summary>
        /// <returns>Lista di oggetti ContactDigitalAddress</returns>
        [Authorize]
        [HttpGet("User/Contact/DigitalAddress/{Id}")]
        public async Task<ActionResult<ContactDigitalAddress>> GetContactDigitalAddressesByUserId(int Id)
        {
            return await userService.GetDigitalAddressById(Id);
        }

        /// <summary>
        /// Aggiunge un ContactDigitalAddress all'utente loggato
        /// </summary>
        /// <returns>Restituisce oggetto ContactDigitalAddress aggiunto </returns>
        [Authorize]
        [HttpPost("User/Contact/DigitalAddress")]
        public async Task<ActionResult<ContactDigitalAddress>> AddContactDigitalAddress(AddOrUpdateContactDigitalAddress bd)
        {
            try
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<AddOrUpdateContactDigitalAddress, ContactDigitalAddress>());
                Mapper mp = new Mapper(config);
                var dc = mp.Map<ContactDigitalAddress>(bd);

                var r = await userService.AddOrUpdateAddress(dc, User.Identity.Name);

                return r > 0 ? await userService.GetDigitalAddressById(dc.Id) : BadRequest("L'inserimento non è andato a buon fine.");
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
        /// Aggiorna un ContactDigitalAddress dell'utente loggato
        /// </summary>
        /// <returns>Restituisce oggetto ContactDigitalAddress aggiornato </returns>
        [Authorize]
        [HttpPut("User/Contact/DigitalAddress/{Id}")]
        public async Task<ActionResult<ContactDigitalAddress>> UpdateContactDigitalAddress(AddOrUpdateContactDigitalAddress bd, int Id)
        {
            try
            {
                if (await userService.GetDigitalAddressById(Id) != null)
                {
                    var config = new MapperConfiguration(cfg => cfg.CreateMap<AddOrUpdateContactDigitalAddress, ContactDigitalAddress>());
                    Mapper mp = new Mapper(config);
                    var dc = mp.Map<ContactDigitalAddress>(bd);

                    dc.Id = Id;

                    var r = await userService.AddOrUpdateAddress(dc, User.Identity.Name);
                    return r > 0 ? await userService.GetDigitalAddressById(Id) : BadRequest("L'aggiornamento non è andato a buon fine.");
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
            var result = await userService.DeleteContactDigitalAddress(Id, User.Identity.Name);
            return result > 0 ? Ok() : BadRequest("Eliminazione non è andata a buon fine.");
        }

    }
}
