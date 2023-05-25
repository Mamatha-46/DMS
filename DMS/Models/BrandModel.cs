using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DMS.Models
{
    public partial class BrandModel
    {
        public int BrandModelId { get; set; }
        public int BrandId { get; set; }
        public string BrandModelName { get; set; }
        public int Status { get; set; }
        public DateTime AddedOn { get; set; }
    }
}
