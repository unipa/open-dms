using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using OpenDMS.Core.Interfaces;

namespace OpenDMS.Core.Filters
{
    public class AuthorizationAttribute : Attribute, IAsyncPageFilter, IAsyncActionFilter
    {
        private readonly string permissionType;
        private readonly string uid;

        public AuthorizationAttribute(string permissionType)
        {
            this.permissionType = permissionType;
        }
        public AuthorizationAttribute(string permissionType, string uid)
        {
            this.permissionType = permissionType;
            this.uid = uid;
        }


        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context == null || context.HttpContext == null) throw new ArgumentNullException(nameof(context));

            var loggedUserService = context.HttpContext.RequestServices.GetRequiredService<ILoggedUserProfile>();
            var userInfo = loggedUserService.Get();
            var A = Domain.Enumerators.AuthorizationType.Denied;
            if (userInfo != null)
            {
                if (!userInfo.Permissions.TryGetValue(permissionType, out A))
                {
                    // Verifico uno o più ruoli globali
                    if (permissionType.StartsWith(":"))
                    {
                        var roles = permissionType.Substring(1).Split(',');
                        foreach (var roleId in roles)
                        {
                            if (userInfo.GlobalRoles.FirstOrDefault(r => r.Id.Equals(roleId, StringComparison.InvariantCultureIgnoreCase)) != null)
                            {
                                A = Domain.Enumerators.AuthorizationType.Granted;
                                break;
                            }
                        }
                    }
                    else
                    {
                        var aclService = context.HttpContext.RequestServices.GetRequiredService<IACLService>();
                        A = await aclService.GetAuthorization("$GLOBAL$", userInfo, permissionType);
                    }

                    userInfo.Permissions.TryAdd(permissionType, A);
                }
            }
            else
            {
                await context.HttpContext.ChallengeAsync();
                return;
            };
            if (A == Domain.Enumerators.AuthorizationType.Granted)
            {
                await next();
            }
            else
                context.Result = new ForbidResult();
        }

        public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
        {
            if (context == null || context.HttpContext == null) throw new ArgumentNullException(nameof(context));
            var A = Domain.Enumerators.AuthorizationType.Denied;

            var loggedUserService = context.HttpContext.RequestServices.GetRequiredService<ILoggedUserProfile>();
            var userInfo = loggedUserService.Get();
            if (userInfo != null)
            {
                if (!userInfo.Permissions.TryGetValue(permissionType, out A))
                {
                    if (permissionType.StartsWith(":"))
                    {
                        var roles = permissionType.Substring(1).Split(',');
                        foreach (var roleId in roles)
                        {
                            if (userInfo.GlobalRoles.FirstOrDefault(r => r.Id.Equals(roleId, StringComparison.InvariantCultureIgnoreCase)) != null)
                            {
                                A = Domain.Enumerators.AuthorizationType.Granted;
                                break;
                            }
                        }
                    }
                    else
                    {
                        var aclService = context.HttpContext.RequestServices.GetRequiredService<IACLService>();
                        A = await aclService.GetAuthorization("$GLOBAL$", userInfo, permissionType);
                    }
                    userInfo.Permissions.TryAdd(permissionType, A);
                }
            }
            else
            {
                await context.HttpContext.ChallengeAsync();
                return;
            };
            if (A == Domain.Enumerators.AuthorizationType.Granted)
            {
                await next();
            }
            else
                context.Result = new ForbidResult(); 
        }

        public async Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
        {

        }
    }
}
