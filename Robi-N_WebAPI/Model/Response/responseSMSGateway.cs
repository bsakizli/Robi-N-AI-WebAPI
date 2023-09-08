using Robi_N_WebAPI.Utility.Tables;

namespace Robi_N_WebAPI.Model.Response
{
    public class responseSMSGateway : GlobalResponse
    {
        public class responsesendSMSById : GlobalResponse {
            public RBN_SMS_TEMPLATES data { get; set; }
        }
    }
}
