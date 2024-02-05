using WhatsAppBusinessAPI.WhatsAppBusinessModel;

namespace Robi_N_WebAPI.Model.Response.WhatsApp
{
    public class sendTextMessageResponse : GlobalResponse
    {
        public SendTextMessage.Root? result { get; set; }
    }
}
