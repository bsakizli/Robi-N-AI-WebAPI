using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Robi_N_WebAPI.Utility.Tables
{
    public class RBN_IVR_SurveyQuestions
    {
        [Key]
        public long id { get; set; }
        public long SurveyId { get; set; }
        public long QuestionID { get; set; }
        public string? Question { get; set; }
        public DateTime? add_date { get; set; }
        public bool? active { get; set; }
    }
}
