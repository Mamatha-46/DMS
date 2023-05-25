using System;


namespace DMS.Models
{
    public class ProspectDoc
    {

        public string ProspectName { get; set; }
        public string IndustryType { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Zipcode { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string EmailAddress { get; set; }
        public string ContactPerson { get; set; }
        public long Phone { get; set; }
        public string Website { get; set; }
        public string Gst { get; set; }
        public string Source { get; set; }
        public int? Tax { get; set; }
        public string LeadType { get; set; }


    }

}