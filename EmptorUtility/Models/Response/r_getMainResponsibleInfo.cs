using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmptorUtility.Models.Response
{
    public class r_getMainResponsibleInfo
    {
        public Boolean? status { get; set; }
        public string? MainResponsibleFullName { get; set; }
        public string? MainResponsibleEmail { get; set; }
        public string? SubResponsibleFullName { get; set; }
        public string? SubResponsibleEmail { get; set; }
    }
}
