using Microsoft.Kiota.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace Robi_N_WebAPI.Utility.Tables
{
	public class RNB_IVR_WORKING_HOURS
	{
        [Key]
        public int id { get; set; }
        public string? csqname { get; set; }
        public TimeSpan? starthours { get; set; }
        public TimeSpan? endhours { get; set; }
        public bool? active { get; set; }
        public bool? weekend { get; set; }
    }
}
