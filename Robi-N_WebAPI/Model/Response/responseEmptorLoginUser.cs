namespace Robi_N_WebAPI.Model.Response
{
    public class responseEmptorLoginUser : GlobalResponse
    {
        public string? token { get; set; }
        public DateTime? expiredDate { get; set; }
        public responseEmptorServiceUserLogin? User { get; set; }
    }
}
