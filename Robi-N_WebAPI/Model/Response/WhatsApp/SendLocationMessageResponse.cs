using WhatsAppBusinessAPI.WhatsAppBusinessModel;

namespace Robi_N_WebAPI.Model.Response.WhatsApp
{
    public class SendLocationMessageResponse : GlobalResponse
    {
        public SendLocationMessage.Root? result { get; set; }
    }
}
