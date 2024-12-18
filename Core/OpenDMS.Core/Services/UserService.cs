using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenDMS.Core.DTOs;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Constants;
using OpenDMS.Domain.Entities.OrganizationUnits;
using OpenDMS.Domain.Entities.Settings;
using OpenDMS.Domain.Entities.Users;
using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Infrastructure;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Repositories;
using OpenDMS.Domain.Services;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Security.Claims;
using System.Text;
//using Color = SixLabors.ImageSharp.Color;

namespace OpenDMS.Core.BusinessLogic
{
    public class UserService : IUserService
    {

        private readonly IUserRepository _repository;
        private readonly IOrganizationUnitService _organizationService;
        private readonly IRoleRepository roleRepository;
        private readonly IVirtualFileSystemProvider _filesystemProvider;
        private readonly ICompanyRepository companyRepository;
        private readonly IUserSettingsRepository userSettings;
        private readonly ILogger<UserService> logger;
        private readonly string _folder = "";
        private readonly string _font = "";
        private readonly string _filesystemType = "";

        public UserService(
            IUserRepository userRepo,
            IOrganizationUnitService organizationRepository,
            IRoleRepository roleRepository,
            IVirtualFileSystemProvider fileSystemProvider,
            ICompanyRepository companyRepository,
            IUserSettingsRepository userSettings,
            ILogger<UserService> logger,
            IConfiguration configuration
            )
        {
            this._repository = userRepo;
            this._organizationService = organizationRepository;
            this.roleRepository = roleRepository;
            this._filesystemProvider = fileSystemProvider;
            this.companyRepository = companyRepository;
            this.userSettings = userSettings;
            this.logger = logger;
            _folder = configuration[StaticConfiguration.CONST_DOCUMENTS_ROOLFOLDER];
            _font = configuration[StaticConfiguration.CONST_AVATAR_FONT];
            if (string.IsNullOrEmpty(_font)) _font = "Ubuntu";
            _filesystemType = configuration[StaticConfiguration.CONST_DOCUMENTS_FILESYSTEMTYPE];
        }

        public IUserSettingsRepository Settings => userSettings;

        public async Task<List<User>> GetAll(bool IncludesDeleted)
        {
            List<User> users = new List<User>();
            return await _repository.GetAll(IncludesDeleted);
        }


        public async Task<List<User>> GetAllByGroup(string userGroupId, string filter)
        {
            UserFilter f = new UserFilter() { userGroupId = userGroupId, filter = filter };
            List<User> users = new List<User>();
            return await _repository.GetByFilter(f);
        }
        public async Task<List<User>> GetAllByRole(string roleId, string filter)
        {
            UserFilter f = new UserFilter() { roleId = roleId, filter = filter };
            List<User> users = await _repository.GetByFilter(f);
            return users;
        }
        public async Task<List<User>> GetAllDeleted(string filter)
        {
            UserFilter f = new UserFilter() { IncludeDeletes = true, filter = filter };
            List<User> users = new List<User>();
            return await _repository.GetByFilter(f);
        }

        public async Task<List<ContactDigitalAddress>> FindMailAddresses(string SearchText, int MaxResults = 0)
        {
            return await _repository.FindMailAddresses(SearchText, MaxResults);
        }
        public async Task<ContactDigitalAddress> FindMailAddress(string SearchName, string address)
        {
            return await _repository.FindMailAddress(SearchName, address);
        }

        public async Task<ContactDigitalAddress> GetOrCreateAddress(string SearchName, string address, DigitalAddressType addressType)
        {
            ContactDigitalAddress c = new ContactDigitalAddress();
            c.SearchName = SearchName.Trim().ToLower()
                .Replace(" ", "")
                .Replace("\"", "")
                //.Replace("'", "")
                .Replace(".", "")
                .Replace("à", "a")
                .Replace("è", "e")
                .Replace("ì", "i")
                .Replace("ò", "o")
                .Replace("ù", "u")
                .Replace(" ", "");
            c.Address = address;
            c.Name = SearchName + "<" + address + ">";
            c.DigitalAddressType = addressType;
            var r = await _repository.AddOrUpdateAddress(c, SpecialUser.SystemUser);
            return c;
        }

