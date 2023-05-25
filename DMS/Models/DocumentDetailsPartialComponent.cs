using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.Models
{
    public class DocumentDetailsPartialComponent
    {
        public string businessDoc { get; set; }
        public string taxDoc { get; set; }
        public int busiDocStatus { get; set; }
        public int taxDocStatus { get; set; }
        public string AdminRemark { get; set; }
        public string AdminRemarks { get; set; }
        public DateTime AddedOn { get; set; }
    }
}
