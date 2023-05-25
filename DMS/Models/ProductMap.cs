using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DMS.Models
{
    public partial class ProductMap
    {
        public int ProductMapId { get; set; }
        public int RId { get; set; }
        public int ProductId { get; set; }
        public DateTime AddedOn { get; set; }
        public int? Discount { get; set; }
        public double? ADiscount { get; set; }
    }
}
