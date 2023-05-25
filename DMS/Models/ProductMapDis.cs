using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DMS.Models
{
    public partial class ProductMapDis
    {
        public int ProductMapDisId { get; set; }
        public int ProductMapId { get; set; }
        public int DisStart { get; set; }
        public int DisEnd { get; set; }
        public int Discount { get; set; }
    }
}
