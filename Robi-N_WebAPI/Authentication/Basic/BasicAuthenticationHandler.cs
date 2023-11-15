using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text;
using Robi_N_WebAPI.Services;
using ExtendedXmlSerializer;

namespace Robi_N_WebAPI.Authentication.basic
{
    public class basicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        #region Property
        readonly IUserService _userService;
        #endregion

        #region Constructor
        public basicAuthenticationHandler(IUserService userService,
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
            _userService = userService;
        }
        #endregion

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string username = null;
            
            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(authHeader.Parameter)).Split(':');
                username = credentials.FirstOrDefault();
                var password = credentials.LastOrDefault();

                 var _result = _userService.ValidateCredentials(username, password);
                
                if (!_result.status)
                    throw new ArgumentException("Invalid credentials");


                var claimArray = new List<Claim>
                             {
                                new Claim("UserId", _result.user!.Id.ToString()),
                                new Claim("Name", _result.user.username!),
                                new Claim("UserFullName", _result.user.username!),
                                new Claim("username", _result.user.username!)
                             };

                foreach (var item in _result.roles)
                {
                    claimArray.Add(new Claim(ClaimTypes.Role, item.RoleName!));
                }

                var identity = new ClaimsIdentity(claimArray, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return AuthenticateResult.Success(ticket);


            }
            catch (Exception ex)
            {
                return AuthenticateResult.Fail($"Authentication failed: {ex.Message}");
            }

            var claims = new[] {
                new Claim(ClaimTypes.Name, username)
            };



           
        }

    }
}
