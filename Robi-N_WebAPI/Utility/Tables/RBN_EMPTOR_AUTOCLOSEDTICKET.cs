using System.ComponentModel.DataAnnotations;

namespace Robi_N_WebAPI.Utility.Tables
{
	public class RBN_EMPTOR_AUTOCLOSEDTICKET
	{
        [Key]
        public int Id { get; set; }
        public int AutoClosedId { get; set; }
        public long TicketId { get; set; }
        public DateTime closedDate { get; set; }
    }
}
