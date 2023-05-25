using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DMS.Models
{
    public partial class Product
    {
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public int? Brand { get; set; }
        public int? Model { get; set; }
        public string Variant { get; set; }
        public string ProductName { get; set; }
        public string ProductDesc { get; set; }
        public string ProductImg { get; set; }
        public int ProductPrice { get; set; }
        public int? FromNode { get; set; }
        public int? ToNode { get; set; }
        public DateTime? AddedOn { get; set; }
        public string AddedBy { get; set; }
        public int Status { get; set; }

        public List<ProductFile> Files { get; set; }
    }
}
