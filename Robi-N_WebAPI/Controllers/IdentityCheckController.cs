using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Nancy.Json;
using Newtonsoft.Json.Linq;
using Robi_N_WebAPI.Model;
using Robi_N_WebAPI.Model.Request;
using Robi_N_WebAPI.Model.Response;
using Robi_N_WebAPI.Utility;
using Robi_N_WebAPI.Utility.Tables;
using SimpleCrypto;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.DirectoryServices.Protocols;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Robi_N_WebAPI.Helper;
using System.Data;
using Microsoft.AspNetCore.Rewrite;
using Robi_N_WebAPI.Services;
using System.Web.Http.Filters;

namespace Robi_N_WebAPI.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class IdentityCheckController : ControllerBase
    {
        private readonly JwtSettings _JwtSettings;
        private readonly AIServiceDbContext _db;
        private readonly ILogger<IdentityCheckController> _logger;
        private readonly Program _program;
        public EmptorService _emptorService = new EmptorService();

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


        [AllowAnonymous]
        [HttpPost("emptorLogin")]
        public IActionResult emptorLogin(string username, string password)
        {
            responseEmptorLoginUser response;
            _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), username +" - " +password));
           
            try
            {
                
                if(!String.IsNullOrEmpty(username) && !String.IsNullOrEmpty(password))
                {
                    //Emptor Login Service
                    var _response = _emptorService.emptorLoginUserCheck(username, Helper.Helper.Base64Decode(password));

                    if (_response != null)
                    {
                        if (Convert.ToInt32(_response.ResultCode) == 0)
                        {
                            if (_JwtSettings.Key == null) throw new Exception("JWT key değeri null olamaz");

                            var secKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_JwtSettings.Key));
                            var credentials = new SigningCredentials(secKey, SecurityAlgorithms.HmacSha256);

                            var claimArray = new List<Claim>
                         {
                            new Claim(ClaimTypes.IsPersistent, _response.UserId),
                            new Claim(ClaimTypes.Email, _response.Email),
                            new Claim(ClaimTypes.Name, _response.FirstName),
                            new Claim(ClaimTypes.GivenName, _response.UserFullName),
                            new Claim(ClaimTypes.NameIdentifier, username!),
                            new Claim(ClaimTypes.GroupSid, _response.PositionName!),
                            //new Claim(ClaimTypes.Role, apiUsers.role!),
                         };

                            claimArray.Add(new Claim(ClaimTypes.Role, "User"!));

                            var token = new JwtSecurityToken(
                                _JwtSettings.Issuer,
                                _JwtSettings.Audience,
                                claimArray,
                                expires: DateTime.Now.AddMinutes(Convert.ToDouble(120)),
                                signingCredentials: credentials
                                );



                            response = new responseEmptorLoginUser
                            {
                                status = true,
                                statusCode = 200,
                                message = "Emptor Login Verified.",
                                User = _response,
                                token = new JwtSecurityTokenHandler().WriteToken(token),
                                expiredDate = token.ValidTo,
                                displayMessage = _response.ResultMessage
                            };
                            var globalResponseResult = new JavaScriptSerializer().Serialize(response);
                            _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), response));
                            return Ok(response);
                            //Token Vereceksin.
                        }
                        else
                        {
                            response = new responseEmptorLoginUser
                            {
                                status = false,
                                statusCode = 401,
                                displayMessage = _response.ResultMessage,
                                message = "Emptor Login Failed."
                            };
                            _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), response));
                            return BadRequest(response);
                            //Hata Mesajı
                        }
                    }
                    else
                    {
                        response = new responseEmptorLoginUser
                        {
                            status = false,
                            statusCode = 500,
                            displayMessage = "Inform the manager. There is a systemic problem.",
                            message = "Emptor Service Error"
                        };
                        _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), response));
                        return BadRequest(response);
                        //Servis Hatası
                    }

                } else
                {
                    response = new responseEmptorLoginUser
                    {
                        status = false,
                        statusCode = 199,
                        displayMessage = "Kullanıcı adı ve şifre alanı boş olamaz.",
                        message = "Username and Password Error"
                    };
                    _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), response));
                    return BadRequest(response);
                    //Kullanıcı Adı ve Şifre Boş Olamaz
                }

                

            } catch
            {
                response = new responseEmptorLoginUser
                {
                    status = false,
                    statusCode = 500,
                    displayMessage = "Inform the manager. There is a systemic problem.",
                    message = "Emptor Service Error"
                };
                _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), response));
                return BadRequest(response);
            }

        }



        [AllowAnonymous]
        [HttpPost("ldaplogin")]
        public IActionResult ldaplogin(string username, string password)
        {
            ldapAccessControl response = new ldapAccessControl();

            Dictionary<string, object> properties;
            string _path = string.Format("LDAP://{0}", "ADSLOCAL");
            string _filterAttribute;

            DirectoryEntry entry = new DirectoryEntry("LDAP://probil.intl", username, Helper.Helper.Base64Decode(password));
            properties = new Dictionary<string, object>();

            try
            {
                //Bind to the native AdsObject to force authentication.
                object obj = entry.NativeObject;

                if (obj != null)
                {
                    DirectorySearcher search = new DirectorySearcher(entry);

                    search.Filter = "(SAMAccountName=" + username + ")";
                    search.PropertiesToLoad.Add("cn");
                    search.PropertiesToLoad.Add("givenName");
                    search.PropertiesToLoad.Add("sn");
                    search.PropertiesToLoad.Add("sAMAccountName");

                    SearchResult result = search.FindOne();

                    if (result == null)
                    {
                        response = new ldapAccessControl
                        {
                            status = true,
                            statusCode = 404,
                            message = "LDAP Login Failed."
                        };
                        var globalResponseResultt = new JavaScriptSerializer().Serialize(response);
                        _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), globalResponseResultt));
                        return BadRequest(response);
                    }
                    else
                    {
                        if (result.Properties["sn"].Count != 0)
                            properties.Add("userName", result.Properties["sAMAccountName"][0]);
                           properties.Add("LastName", result.Properties["sn"][0]);
                        if (result.Properties["givenName"].Count != 0)
                            properties.Add("FirstName", result.Properties["givenName"][0]);
                    }

                    // Update the new path to the user in the directory.
                    _path = result.Path;
                    _filterAttribute = (string)result.Properties["cn"][0];

                    //string _fullName = properties.FirstOrDefault(item => "FirstName")?.Value;
                    var _fullName = properties.FirstOrDefault(pair => pair.Key == "FirstName").Value +" " + properties.FirstOrDefault(pair => pair.Key == "LastName").Value;
                    var _username = properties.FirstOrDefault(pair => pair.Key == "userName").Value.ToString();

                    if (_JwtSettings.Key == null) throw new Exception("JWT key değeri null olamaz");

                    var secKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_JwtSettings.Key));
                    var credentials = new SigningCredentials(secKey, SecurityAlgorithms.HmacSha256);

                    var claimArray = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, _username!),
                        new Claim(ClaimTypes.GivenName, _filterAttribute!),
                        new Claim(ClaimTypes.Name, _fullName!),
                        //new Claim(ClaimTypes.Role, apiUsers.role!),
                    };

                    claimArray.Add(new Claim(ClaimTypes.Role, "User"!));

                    var token = new JwtSecurityToken(
                        _JwtSettings.Issuer,
                        _JwtSettings.Audience,
                        claimArray,
                        expires: DateTime.Now.AddMinutes(Convert.ToDouble(120)),
                        signingCredentials: credentials
                        );

                    response = new ldapAccessControl
                    {
                        status = true,
                        statusCode = 200,
                        message = "LDAP verification is done and login is done.",
                        displayName = _filterAttribute,
                        userName = properties.FirstOrDefault(pair => pair.Key == "userName").Value.ToString(),
                        fullName = _fullName,
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiredDate = token.ValidTo
                    };
                    var globalResponseResult = new JavaScriptSerializer().Serialize(response);
                    _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), globalResponseResult));
                    return Ok(response);

                }
                else
                {
                    response = new ldapAccessControl
                    {
                        status = true,
                        statusCode = 404,
                        message = "LDAP Login Failed."
                    };
                    var globalResponseResult = new JavaScriptSerializer().Serialize(response);
                    _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), globalResponseResult));
                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                response = new ldapAccessControl
                {
                    status = true,
                    statusCode = 500,
                    message = String.Format(@"LDAP System Error. - {0}",ex.Message)
                };
                var globalResponseResult = new JavaScriptSerializer().Serialize(response);
                _logger.LogInformation(String.Format(@"Controller: {0} - Method: {1} - Response: {2}", this.ControllerContext?.RouteData?.Values["controller"]?.ToString(), this.ControllerContext?.RouteData?.Values["action"]?.ToString(), globalResponseResult));
                return BadRequest(response);
            }

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
