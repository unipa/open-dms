using Google.Protobuf.WellKnownTypes;
using MessageBus.Interface;
using MessageBus.RabbitMQ;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using OpenDMS.Domain.Constants;
using OpenDMS.MultiTenancy.Interfaces;
using OpenDMS.Startup;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;


public static class AuthenticationExtensions
{
    public static IServiceCollection AddExternalIdentity(this IServiceCollection Services, IConfiguration config, bool IsProduction, bool IsAPI = true)
    {
        Services.AddDistributedMemoryCache((option) => { option.ExpirationScanFrequency = TimeSpan.FromSeconds(30); });

        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        Services.Configure<CookiePolicyOptions>(options =>
        {
            // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            options.CheckConsentNeeded = context => true;
            options.MinimumSameSitePolicy = SameSiteMode.Lax;
            //options.Secure = Microsoft.AspNetCore.Http.CookieSecurePolicy.Always;

        });
        var A = Services.AddAuthentication(options =>
        {
            // Store the session to cookies
            //            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            if (IsAPI)
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                //options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }
            else
            {
                options.DefaultAuthenticateScheme = OpenIdConnectDefaults.AuthenticationScheme;
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;

            }
        });
        A = A.AddCookie();
        if (IsAPI)
            A = A.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
        {
            options.Authority = $"{config["Keycloak:auth-server-url"]}realms/{config["Keycloak:realm"]}";
            options.RequireHttpsMetadata = IsProduction;
            //            if (!IsProduction)
            options.BackchannelHttpHandler = new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = (m, c, s, o) => { return true; },
                UseDefaultCredentials = true,
            };
            options.TokenValidationParameters = new TokenValidationParameters
            {
                NameClaimType = "preferred_username",
                RoleClaimType = "roles"
            };

            //Services.AddSession(options =>
            //{
            //    options.IdleTimeout = TimeSpan.FromSeconds(30);
            //    options.Cookie.HttpOnly = true;
            //    options.Cookie.IsEssential = true;
            //});

            options.Events = new JwtBearerEvents()
            {
                //    //OnMessageReceived = async context =>
                //    //{
                //    //    var c = new ClaimsIdentity(context.Principal.Claims, "AuthenticationTypes.Federation", "name", "roles");
                //    //    context.Principal = new ClaimsPrincipal();
                //    //    //.Principal..AddIdentity(c);
                //    //    context.Principal.AddIdentity(c);
                //    //},
                //    OnTokenValidated = async context =>
                //    {
                //        var c = new ClaimsIdentity(context.Principal.Claims, "AuthenticationTypes.Federation", "name", "roles");
                //        context.Principal = new ClaimsPrincipal();
                //        //.Principal..AddIdentity(c);
                //        context.Principal.AddIdentity(c);

                //    }
            };
            options.Audience = "account";
            options.MetadataAddress = $"{config["Keycloak:auth-server-url"]}realms/{config["Keycloak:realm"]}/.well-known/openid-configuration";
        });
        if (!IsAPI)
            A = A.AddOpenIdConnect(options =>
            {
                // URL of the Keycloak server
                options.Authority = $"{config["Keycloak:auth-server-url"]}realms/{config["Keycloak:realm"]}";
                // Client configured in the Keycloak
                options.ClientId = config["Keycloak:resource"];
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "preferred_username",
                    RoleClaimType = "roles"
                };
                options.Events = new OpenIdConnectEvents
                {
                    OnRemoteFailure = async (context) =>
                    {
                        context.Response.Redirect("/Error");
                    }
                };
                // For testing we disable https (should be true for production)
                options.RequireHttpsMetadata = IsProduction;
                //if (!IsProduction)
                    options.BackchannelHttpHandler = new HttpClientHandler()
                    {
                        ServerCertificateCustomValidationCallback = (m, c, s, o) => { return true; },
                        UseDefaultCredentials = true,
                    };
                options.SaveTokens = true;
                // Client secret shared with Keycloak
                options.ClientSecret = config["Keycloak:Credentials:secret"];
                options.CallbackPath = "/keycloak";
                options.GetClaimsFromUserInfoEndpoint = true;
                //options.ClaimActions.MapUniqueJsonKey(System.Security.Claims.ClaimTypes.Name,
                //                                      "preferred_username");
                //options.ClaimActions.MapUniqueJsonKey("gender", "gender");
                // OpenID flow to use
                options.ResponseType = OpenIdConnectResponseType.CodeIdToken;
            });
        //Services.AddTransient<IClaimsTransformation, ClaimsTransformation>();
        Services.AddTransient<IClaimsTransformation, ClaimsTransformation>();

        Console.WriteLine("ConnectionString : " + config["ConnectionString"]);
        Console.WriteLine("Ambiente : " + (IsProduction ? "PRODUCTION" : "DEVELOPMENT"));


        FieldInfo[] Fields = typeof(OpenDMS.Domain.Constants.StaticConfiguration).GetFields(BindingFlags.Public | BindingFlags.Static);
        foreach (var F in Fields)
        {
            var Value = (string)F.GetRawConstantValue();
            if (Value != null)
            {
                if (config[Value] == null)
                    Console.WriteLine("## - Setting: " + Value);
                else
                    Console.WriteLine("OK - Setting: " + Value+" = " + config[Value].ToString());
            }
        }
        var messageBus_URL = "";
        var messageBus_Type = "";
        try
        {
            messageBus_Type = config[StaticConfiguration.CONST_MESSAGEBUS_TYPE];
            messageBus_URL = config[StaticConfiguration.CONST_MESSAGEBUS_URL];
            if (!string.IsNullOrEmpty(messageBus_URL) && string.IsNullOrEmpty(messageBus_Type)) messageBus_Type = "rabbitmq";
        }
        catch (Exception)
        {
        }
        Console.WriteLine("MessageBus: " + messageBus_Type);
        switch (messageBus_Type)
        {
            case "rabbitmq":
                Services.AddTransient<IMessaging<string>>(s => new RabbitMQMessageBus<string>(messageBus_URL));
                break;
            default:
                Services.AddTransient<IMessaging<string>>(s => new FakeMessaging<string>());
                break;
        }

        return Services;
    }
}
