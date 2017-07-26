using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;

namespace Fiver.Security.AuthServer.Client
{
    public class Startup
    {
        public void ConfigureServices(
            IServiceCollection services)
        {
            services.AddMvc();
        }

        public void Configure(
            IApplicationBuilder app, 
            IHostingEnvironment env, 
            ILoggerFactory loggerFactory)
        {
            app.UseDeveloperExceptionPage();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationScheme = "Cookies"
            });

            app.UseOpenIdConnectAuthentication(new OpenIdConnectOptions
            {
                AuthenticationScheme = "oidc",
                SignInScheme = "Cookies", // cookie middle setup above

                Authority = "http://localhost:5000", // Auth Server
                RequireHttpsMetadata = false,

                ClientId = "fiver_auth_client", // client setup in Auth Server
                ClientSecret = "secret",

                ResponseType = "code id_token", // means Hybrid flow (id + access token)
                Scope = { "fiver_auth_api", "offline_access" },

                GetClaimsFromUserInfoEndpoint = true,

                SaveTokens = true,
            });

            app.UseMvcWithDefaultRoute();
        }
    }
}
