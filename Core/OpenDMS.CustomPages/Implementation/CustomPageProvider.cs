using Microsoft.Extensions.Configuration;
using OpenDMS.CustomPages.Models;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Models;
using OpenDMS.Domain.Repositories;
using OpenDMS.Core.Utilities;
using System.Reflection.Metadata.Ecma335;
using OpenDMS.Domain.Services;
using OpenDMS.Domain.Entities.Settings;

namespace OpenDMS.CustomPages.Implementation
{
    public class CustomPageProvider : ICustomPageProvider
    {
        private readonly IConfiguration config;
        private readonly ICustomPagesRepository repo;
        private readonly IUserService userService;
        private readonly ILookupTableRepository tableRepository;
        private readonly IACLService aCLService;

        public CustomPageProvider(IConfiguration config,
            ICustomPagesRepository customPagesRepository,
            IUserService userService,
            ILookupTableRepository tableRepository,
            IACLService aCLService
            )
        {
            this.config = config;
            this.repo = customPagesRepository;
            this.userService = userService;
            this.tableRepository = tableRepository;
            this.aCLService = aCLService;
        }
        public async Task<CustomPageDTO> GetPage(UserProfile userInfo, string pageId)
        {
            var page = repo.GetPage(pageId);
            if (page == null) return null;

            foreach (var p in page.Permissions.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            {
                var ok = false;
                if (p.StartsWith(":"))
                {
                    ok = userInfo.Roles.Select(s => s.Id).Contains(p.Substring(1));
                    if (!ok) ok = userInfo.GlobalRoles.Select(s => s.Id).Contains(p.Substring(1));
                }
                else
                if (p.StartsWith("@"))
                {
                    ok = userInfo.userId.ToLower().CompareTo(p.Substring(1).ToLower()) == 0;
                }
                else
                {
                    var t = await tableRepository.GetById("$PERMISSIONS$", p, false);
                    if (t == null)
                    {
                        await tableRepository.Insert(new() { Id = p, TableId = "$PERMISSIONS$", Description = p });
                    }
                    else
                        ok = await aCLService.GetAuthorization("", userInfo, p) == Domain.Enumerators.AuthorizationType.Granted;
                }
                if (!ok) return null;
            }
            var URL = page.URL;
            CustomPageDTO P = new();
            var User = await userService.GetById(userInfo.userId);
            P.ParentPageId = page.ParentPageId;
            P.PageId = page.PageId;
            P.Target = page.Target; // URL.StartsWith("*") ? "_blank" : "";
            if (URL.StartsWith("*")) URL = URL.Substring(1);
            P.Icon = page.Icon
                .Parse(userInfo, "UserProfile")
                .Parse(User, "User");
            P.Title = page.Title
                .Parse(userInfo, "UserProfile")
                .Parse(User, "User");
            P.URL = URL
                .Parse(userInfo, "UserProfile")
                .Parse(User, "User");
            P.Alignment = page.Alignment;
            P.Tooltip = page.ToolTip
                .Parse(userInfo, "UserProfile")
                .Parse(User, "User");
            if (page.IncludeSubMenus)
                P.SubItems = await GetPages(userInfo, P.PageId);
            return P;
        }

        public async Task<List<CustomPageDTO>> GetPages(UserProfile userInfo, string pageId)
        {
            List<CustomPageDTO> Pages = new();
            var pages = repo.GetPages(pageId);
            foreach (var page in pages)
            {
                bool ok = string.IsNullOrEmpty(page.Permissions);
                if (!ok)
                {
                    foreach (var p in page.Permissions.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                    {
                        if (p.StartsWith(":"))
                        {
                            ok = userInfo.Roles.Select(s => s.Id).Contains(p.Substring(1));
                        }
                        else
                        if (p.StartsWith("@"))
                        {
                            ok = userInfo.userId.ToLower().CompareTo(p.Substring(1).ToLower()) == 0;
                        }
                        else
                        {
                            var t = await tableRepository.GetById("$PERMISSIONS$", p, false);
                            if (t == null)
                            {
                                await tableRepository.Insert(new() { Id = p, TableId = "$PERMISSIONS$", Description = p });
                            }
                            else
                            {
                                ok = await aCLService.GetAuthorization("", userInfo, p) == Domain.Enumerators.AuthorizationType.Granted;
                                if (!ok) break;
                            }
                        }
                    }
                }
                if (ok)
                {
                    var User = await userService.GetById(userInfo.userId);
                    CustomPageDTO P = new();
                    P.ParentPageId = page.ParentPageId;
                    P.PageId = page.PageId;
                    if (!page.Icon.StartsWith("<"))
                        page.Icon = "<i class='" + page.Icon + "'></i>";
                    P.Icon = page.Icon
                        .Parse(userInfo, "UserProfile")
                        .Parse(User, "User");
                    P.Title = page.Title
                        .Parse(userInfo, "UserProfile")
                        .Parse(User, "User");
                    P.URL = page.URL
                        .Parse(userInfo, "UserProfile")
                        .Parse(User, "User");
                    P.Alignment = page.Alignment;
                    P.Tooltip = page.ToolTip
                        .Parse(userInfo, "UserProfile")
                        .Parse(User, "User");
                    P.SubItems = await GetPages(userInfo, P.PageId);
                    if (!String.IsNullOrEmpty(page.BadgeURL))
                    {
                        P.BadgeURL = page.BadgeURL
                            .Parse(userInfo, "UserProfile")
                            .Parse(User, "User");
                        P.BadgeMessage = "Hai {0} " + P.Title;
                    }
                    Pages.Add(P);
                }
            }
            return Pages;
        }
        public async Task Register(CustomPage page)
        {
            repo.Register(page);
        }

        public async Task Remove(string pageId)
        {
            var P = repo.GetPage(pageId);
            repo.Remove(P);
        }




    }
}
