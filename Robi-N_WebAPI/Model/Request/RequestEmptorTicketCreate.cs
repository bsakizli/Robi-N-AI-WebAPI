namespace Robi_N_WebAPI.Model.Request
{
    public class RequestEmptorTicketCreate
    {
        public int companyId { get; set; }
        public string companyName { get; set; }
        public string fistName { get; set; }
        public string lastName { get; set; }
        public string gsmPhone { get; set; }
        public string email { get; set; }
        public string notes { get; set; }
    }
}
