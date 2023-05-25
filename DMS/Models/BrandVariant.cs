using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DMS.Models
{
    public partial class BrandVariant
    {
        public int VariantId { get; set; }
        public int BrandId { get; set; }
        public int ModelId { get; set; }
        public string VariantName { get; set; }
        public int Status { get; set; }
        public DateTime AddedOn { get; set; }
    }
}
