using System.ComponentModel.DataAnnotations;

namespace Robi_N_WebAPI.Utility.Tables
{
    public class RBN_IVR_AutomaticSurveyAnswers
    {
        [Key]
        public long id { get; set; }
        public int CallId { get; set; }
        public long SurveyId { get; set; }
        public long QuestionID { get; set; }
        public int AnswerKeying { get; set; }
        public long arayan_no { get; set; }
        public long aranan_no { get; set; }
        public long santral_no { get; set; }
        public DateTime add_date { get; set; }
        public bool active { get; set; }
    }
}
