using Microsoft.Extensions.Options;
using Robi_N_WebAPI.Model;
using Robi_N_WebAPI.Utility;
using SimpleCrypto;

namespace Robi_N_WebAPI.Services
{
    public class UserService : IUserService
    {

        private readonly AIServiceDbContext ?_db;
        PBKDF2 crypto = new PBKDF2();

        public UserService(AIServiceDbContext db)
        {
            _db = db;
        }

        public bool ValidateCredentials(string _username, string password)
        {

            try
            {
                var _serviceUser = _db.RBN_AI_SERVICE_USERS.Where(x => x.active == true && x.username == _username).FirstOrDefault();

                if (_serviceUser == null) return false;

                if (_serviceUser.password == crypto.Compute(password, _serviceUser.salt)) {
                    return true;
                } else
                {
                    return false;
                }
            } catch (Exception? ex)
            {
                return false;
            }
        }
    }
}
