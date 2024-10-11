
namespace AdminShell
{
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Microsoft.Extensions.Primitives;
    using System;
    using System.Linq;
    using System.Net.Http.Headers;
    using System.Security.Claims;
    using System.Text;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;

    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder)
            : base(options, logger, encoder)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (StringValues.IsNullOrEmpty(Request.Headers["Authorization"]))
            {
                return Task.FromResult(AuthenticateResult.Fail($"Authentication failed: Authentication header missing in request!"));
            }

            AuthenticationHeaderValue authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
            string[] credentials = Encoding.UTF8.GetString(Convert.FromBase64String(authHeader.Parameter)).Split(':');
            string username = credentials.FirstOrDefault();
            string password = credentials.LastOrDefault();

            if (!ValidateCredentials(username, password))
            {
                return Task.FromResult(AuthenticateResult.Fail($"Authentication failed: Invalid credentials!"));
            }
            
            Claim[] claims = new[] {
                new Claim(ClaimTypes.Name, username)
            };

            ClaimsIdentity identity = new ClaimsIdentity(claims, Scheme.Name);
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);
            AuthenticationTicket ticket = new AuthenticationTicket(principal, Scheme.Name);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }

        public bool ValidateCredentials(string username, string password)
        {
#if DEBUG
            return true;
#else
            string passwordFromEnvironment = Environment.GetEnvironmentVariable("ServicePassword");
            if (string.IsNullOrEmpty(passwordFromEnvironment))
            {
                return false;
            }
            else
            {
                return username.Equals("admin") && password.Equals(passwordFromEnvironment);
            }
#endif
        }
    }
}