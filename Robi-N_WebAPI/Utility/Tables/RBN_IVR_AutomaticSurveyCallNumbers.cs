using System.ComponentModel.DataAnnotations;

namespace Robi_N_WebAPI.Utility.Tables
{
    public class RBN_IVR_AutomaticSurveyCallNumbers
    {
        [Key]
        public int id { get; set; }
        public bool SurveyCheck { get; set; }
        public int CallId { get; set; }
        public long number { get; set; }
        public DateTime add_date { get; set; }
        public int process { get; set; }
        public bool active { get; set; }
        public DateTime CallDate { get; set; }

    }
}
