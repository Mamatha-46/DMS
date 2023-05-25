using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.Models
{
    public class InvoiceViewModel
    {
        public int Id { get; set; }
        public string InvoiceNo { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Amount { get; set; }
        public int Status { get; set; }
        public DateTime AddedOn { get; set; }
        public int Licence { get; set; }
    }
}
