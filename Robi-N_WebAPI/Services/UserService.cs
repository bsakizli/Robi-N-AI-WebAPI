using Microsoft.Extensions.Options;
using Robi_N_WebAPI.Migrations;
using Robi_N_WebAPI.Model;
using Robi_N_WebAPI.Utility;
using Robi_N_WebAPI.Utility.Tables;
using SimpleCrypto;

namespace Robi_N_WebAPI.Services
{
    public class UserService : IUserService
    {

        private readonly AIServiceDbContext? _db;
        PBKDF2 crypto = new PBKDF2();

        public UserService(AIServiceDbContext db)
        {
            _db = db;
        }

        public class ValidateCredentialsResult : GlobalResponse
        {

            public List<RBN_AI_SERVICE_ROLE>? roles { get; set; }
            public ApiUsers? user { get; set; }
        }

        public ValidateCredentialsResult ValidateCredentials(string _username, string password)
        {
            ValidateCredentialsResult _respose;

            try
            {
                var _serviceUser = _db.RBN_AI_SERVICE_USERS.Where(x => x.active == true && x.username == _username).FirstOrDefault();

                if (_serviceUser == null) return _respose = new ValidateCredentialsResult { status = false };

                if (_serviceUser.password == crypto.Compute(password, _serviceUser.salt))
                {


                    var _roleMap = _db.RBN_AI_SERVICE_ROLES_MAP.Where(x => x.UserId == _serviceUser.Id && x.active == true).ToList();

                    List<RBN_AI_SERVICE_ROLE> _roles = new List<RBN_AI_SERVICE_ROLE>();
                    foreach (var item in _roleMap)
                    {
                        var _role = _db.RBN_AI_SERVICE_ROLE.Where(x => x.active == true && x.Id == item.RoleId).FirstOrDefault();
                        if (_role != null)
                        {
                            _roles.Add(_role);
                        }
                    }

                    if (_roles.Count > 0)
                    {
                        _respose = new ValidateCredentialsResult
                        {
                            status = true,
                            statusCode = 200,
                            roles = _roles,
                            user = _serviceUser,
                            displayMessage = "Kullanıcı doğrulaması yapıldı, role tanımı ilişkilendirildi."
                        };

                    }
                    else
                    {
                        _respose = new ValidateCredentialsResult
                        {
                            status = false,
                            statusCode = 404,
                            displayMessage = "Rol tanımı olmadığı için kullanıcı giriş yapamaz."
                        };
                    }

                    return _respose;
                }
                else
                {
                    _respose = new ValidateCredentialsResult
                    {
                        status = false,
                    };
                    return _respose;
                }



            }
            catch (Exception? ex)
            {
                _respose = new ValidateCredentialsResult
                {
                    status = false,
                };
                return _respose;
            }
        }

       
    }
}
