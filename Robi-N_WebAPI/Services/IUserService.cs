namespace Robi_N_WebAPI.Services
{
    public interface IUserService
    {
        bool ValidateCredentials(string username, string password);
    }
}
