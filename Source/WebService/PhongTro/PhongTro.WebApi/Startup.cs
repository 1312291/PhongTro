using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using Owin;
using PhongTro.Domain.Infracstucture;
using PhongTro.Domain.Migrations;
using PhongTro.WebApi.Providers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net.Http.Formatting;
using System.Reflection;
using System.Web;
using System.Web.Http;

namespace PhongTro.WebApi
{
    /// <summary>
    /// This is an Owin startup class which will be fired once the server start.
    /// </summary>
    public class Startup
    {
        #region Constants
        const string KeyAudienceID = "as:AudienceId";
        const string KeyAudienceSecret = "as:AudienceSecret";
        const string KeyTokenIssuer = "tokenIssuer";
        const string KeyTokenGenPath = "tokenGenPath";
        const int TokenExpireTime = 1;
        #endregion

        /// <summary>
        /// Method config the Owin server
        /// </summary>
        /// <param name="app">Param supplied by the host at run-time</param>
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration httpConfig = new HttpConfiguration();
            
            ConfigureOAuthTokenGeneration(app);
            ConfigureOAuthTokenConsumption(app);

            ConfigureWebApi(httpConfig);

            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            app.UseWebApi(httpConfig);
        }

        /// <summary>
        /// Method configures OAuth token generations
        /// </summary>
        /// <param name="app">Param will be supplied by the host at run-time</param>
        private void ConfigureOAuthTokenGeneration(IAppBuilder app)
        {
            // Configure the db context and user manager to use a single instance per request
            app.CreatePerOwinContext(PhongTroDbContext.Create);
            app.CreatePerOwinContext<PhongTroUserManager>(PhongTroUserManager.Create);
            app.CreatePerOwinContext<PhongTroRoleManager>(PhongTroRoleManager.Create);

            // Add options to the OAuth server
            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString(ConfigurationManager.AppSettings[KeyTokenGenPath]),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(TokenExpireTime),

                // config to use the custom provider
                Provider = new CustomOAuthProvider(),

                // config to use the custom Jwt format
                AccessTokenFormat = new CustomJwtFormat(ConfigurationManager.AppSettings[KeyTokenIssuer]) 
            };

            // OAuth 2.0 Bearer Access Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
        }
        
        private void ConfigureWebApi(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }

        /// <summary>
        /// Method configures the server in order to enable it to consume the Jwt
        /// </summary>
        /// <param name="app">The AppBuilder param supplied by host at run-time</param>
        private void ConfigureOAuthTokenConsumption(IAppBuilder app)
        {
            var issuer = ConfigurationManager.AppSettings[KeyTokenIssuer];
            string audienceId = ConfigurationManager.AppSettings[KeyAudienceID];
            byte[] audienceSecret = TextEncodings.Base64Url.Decode(ConfigurationManager.AppSettings[KeyAudienceSecret]);

            // provide values for audience, audience secret so that server can extract the token
            app.UseJwtBearerAuthentication(
                new JwtBearerAuthenticationOptions
                {
                    AuthenticationMode = AuthenticationMode.Active,
                    AllowedAudiences = new[] { audienceId },
                    IssuerSecurityTokenProviders = new IIssuerSecurityTokenProvider[]
                    {
                        new SymmetricKeyIssuerSecurityTokenProvider(issuer, audienceSecret)
                    }
                });
        }
    }
}