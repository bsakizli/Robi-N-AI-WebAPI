using System.Security.Permissions;

namespace Robi_N_WebAPI.Model
{
    public class StaticApiUsers
    {
        public static List<ApiUsers> Users = new List<ApiUsers>()
        {
            new ApiUsers { Id = 1, username = "demo", password = "demo", role="demo" },
          
        };
    }
}