        public async Task<string> GetAttribute(string userId, string attributeId, int companyId = 0)
        {
            var cid = await _repository.GetById(userId);
            if (cid == null) return "";
            var value = await userSettings.Get(companyId, cid.ContactId, attributeId);
            return value;
        }
        public async Task SetAttribute(string userId, string attributeId, string value, int companyId = 0)
        {
            var cid = await _repository.GetById(userId);
            if (cid != null) await userSettings.Set(companyId, cid.ContactId, attributeId, value);
        }


        #region Avatar
        public async Task<byte[]> GetAvatar(string userId)
        {
            return await GetAvatar(userId, ProfileType.User);
        }
        public async Task<byte[]> GetAvatar(string profileId, ProfileType profileType)
        {
            var TenanFolder = _folder;
            if (!String.IsNullOrWhiteSpace(profileId))
            {
                var AvatarFolder = Path.Combine(TenanFolder, "Users", ((int)profileType).ToString() + profileId, "Avatars");
                var AvatarImage = Path.Combine(AvatarFolder, "Default.png");
                IVirtualFileSystem FM = null;
                try
                {
                    FM = await _filesystemProvider.InstanceOf(_filesystemType);
                    if (FM != null && await FM.Exists(AvatarImage))
                    {
                        return await FM.ReadAllBytes(AvatarImage);
                    }
                }
                catch { }
            }
            return await GetDefaultAvatar(profileId, profileType);
        }

