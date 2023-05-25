using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DMS.Models
{
    public partial class Prospect
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public long Number { get; set; }
        public string IndustryType { get; set; }
        public string CountryName { get; set; }
        public int? CountryId { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Province { get; set; }
        public string CPerson { get; set; }
        public string Website { get; set; }
        public string Logo { get; set; }
        public string Gst { get; set; }
        public string Source { get; set; }
        public string LeadType { get; set; }
        public int ProsType { get; set; }
        public int? Status { get; set; }
        public string Terms { get; set; }
        public string Bank { get; set; }
        public DateTime AddedOn { get; set; }
        public int AddedId { get; set; }
        public string AddedBy { get; set; }
        public string UpdatedBy { get; set; }
        public int? UpdatedId { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int? Readed { get; set; }
        public int AReaded { get; set; }
        public int? Converted { get; set; }
    }
}
