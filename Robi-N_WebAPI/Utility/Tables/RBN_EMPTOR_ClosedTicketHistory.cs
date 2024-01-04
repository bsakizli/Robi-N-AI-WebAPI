using System.ComponentModel.DataAnnotations;

namespace Robi_N_WebAPI.Utility.Tables
{
    public class RBN_EMPTOR_ClosedTicketHistory
    {
        [Key]
        public int Id { get; set; }

		public int autoTicketId { get; set; }
		public long  TicketId { get; set; }
        public string?  TicketIdDesc { get; set; }
        public DateTime closedDate { get; set; }
        public DateTime addDate { get; set; }
        public bool active { get; set; }
    }
}
