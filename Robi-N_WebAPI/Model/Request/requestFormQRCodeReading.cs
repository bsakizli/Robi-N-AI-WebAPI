namespace Robi_N_WebAPI.Model.Request
{
    public class requestFormQRCodeReading
    {
        public IFormFile? ServiceFormPicture { get; set; }
        public long ticketId { get; set; }
        public long solutionDate { get; set; }
    }
}