        public async Task<byte[]> GetDefaultAvatar(string profileId, ProfileType profileType)
        {
            var TenanFolder = _folder;
            if (profileType == ProfileType.Group)
                profileId = "G";
            if (profileType == ProfileType.Role)
                profileId = "R";
            if (profileType == ProfileType.User && String.IsNullOrWhiteSpace(profileId))
                profileId = "U";

            string Initials = profileId.Substring(0, 1);
            int i = profileId.IndexOf("_");
            if (i < 0) i = profileId.IndexOf(".");
            if (i < 0) i = profileId.IndexOf("-");
            if (i < 0 && profileId.Length > 1) i = 0;
            if (i >= 0)
            {
                Initials += profileId.Substring(1 + i, 1);
            }
            Initials = Initials.ToUpper();
            byte r, g, b, a = 255;
            r = (byte)(4 * ((byte)Initials[0] - 16));
            if (i >= 0)
                b = (byte)((8 * (byte)Initials[1]) - (byte)Initials[0]);
            else
                b = 0;
            g = (byte)((profileId.Length % 16) * 16);
            //r = (byte)(2 * ((byte)Initials[0] - 16));
            //if (i >= 0)
            //    b = (byte)((3 * (byte)Initials[1]) - (byte)Initials[0]);
            //else
            //    b = 0;
            //g = (byte)(255 - (r + b) / 2);
            //Image<Rgba32> image = new((int)48, (int)48, new Rgba32(r, g, b, a));
            Image<Rgba32> image = new((int)48, (int)48, new Rgba32(r, g, b, a));

            SixLabors.Fonts.FontFamily fontFamily;
            var HasFont = true;
            try
            {
                if (!SystemFonts.TryGet(_font, out fontFamily))
                    if (SystemFonts.Collection.Families.FirstOrDefault() != null)
                    {
                        fontFamily = SystemFonts.Collection.Families.FirstOrDefault();
                        logger.LogError("Il font richiesto per gli Avatar (" + _font + ") non è installato. Uso " + fontFamily.Name);

                    }
                    else
                        HasFont = false;

                if (HasFont)
                {
                    var font = fontFamily.CreateFont((float)(image.Height / 1.75), FontStyle.Regular);

                    var options = new TextOptions(font)
                    {
                        Dpi = 150,
                        KerningMode = KerningMode.Standard,
                        VerticalAlignment = VerticalAlignment.Center
                    };
                    var rect = TextMeasurer.MeasureAdvance(Initials, options);
                    image.Mutate(x => x.DrawText(
                        Initials,
                        font,
                        new Color(new Rgba32(255 - r, g, 255 - b, a)),
                        new PointF((int)((image.Width - (rect.Width / 2)) / 2),
                                (image.Height - (rect.Height / 2)) / 2)
                    ));
                }

                else
                {
                    logger.LogError("Nessun Font disponibile per renderizzare le iniziali dell'Avatar");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "GetAvatar");
            };
            byte[] data = null;
            using (var M = new MemoryStream())
            {
                image.Save(M, new SixLabors.ImageSharp.Formats.Png.PngEncoder());
                data = M.ToArray();
            }
            return data;
        }
        public async Task<List<Tuple<string, byte[]>>> GetUploadedAvatars(string profileId, ProfileType profileType)
        {

            var TenanFolder = _folder;
            var AvatarFolder = Path.Combine(TenanFolder, "Users", ((int)profileType).ToString() + profileId, "Avatars");
            var query = "uploaded_" + ((int)profileType).ToString() + profileId + "*.png";

            List<Tuple<string, byte[]>> res = new();

            IVirtualFileSystem FM = null;

            FM = await _filesystemProvider.InstanceOf(_filesystemType);
            if (FM != null && Directory.Exists(AvatarFolder))
            {
                try
                {
                    foreach (var filename in FM.GetFiles(AvatarFolder, query).ToList())
                    {
                        var bytes = await FM.ReadAllBytes(filename);
                        var guid = filename.Substring(filename.LastIndexOf('_') + 1, filename.IndexOf(".png") - filename.LastIndexOf('_') - 1);
                        res.Add(new Tuple<string, byte[]>(guid, bytes));
                    }
                }
                catch { }
            }
            return res;
        }
        public async Task SetAvatar(string profileId, ProfileType profileType, byte[] imageBytes)
        {
            var TenanFolder = _folder;
            var AvatarFolder = Path.Combine(TenanFolder, "Users", ((int)profileType).ToString() + profileId, "Avatars");
            var AvatarImage = Path.Combine(AvatarFolder, "Default.png");

            IVirtualFileSystem FM = null;

            FM = await _filesystemProvider.InstanceOf(_filesystemType);
            if (FM != null)
            {
                await FM.Delete(AvatarImage);
                if (imageBytes != null)
                {
                    await FM.WriteAllBytes(AvatarImage, imageBytes);
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="profileId"></param>
        /// <param name="profileType"></param>
        /// <param name="guid">Guid dell'immagine caricata dall'utente</param>
        /// <returns></returns>
        public async Task SetAvatar(string profileId, ProfileType profileType, string guid)
        {

            byte[] bytes = null;

            if (!string.IsNullOrEmpty(guid))
            {
                var TenanFolder = _folder;
                var AvatarFolder = Path.Combine(TenanFolder, "Users", ((int)profileType).ToString() + profileId, "Avatars");
                var AvatarImage = Path.Combine(AvatarFolder, "uploaded_" + ((int)profileType).ToString() + profileId + "_" + guid + ".png");

                IVirtualFileSystem FM = null;
                FM = await _filesystemProvider.InstanceOf(_filesystemType);

                if (FM != null && await FM.Exists(AvatarImage))
                {
                    bytes = await FM.ReadAllBytes(AvatarImage);
                }
            }
            await SetAvatar(profileId, profileType, bytes);
        }
        public async Task UploadAvatar(string profileId, ProfileType profileType, byte[] imageBytes)
        {
            var TenanFolder = _folder;
            var AvatarFolder = Path.Combine(TenanFolder, "Users", ((int)profileType).ToString() + profileId, "Avatars");
            var AvatarImage = Path.Combine(AvatarFolder, "uploaded_" + ((int)profileType).ToString() + profileId + "_" + Guid.NewGuid().ToString() + ".png");

            IVirtualFileSystem FM = null;

            FM = await _filesystemProvider.InstanceOf(_filesystemType);
            if (FM != null)
            {
                await FM.Delete(AvatarImage);
                await FM.WriteAllBytes(AvatarImage, imageBytes);
            }
        }
        public async Task DeleteAvatar(string profileId, ProfileType profileType, string guid)
        {
            var TenanFolder = _folder;
            var AvatarFolder = Path.Combine(TenanFolder, "Users", ((int)profileType).ToString() + profileId, "Avatars");
            var AvatarImage = Path.Combine(AvatarFolder, "uploaded_" + ((int)profileType).ToString() + profileId + "_" + guid + ".png");

            IVirtualFileSystem FM = null;

            FM = await _filesystemProvider.InstanceOf(_filesystemType);
            if (FM != null)
            {
                await FM.Delete(AvatarImage);
            }
        }

        #endregion

        #region UserStamp

        public async Task<byte[]> GetUserStamp(string profileId, ProfileType profileType, string stampId)
        {
            var TenanFolder = _folder;
            var StampFolder = Path.Combine(TenanFolder, "Users", ((int)profileType).ToString() + profileId, "Stamps");
            var StampImage = Path.Combine(StampFolder, stampId + ".png");
            IVirtualFileSystem FM = null;
            try
            {
                FM = await _filesystemProvider.InstanceOf(_filesystemType);
                if (FM != null && await FM.Exists(StampImage))
                {
                    return await FM.ReadAllBytes(StampImage);
                } 
            }
            catch { }
            return null;
        }

        public async Task SetUserStamp(string profileId, ProfileType profileType, string stampId, byte[] stampBytes)
        {
            var TenanFolder = _folder;
            var StampFolder = Path.Combine(TenanFolder, "Users", ((int)profileType).ToString() + profileId, "Stamps");
            var StampImage = Path.Combine(StampFolder, stampId + ".png");
            IVirtualFileSystem FM = null;

            FM = await _filesystemProvider.InstanceOf(_filesystemType);
            if (FM != null)
            {
                await FM.Delete(StampImage);
                await FM.WriteAllBytes(StampImage, stampBytes);
            }
        }

        #endregion

        public async Task<UserProfile> GetUserProfile(string userId)
        {
            ArgumentNullException.ThrowIfNull(userId);

            var groups = (await _organizationService.GetGroupsByUser(userId));
            List<Company> companies = new();
            var si = await companyRepository.GetAll();
            foreach (var s in si)
            {
                if (!string.IsNullOrEmpty(s.ERP))
                {
                    var cg = await _organizationService.GetById(s.ERP);
                    var included = (cg == null) || (groups.Any(g => g.NodeInfo != null && g.NodeInfo.LeftBound >= cg.LeftBound && g.NodeInfo.RightBound <= cg.RightBound));
                    if (included)
                    {
                        companies.Add(s);
                    }
                }
                else
                {
                    companies.Add(s);
                }
            }


            UserProfile UP = new UserProfile();
            UP.userId = userId ?? ""; ;
            UP.UserInfo = await GetById(userId);

            UP.GlobalRoles = groups.Select<UserInGroup, LookupTable>(s => new LookupTable() { Id = s.RoleId, Description = s.RoleName, Annotation = s.RoleName }).DistinctBy(g => g.Id).ToList();
            UP.Roles = groups.Select<UserInGroup, LookupTable>(s => new LookupTable() { Id = (!String.IsNullOrEmpty(s.UserGroupId) ? s.RoleId + "\\" + s.UserGroupId : s.RoleId), Description = (!String.IsNullOrEmpty(s.UserGroupId) ? s.RoleName + "\\" + s.UserGroup.ShortName : s.RoleName), Annotation = (!String.IsNullOrEmpty(s.UserGroupId) ? s.RoleName + "\\" + s.UserGroup.Name : s.RoleName) }).DistinctBy(g => g.Id).ToList();
            UP.Groups = groups.Where(s => !String.IsNullOrEmpty(s.UserGroupId)).Select<UserInGroup, LookupTable>(s => new LookupTable() { Id = s.UserGroupId, Description = s.UserGroup.ShortName, Annotation = s.UserGroup.Name, TableId=s.UserGroup.ExternalId }).DistinctBy(g => g.Id).ToList();
            UP.Companies = companies;
            UP.LoggedUser = userId;
            return UP;
        }

        public async Task<string> GetName(string userId)
        {
            //            if (userId == SpecialUser.AllUsers)
            //                return "Tutti gli utenti";
            //            else 
            var Name = (await GetById(userId))?.Contact?.FriendlyName;
            if (string.IsNullOrEmpty(Name)) Name = "?" + userId;
            return Name;

        }

        public async Task<List<LookupTable>> GetGroups(string userId)
        {
            Dictionary<string, string> storedGroupName = new Dictionary<string, string>();
            List<LookupTable> groups = new List<LookupTable>();
            foreach (var g in (await _organizationService.GetGroupsByUser(userId)).Select(u => u.UserGroupId).Distinct())
            {
                if (g != null)
                {
                    var group = await _organizationService.GetById(g);
                    if (group != null && group.Visible)
                    {
                        var ShortName = group.ShortName;
                        var LongName = group.Name;
                        var parentId = group.ParentUserGroupId;
                        StringBuilder path = new StringBuilder();
                        while (!String.IsNullOrEmpty(group.ParentUserGroupId))
                        {
                            string parentName = "";
                            if (storedGroupName.ContainsKey(group.ParentUserGroupId))
                            {
                                parentName = storedGroupName[group.ParentUserGroupId];
                                group.ParentUserGroupId = "";
                            }
                            else
                            {
                                group = await _organizationService.GetById(group.ParentUserGroupId);
                                parentName = group.ShortName;
                                storedGroupName[group.UserGroupId] = parentName;
                            }
                            path.Insert(0, $"{parentName}\\");
                        }
                        LookupTable table = new LookupTable { Id = g, Description = ShortName, Annotation = LongName, TableId = path.ToString() };
                        groups.Add(table);
                    }
                }
            }
            return groups;
        }


        public async Task<List<LookupTable>> GetRoles(string userId)
        {
            return (await _organizationService
                .GetGroupsByUser(userId))
                    .Select(u => new LookupTable() { Id = u.RoleId + (u.UserGroupId != null ? "\\" + u.UserGroupId : ""), Description = u.UserGroupId != null ? u.RoleName + "\\" + u.UserGroup.ShortName : u.RoleName }).Distinct().ToList();
        }

        public async Task<User> AddOrUpdate(User new_user)
        {
            if (string.IsNullOrEmpty(new_user.Contact.FriendlyName)) new_user.Contact.FriendlyName = new_user.Contact.FullName;
            if (string.IsNullOrEmpty(new_user.Contact.FriendlyName)) new_user.Contact.FriendlyName = new_user.Contact.SearchName;
            new_user.Contact.SearchName = new_user.Contact.FriendlyName.ToLower()
                .Replace(".", "")
                .Replace("/", "")
                .Replace("\\", "")
                .Replace("*", "")
                .Replace("'", "")
                .Replace("!", "")
                .Replace("?", "")
                .Replace("  ", " ")
                .Replace("+", "")
                .Replace("-", "")
                .Replace("(", "")
                .Replace(")", "")
                .Replace("[", "")
                .Replace("]", "")
                .Replace("{", "")
                .Replace("}", "")
                .Replace("#", "")
                .Replace("\"", "");
            return await _repository.AddOrUpdate(new_user);
        }

        public async Task<User> UpdateUserInfo(ClaimsPrincipal Claim)
        {
            var uid = Claim.Identity.Name;
            var name = Claim.Claims.FirstOrDefault(c => c.Type == "preferred_username").Value;
            var mail = Claim.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email).Value;
            var U = await _repository.GetById(uid);
            if (U == null)
            {
                string cid = Guid.NewGuid().ToString();
                //aggiungo l'utente all'archivio
                U = new User();
                U.Id = uid;
                U.ContactId = cid;
                U.Contact = new Contact() { Id = cid, FullName = name, FriendlyName = name, SearchName = name };
            }
            // Verifico l'esistenza dei ruoli
            foreach (var role in Claim.Claims.Where(c => c.Type == System.Security.Claims.ClaimTypes.Role))
            {
                var r = await roleRepository.GetById(role.Value);
                if (r == null)
                {
                    r = new Role();
                    r.Id = role.Value;
                    r.RoleName = role.Value;
                    await roleRepository.Insert(r);
                    UserInGroup UG = new UserInGroup();
                    UG.UserGroupId = "$USERS$";
                    UG.RoleName = r.Id;
                    UG.UserId = uid;
                    await _organizationService.AddUser(UG);
                }
            }
            U = await _repository.AddOrUpdate(U);
            return U;
        }

        public async Task<User> GetById(string userId)
        {
            //            if (userId == SpecialUser.SystemUser)
            //                return User.SystemUser();
            //            else
            return await _repository.GetById(userId);
        }

        #region ContactDigitalAddress

        public async Task<List<ContactDigitalAddress>> GetAllContactDigitalAddress(string userId)
        {
            return await _repository.GetAllAddresses(userId);
        }
        public async Task<List<DigitalAddress>> GetDigitalAddress(string userId)
        {
            return null;// await _repository.GetAllAddresses(userId);
        }
        //public async Task<List<Contact>> FindContactsByDigitalAddress(string address)
        //{
        //    return await _repository.FindContactsByDigitalAddress(address);

        //}
        public async Task<List<ContactDigitalAddress>> GetAllDeletedContactDigitalAddress(string userId)
        {
            return await _repository.GetAllDeletedContactDigitalAddress(userId);
        }

        public async Task<ContactDigitalAddress> GetDigitalAddressById(int DigitalAddressId)
        {
            return await _repository.GetDigitalAddressById(DigitalAddressId);
        }

        public async Task<int> AddOrUpdateAddress(ContactDigitalAddress bd, string executor)
        {
            return await _repository.AddOrUpdateAddress(bd, executor);
        }

        public async Task<int> DeleteContactDigitalAddress(int ContactDigitalAddressId, string executor)
        {
            return await _repository.DeleteAddress(ContactDigitalAddressId, executor);
        }

        #endregion

        public async Task<List<User>> Find(string SearchText, int MaxResults = 0)
        {
            return await _repository.Find(SearchText, MaxResults);

        }
        public async Task<string> GetProfileName(string Profile)
        {
            string ProfileId = Profile.Substring(1);
            ProfileType ProfileType = (ProfileType)(int.Parse(Profile.Substring(0, 1)));
            return await GetProfileName(ProfileId, ProfileType);
        }
        public async Task<string> GetProfileName(string ProfileId, ProfileType ProfileType)
        {
            if (string.IsNullOrEmpty(ProfileId)) return "";
            switch (ProfileType)
            {
                case ProfileType.User:
                    var UName = await GetName(ProfileId);
                    return UName;
                case ProfileType.Group:
                    var GroupId = ProfileId;
                    var Name = ((await _organizationService.GetGroup(GroupId))?.ShortName) ?? "";
                    if (string.IsNullOrEmpty(Name)) Name = "?" + GroupId;
                    return Name;
                case ProfileType.Role:
                    var RoleId = ProfileId;
                    var RGroupId = "";
                    var i = RoleId.IndexOf("\\");
                    if (i >= 0)
                    {
                        RGroupId = RoleId.Substring(i + 1);
                        RoleId = RoleId.Substring(0, i);
                    }
                    var RName = (await roleRepository.GetById(RoleId))?.RoleName;
                    if (string.IsNullOrEmpty(RName)) RName = "?" + RoleId;
                    if (!String.IsNullOrEmpty(RGroupId))
                        RName += " (" + (((await _organizationService.GetGroup(RGroupId))?.ShortName) ?? "?" + RGroupId) + ")";
                    return RName;

                case ProfileType.MailAddress:
                    break;
                default:
                    break;
            }
            return "";
        }

        public async Task<List<User>> GetFilteredAndPaginatedUsers(string SearchName, int pageSize, int pageNumber, bool IncludesDeleted = false)
        {
            return await _repository.GetFilteredAndPaginatedUsers(SearchName, pageSize, pageNumber, IncludesDeleted);
        }

        public async Task<int> GetTotalCountOfFilteredUsers(string SearchName, bool IncludesDeleted = false)
        {
            return await _repository.GetTotalCountOfFilteredUsers(SearchName, IncludesDeleted);
        }
    }
}
