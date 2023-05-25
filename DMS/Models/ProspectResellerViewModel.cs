using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.Models
{
    public class ProspectResellerViewModel
    {

        public int Id { get; set; }
        public int AddedId { get; set; }
        public string Name { get; set; }
        public string IndustryType { get; set; }
        public string CountryName { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public int? CityId { get; set; }
        public string Zip { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Email { get; set; }
        public string CPerson { get; set; }
        public long Number { get; set; }
        public string Website { get; set; }
        public string Gst { get; set; }
        public string Source { get; set; }
        public string Tax { get; set; }
        public string LeadType { get; set; }
        public int? Status { get; set; }
        public string Terms { get; set; }
        //public string Logo { get; set; }
    }
}
