using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DMS.Models
{
    public partial class DistriDocu
    {
        public int Id { get; set; }
        public int DistriId { get; set; }
        public string Business { get; set; }
        public int? BusiDocStatus { get; set; }
        public string BRemark { get; set; }
        public string ReBRemarks { get; set; }
        public string Gst { get; set; }
        public int? GstDocStatus { get; set; }
        public string Remarks { get; set; }
        public string ReGRemarks { get; set; }
        public int Opened { get; set; }
        public int Status { get; set; }
        public DateTime AddedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string AddedBy { get; set; }
    }
}
