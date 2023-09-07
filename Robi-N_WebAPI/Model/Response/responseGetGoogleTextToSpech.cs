namespace Robi_N_WebAPI.Model.Response
{
    public class responseGetGoogleTextToSpech : GlobalResponse
    {
        public int soundId { get; set; }
        public string? soundUrl { get; set; }
        public string? base64SoundContent { get; set; }
    }
}
