using static Robi_N_WebAPI.Services.UserService;

namespace Robi_N_WebAPI.Services
{
    public interface IUserService
    {
        ValidateCredentialsResult ValidateCredentials(string username, string password);
    }
}
