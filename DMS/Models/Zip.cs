using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DMS.Models
{
    public partial class Zip
    {
        public int Id { get; set; }
        public string Zipcode { get; set; }
        public string AreaName { get; set; }
        public string City { get; set; }
        public int? CityId { get; set; }
        public string State { get; set; }
        public int? StateId { get; set; }
        public string Country { get; set; }
        public int CountryId { get; set; }
        public string Division1 { get; set; }
        public string Division2 { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public int? Accuracy { get; set; }
    }

    

}
