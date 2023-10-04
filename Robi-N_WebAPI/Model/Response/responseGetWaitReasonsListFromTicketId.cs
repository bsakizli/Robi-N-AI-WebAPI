using EmptorUtility.Models.Response;

namespace Robi_N_WebAPI.Model.Response
{
    public class responseGetWaitReasonsListFromTicketId : GlobalResponse
    {
        public List<r_GetWaitReasonsListFromTicketId>? data { get; set; }
        public r_getCompanyFullName? company { get; set; }
    }
}
