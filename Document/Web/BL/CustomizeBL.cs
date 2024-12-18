using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Entities.Users;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Services;
using Web.BL.Interface;
using Web.DTOs;

namespace Web.BL
{
    public class CustomizeBL : ICustomizeBL
    {
        private readonly ILoggedUserProfile _userContext;
        private readonly ILookupTableService lookupService;
        private readonly IUserService _userService;
        private readonly IACLService _aclService;
        private readonly IWebHostEnvironment _env;
        private readonly UserProfile userProfile;

        public CustomizeBL(IConfiguration config, ILoggedUserProfile userContext, ILookupTableService lookupService, IUserService userService, IWebHostEnvironment env, IACLService aclService)
        {
            _userContext = userContext;
            this.lookupService = lookupService;
            _userService = userService;
            _aclService = aclService;
            _env = env;
            userProfile = _userContext.Get();
        }

        public async Task<User> GetUser()
        {
            var userId = userProfile.userId;
            return await _userService.GetById(userId);
        }

        public UserProfile GetUserProfile()
        {
            return userProfile;
        }

        public async Task<List<string>> GetRoles()
        {
            return userProfile.Roles.Select(s=>s.Description).ToList();
        }
        public async Task<List<string>> GetGroups()
        {
            return userProfile.Groups.Select(s => s.Description).ToList();
        }

        private async Task<List<string>> GetNames(List<string> IdList, ProfileType profileType)
        {
            var res = new List<string>();
            foreach (var id in IdList)
            {
                res.Add(await _userService.GetProfileName(id, profileType));
            }
            return res;
        }

        public async Task<List<ContactDigitalAddress>> GetUserMails()
        {
            return await _userService.GetAllContactDigitalAddress(userProfile.userId);
        }
        public async Task<List<ACLPermission_DTO>> GetAuthorizations()
        {
            //creo una lista di stringhe con gli id dei profili assegnati l'utente
            var ProfileIdList = new List<string>();
            ProfileIdList.AddRange(userProfile.Groups.Select(s=>s.Id));
            ProfileIdList.AddRange(userProfile.Roles.Select(s => s.Id));

            var GlobalPermissionslist = await _aclService.GetAllPermissions("$GLOBAL$");

            List<ACLPermission_DTO> res = new();

            foreach (var permission in GlobalPermissionslist)
            {
                if (ProfileIdList.Any(p => p != null && p.Equals(permission.ProfileId)))
                {
                    var permissionName = (await lookupService.GetById("$PERMISSIONS$", permission.PermissionId)).Description;
                    res.Add(new ACLPermission_DTO(permission.ProfileId, permissionName, permission.Authorization));
                }
            }
            return res;
        }

        public async Task<List<ACLPermission_DTO>> GetAuthorizationsForUser(string userId)
        {
            //creo una lista di stringhe con gli id dei profili assegnati l'utente
            var user = await _userService.GetUserProfile(userId);
            var ProfileIdList = new List<string>();
            ProfileIdList.AddRange(user.Groups.Select(s => s.Id));
            ProfileIdList.AddRange(user.Roles.Select(s => s.Id));

            var GlobalPermissionslist = await _aclService.GetAllPermissions("$GLOBAL$");

            List<ACLPermission_DTO> res = new();

            foreach (var permission in GlobalPermissionslist)
            {
                if (ProfileIdList.Any(p => p != null && p.Equals(permission.ProfileId)))
                {
                    var permissionName = (await lookupService.GetById("$PERMISSIONS$", permission.PermissionId)).Description;
                    res.Add(new ACLPermission_DTO(permission.ProfileId, permissionName, permission.Authorization));
                }
            }
            return res;
        }

        public async Task<string> GetUserAttribute(string name)
        {
            return await _userService.GetAttribute(userProfile.userId, name);
        }

        public async Task UpdateUser(User user)
        {
            await _userService.AddOrUpdate(user);
        }

        public async Task SetUserAttribute(string name, string value)
        {
            await _userService.SetAttribute(userProfile.userId, name, value);
        }

