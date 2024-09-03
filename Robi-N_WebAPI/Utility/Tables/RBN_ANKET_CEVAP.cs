using System.ComponentModel.DataAnnotations;

namespace Robi_N_WebAPI.Utility.Tables
{
    public class RBN_ANKET_CEVAP
    {
        [Key]
        public long Id { get; set; }
        public int AnketId { get; set; }
        public string? CallUniqID { get; set; }
        public string ?Result { get; set; }
        public DateTime add_date { get; set; }
        public bool active { get; set; }

    }
}
