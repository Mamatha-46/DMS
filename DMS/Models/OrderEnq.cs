using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DMS.Models
{
    public partial class OrderEnq
    {
        public int OrderEnqId { get; set; }
        public int OrdersId { get; set; }
        public int ProductId { get; set; }
        public int Qty { get; set; }
        public int UnitPrice { get; set; }
        public int? Discount { get; set; }
        public int? STotal { get; set; }
        public int Status { get; set; }
        public string Comment { get; set; }
        public string RComment { get; set; }
        public string AComment { get; set; }
        public DateTime AdddedOn { get; set; }
        public string AddedBy { get; set; }
    }
}
