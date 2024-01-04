namespace Robi_N_WebAPI.Utility.Tables
{
    public class RBN_EMPTOR_AUTOTICKETCLOSEDScheduler
    {
        public int Id { get; set; }
        public string? name { get; set; }
        public string? cron { get; set; }
        public int? process { get; set; }
        //public string? oneClosedTicketCount { get; set; }
        public string? ticketQuery { get; set; }
        public DateTime? lastStartDate { get; set; }
        public DateTime? registerDate { get; set; }
        public bool? active { get; set; }
    }
}
