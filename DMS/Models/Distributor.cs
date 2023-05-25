using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DMS.Models
{
    public partial class Distributor
    {
        public int DisId { get; set; }
        public string DistributorId { get; set; }
        public int UserId { get; set; }
        public string DistributorName { get; set; }
        public string Website { get; set; }
        public string DistributorType { get; set; }
        public string DistributorZip { get; set; }
        public int DistributorCountry { get; set; }
        public int DistributorState { get; set; }
        public int DistributorCity { get; set; }
        public string DistributorAddress1 { get; set; }
        public string DistributorAddress2 { get; set; }
        public string Province { get; set; }
        public string DistributorCName { get; set; }
        public long DistributorNumber { get; set; }
        public string DistributorVat { get; set; }
        public string Doctype { get; set; }
        public string Docfile { get; set; }
        public string Terms { get; set; }
        public string Clauses { get; set; }
        public string Bank { get; set; }
        public DateTime AddedOn { get; set; }
        public string AddedBy { get; set; }
        public string Refer { get; set; }
    }
}
