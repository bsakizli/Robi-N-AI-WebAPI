using Robi_N_WebAPI.Utility.Tables;

namespace Robi_N_WebAPI.Model.Response
{
	public class responseAutomaticTicketClosingList : GlobalResponse
	{
        public List<RBN_EMPTOR_AUTOTICKETCLOSEDScheduler>? data { get; set; }
    }

	public class responseSingleAutomaticTicketClosingList : GlobalResponse
	{
        public RBN_EMPTOR_AUTOTICKETCLOSEDScheduler ? data { get; set; }
    }


	public class responseAutomaticTicketList : GlobalResponse
	{
		public List<long>? data { get; set; }
	}

	

}
