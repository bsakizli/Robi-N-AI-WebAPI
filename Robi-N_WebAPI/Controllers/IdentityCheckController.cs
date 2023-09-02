using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using Robi_N_WebAPI.Model;
using Robi_N_WebAPI.Model.Request;
using Robi_N_WebAPI.Model.Response;
using Robi_N_WebAPI.Utility;
using Robi_N_WebAPI.Utility.Tables;
using SimpleCrypto;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Robi_N_WebAPI.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class IdentityCheckController : ControllerBase
    {
        private readonly JwtSettings _JwtSettings;
        private readonly AIServiceDbContext _db;
        private readonly ILogger<IdentityCheckController> _logger;

        PBKDF2 crypto = new PBKDF2();

        public IdentityCheckController(ILogger<IdentityCheckController> logger,AIServiceDbContext db, IOptions<JwtSettings> JwtSettings)
        {
            _logger = logger;
            _db = db;
            _JwtSettings = JwtSettings.Value;
        }

        [AllowAnonymous]
        //[HttpPost]
        [HttpPost("token")]
        public IActionResult Login([FromBody] requestTokenCreation apiUsers)
        {
            var _apiUsers = IdentityCheckDo(apiUsers);
            if (_apiUsers == null) {

                GlobalResponse response = new GlobalResponse
                {
                    statusCode = 404,
                    status = true,
                    message = "No such user exists. Contact your service manager."
                };

                return NotFound(response);
            }

            var _getRoles = GetRoles(_apiUsers);
            if (_getRoles == null || _getRoles.Count == 0)
            {
                GlobalResponse response = new GlobalResponse
                {
                    statusCode = 404,
                    status = true,
                    message = "The user is not assigned to an active role."
                };
                return NotFound(response);
            }

            var _CreateToken = CreateToken(_apiUsers, _getRoles);

            return Ok(_CreateToken);
        }

        private responseCreateToken CreateToken(ApiUsers apiUsers, List<RBN_AI_SERVICE_ROLE> roles)
        {
            responseCreateToken responseCreateToken;

            if (_JwtSettings.Key == null) throw new Exception("JWT key değeri null olamaz");

            var secKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_JwtSettings.Key));
            var credentials = new SigningCredentials(secKey, SecurityAlgorithms.HmacSha256);

            var claimArray = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, apiUsers.username!),
                //new Claim(ClaimTypes.Role, apiUsers.role!),
            };

            //var userRoles = GetRoles(apiUsers);
            //if (userRoles != null)  throw new Exception("JWT key değeri null olamaz");
            foreach (var _role in roles)
            {
                claimArray.Add(new Claim(ClaimTypes.Role, _role.RoleName!));
                //claims.add(new claim(claimtypes.role, userrole));
            }

            var token = new JwtSecurityToken(
                _JwtSettings.Issuer,
                _JwtSettings.Audience,
                claimArray,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(apiUsers.expirationTime)),
                signingCredentials: credentials
                );

            var tt = DateTime.Now;

            responseCreateToken = new responseCreateToken
            {
                expiredDate = token.ValidTo,
                token = new JwtSecurityTokenHandler().WriteToken(token)
        };

            return responseCreateToken;


        }

        private ApiUsers? IdentityCheckDo(requestTokenCreation apiUsers)
        {
            var _serviceUser = _db.RBN_AI_SERVICE_USERS.Where(x => x.active == true && x.username == apiUsers.username).FirstOrDefault();

            if (_serviceUser == null) return null;

            if (_serviceUser.password == crypto.Compute(apiUsers.password,_serviceUser.salt))
            {
                return _serviceUser;
            }

            return null;
              
           // return _db.RBN_AI_SERVICE_USERS.FirstOrDefault(x => x.active == true && x.username.ToLower() == apiUsers.username.ToLower() && x.password == crypto.Compute());

            //return StaticApiUsers
            //    .Users
            //    .FirstOrDefault(x =>
            //    x.username?.ToLower() == apiUsers.username
            //    && x.password == apiUsers.password
            //    );
                
        }

        private List<RBN_AI_SERVICE_ROLE> GetRoles(ApiUsers apiUsers)
        {
            var _roleList = _db.RBN_AI_SERVICE_ROLES_MAP.Where(x => x.active == true && x.UserId == apiUsers.Id).ToList();
            List<RBN_AI_SERVICE_ROLE> _RoleList = new List<RBN_AI_SERVICE_ROLE>();
            foreach (var item in _roleList)
            {
                var _role = _db.RBN_AI_SERVICE_ROLE.FirstOrDefault(x => x.Id == item.RoleId);
                if (_role == null) continue;
                _RoleList.Add(_role);
            }
            return _RoleList;
        }
    }
}
