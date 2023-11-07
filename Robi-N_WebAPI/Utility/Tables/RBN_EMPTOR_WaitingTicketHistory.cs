using System.ComponentModel.DataAnnotations;

namespace Robi_N_WebAPI.Utility.Tables
{
    public class RBN_EMPTOR_WaitingTicketHistory
    {
        [Key]
        public int Id { get; set; }
        public int TicketId { get; set; }
        public string? TicketIdDesc { get; set; }
        public int MainUserId { get; set; }
        public int UserId { get; set; }
        public int WaitingReasonId { get; set; }
        //public string? TicketDesc { get; set; }
        //public int? CompanyId { get; set; }
        //public string? CompanyName { get; set; }
        //public int? MainUserId { get; set; }
        //public string? MainUserFullName { get; set; }
        //public string? OrganizationName { get; set; }
        //public string? TicketSubStatusName { get; set; }
        //public string? WaitingReasonName { get; set; }
        public DateTime? CallBackDate { get; set; }
        public DateTime? TransactionDate { get; set; }
        public bool? Active { get; set; }
    }
}
