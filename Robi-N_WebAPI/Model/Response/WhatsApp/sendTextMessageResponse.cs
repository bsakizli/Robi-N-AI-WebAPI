using System.Text.Json.Nodes;
using WhatsAppBusinessAPI.WhatsAppBusinessModel;

namespace Robi_N_WebAPI.Model.Response.WhatsApp
{
    public class sendTextMessageResponse : GlobalResponse
    {
        public JsonNode? result { get; set; }
    }
}
