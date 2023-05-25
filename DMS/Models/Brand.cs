using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DMS.Models
{
    public partial class Brand
    {
        public int BrandId { get; set; }
        public string BrandName { get; set; }

        public string BrandType { get; set; }
        public int? Country { get; set; }
        public int? State { get; set; }
        public int? City { get; set; }
        public string Zipcode { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Province { get; set; }
        public string Logo { get; set; }
        public string Material { get; set; }
        public int Status { get; set; }
        public int? AddedBy { get; set; }
        public DateTime? AddedOn { get; set; }
        public List<BrandFile> Files { get; set; }
        // public string FileName { get; set; } // Add the FileName property to the Brand class


    }
}