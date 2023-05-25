using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DMS.Models
{
    public partial class States
    {
        public States()
        {
            Cities = new HashSet<Cities>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int CountryId { get; set; }
        public string CountryCode { get; set; }
        public string FipsCode { get; set; }
        public string Iso2 { get; set; }
        public string Type { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public bool? Flag { get; set; }
        public string WikiDataId { get; set; }

        public virtual Countries Country { get; set; }
        public virtual ICollection<Cities> Cities { get; set; }
    }
}
