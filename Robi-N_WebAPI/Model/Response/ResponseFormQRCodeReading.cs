namespace Robi_N_WebAPI.Model.Response
{
    public class ResponseFormQRCodeReading :GlobalResponse
    {
        public string resultText { get; set; }
        public responseFormQRCodeReadingQRResponse QrResult { get; set; }
    }

    public class responseFormQRCodeReadingQRResponse
    {
        public int Ticket { get; set; }
        public int Solution { get; set; }
        public int Activity { get; set; }
        public int Create { get; set; }
        public int Start { get; set; }
        public int End { get; set; }
    }
}
