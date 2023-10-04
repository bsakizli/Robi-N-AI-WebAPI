using System.ComponentModel.DataAnnotations;

namespace Robi_N_WebAPI.Utility.Tables
{
    public class RBN_WAITING_TIMES
    {
        [Key]
        public int Id { get; set; }
        public int EmptorTicketWaitingReasonId { get; set; }
        public int WaitingTimeDay { get; set; }
        public Boolean Overtime { get; set; }
        public DateTime add_date { get; set; }
        public DateTime update_date { get; set; }
        public Boolean Active { get; set; }
    }
}
