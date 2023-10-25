using System.ComponentModel.DataAnnotations;

namespace Robi_N_WebAPI.Utility.Tables
{
    public class RBN_EMPTOR_WaitingTicketHistory
    {
        [Key]
        public int Id { get; set; }
        public int TicketId { get; set; }
        public string? TicketDesc { get; set; }
        public int? CompanyId { get; set; }
        public string? CompanyName { get; set; }
        public int? MainUserId { get; set; }
        public string? MainUserFullName { get; set; }
        public int? SubUserId { get; set; }
        public string? SubUserFullName { get; set; }
        public int? WaitingReason { get; set; }
        public string? WaitingReasonLabel { get; set; }
        public DateTime? WaitingDate { get; set; }
        public DateTime? TransactionDate { get; set; }
        public bool? Active { get; set; }
    }
}
