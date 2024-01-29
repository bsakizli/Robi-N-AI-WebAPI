using System.ComponentModel.DataAnnotations;

namespace Robi_N_WebAPI.Utility.Tables
{
    public class RBN_SGK_VisitingIntroductionInformation
    {
        [Key]
        public int id { get; set; }
        public string? value { get; set; }
        //public string? username { get; set; }
        //public string? workplaceCode { get; set; }
        //public string? workplacePassword { get; set; }
        //public string? hash { get; set; }
        public DateTime? addDate { get; set; }
        public DateTime? updateDate { get; set; }
        public Boolean? active { get; set; }
        public string? region { get; set; }
    }
}
