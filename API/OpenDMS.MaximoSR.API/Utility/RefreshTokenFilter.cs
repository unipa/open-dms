using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OpenDMS.MaximoSR.API.Utility.Interfacce;

namespace OpenDMS.MaximoSR.API.Utility
{
    public class RefreshTokenFilter : IAuthorizationFilter
    {
        private IKeycloakTokenHandler _tokenHandler;

        public RefreshTokenFilter(IKeycloakTokenHandler tokenHandler)
        {
            _tokenHandler = tokenHandler;
        }

        public async void OnAuthorization(AuthorizationFilterContext context)
        {
            try
            {
                await _tokenHandler.IsLoginStillValidAsync();
            }
            catch (Exception)
            {
                context.Result = new RedirectToRouteResult(
                        new RouteValueDictionary(new { controller = "Home", action = "SessionExpired" })
                    );
            }
        }
    }
}