        public async Task<List<Tuple<string, string>>> GetUploadedAvatars()
        {
            string profileId = userProfile.userId;
            ProfileType profileType = ProfileType.User;

            var AvatarsInBytes = await _userService.GetUploadedAvatars(profileId, profileType);

            List<Tuple<string, string>> res = new();

            AvatarsInBytes.ForEach(avatar =>
            {
                res.Add(new Tuple<string, string>(avatar.Item1, "data:image/png;base64," + Convert.ToBase64String(avatar.Item2)));
            });

            return res;
        }

        public async Task SetAvatar(string imageInput)
        {
            //imageInput può essere:
            //- un path in caso di immagine sceltra tra gli avatar di default nel wwwroot (quindi comincia con "/")
            //- Un guid in caso di immagine scelta tra le foto caricate dall'utente

            if (imageInput != null && imageInput.StartsWith("/")) //Caso: immagine di default (nel wwwroot)
            {
                imageInput = _env.WebRootPath + imageInput;

                byte[] imageBytes = null;

                // Verifica se il file esiste e se è un file PNG
                if (File.Exists(imageInput) && Path.GetExtension(imageInput).ToLower() == ".png")
                {
                    imageBytes = File.ReadAllBytes(imageInput);

                    string profileId = userProfile.userId;
                    ProfileType profileType = ProfileType.User;

                    await _userService.SetAvatar(profileId, profileType, imageBytes);
                }
                else
                {
                    throw new Exception("Il percorso specificato non corrisponde a un file PNG o il file non esiste.");
                }

            }
            else                            //Caso: immagine caricata dall'utente
            {
                string profileId = userProfile.userId;
                ProfileType profileType = ProfileType.User;

                await _userService.SetAvatar(profileId, profileType, imageInput);
            }
        }

        public async Task DeleteAvatar(string guid)
        {
            UserProfile user = userProfile;
            string profileId = user.userId;
            ProfileType profileType = ProfileType.User;

            await _userService.DeleteAvatar(profileId, profileType, guid);
        }

        public async Task UploadAvatar(IFormFile image)
        {

            if (image == null || image.Length == 0)
                throw new Exception("L'immagine non è stata caricata correttamente.");

            string fileExtension = Path.GetExtension(image.FileName);

            if (!fileExtension.Equals(".png", StringComparison.OrdinalIgnoreCase))
                throw new Exception("Il file caricato non è un'immagine PNG.");

            long fileSizeLimit = 5 * 1024 * 1024; // 5 MB
            if (image.Length > fileSizeLimit)
                throw new Exception("Il file caricato supera la dimensione massima consentita (5 MB).");

            byte[] imageBytes = null;

            using (var memoryStream = new MemoryStream())
            {
                image.CopyTo(memoryStream);
                imageBytes = memoryStream.ToArray();
            }

            string profileId = userProfile.userId;
            ProfileType profileType = ProfileType.User;

            await _userService.UploadAvatar(profileId, profileType, imageBytes);

        }

        #region HandWrittenSign e Visto

        public async Task SetHandWrittenSign(string signBase64)
        {

            if (String.IsNullOrEmpty(signBase64))
                throw new Exception("La firma non è stata caricata correttamente.");

            var signBytes = Convert.FromBase64String(signBase64);

            string profileId = userProfile.userId;
            ProfileType profileType = ProfileType.User;

            await _userService.SetUserStamp(profileId, profileType, "Sign", signBytes);
        }

        public async Task DeleteHandWrittenSign()
        {
            string profileId = userProfile.userId;
            ProfileType profileType = ProfileType.User;

            await _userService.SetUserStamp(profileId, profileType, "Sign", new byte[]{});
        }
        
        public async Task SetVisto(string vistoBase64)
        {

            if (String.IsNullOrEmpty(vistoBase64))
                throw new Exception("Il visto non è stato caricato correttamente.");

            var vistoBytes = Convert.FromBase64String(vistoBase64);

            string profileId = userProfile.userId;
            ProfileType profileType = ProfileType.User;

            await _userService.SetUserStamp(profileId, profileType, "Visto", vistoBytes);
        }

        public async Task DeleteVisto()
        {
            string profileId = userProfile.userId;
            ProfileType profileType = ProfileType.User;

            await _userService.SetUserStamp(profileId, profileType, "Visto", new byte[]{});
        }

        #endregion

    }
}
