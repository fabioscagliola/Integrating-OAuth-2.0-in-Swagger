using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace com.fabioscagliola.OAuthSwagger.WebApiTest
{
    public class WebApiTestAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public static string AuthenticationScheme { get { return "TestScheme"; } }

        public WebApiTestAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            Claim[] claims = new[] { new Claim(ClaimTypes.Name, "Fabio"), };
            ClaimsIdentity claimsIdentity = new(claims, AuthenticationScheme);
            ClaimsPrincipal claimsPrincipal = new(claimsIdentity);
            AuthenticationTicket authenticationTicket = new(claimsPrincipal, AuthenticationScheme);
            AuthenticateResult authenticateResult = AuthenticateResult.Success(authenticationTicket);
            return Task.FromResult(authenticateResult);
        }
    }
}
