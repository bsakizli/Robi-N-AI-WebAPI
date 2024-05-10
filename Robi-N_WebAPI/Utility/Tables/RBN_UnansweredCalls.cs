using System.ComponentModel.DataAnnotations;

namespace Robi_N_WebAPI.Utility.Tables
{
    public class RBN_UnansweredCalls
    {
        [Key] 
        public int id { get; set; }
        public long contactid { get; set; }
        public long agentid { get; set; }
        public long calltype { get; set; }
        public string? phonenumber { get; set; }
        public int disposition { get; set; }
        public string? csqname { get; set; }
        public DateTime? startdatetime { get; set; }
        public DateTime? enddatetime { get; set; }
        public DateTime? record_date { get; set; }
        public DateTime? smsSendDate { get; set; }
        public bool smsSendStatus { get; set; }
        public long smsSendId { get; set; }
        public bool active { get; set; }
        public int process { get; set; }
    }
}
