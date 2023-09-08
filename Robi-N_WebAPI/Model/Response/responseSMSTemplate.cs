using Robi_N_WebAPI.Utility.Tables;

namespace Robi_N_WebAPI.Model.Response
{
    public class responseSMSTemplate : GlobalResponse
    {
        public class getSMSTemplate : GlobalResponse
        {
            public RBN_SMS_TEMPLATES? data { get; set; }
        }


        public class getSMSTemplateList : GlobalResponse
        {
            public List<RBN_SMS_TEMPLATES>? data { get; set; }
        }

    }
}
