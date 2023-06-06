using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.Models
{
    public class DistributorViewmodel
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
        public string Email { get; set; }
        public string DistributorCName { get; set; }
        public long DistributorNumber { get; set; }
        public string DistributorVat { get; set; }
        public string UserLogo { get; set; }
        public string CompanyDocument { get; set; }
        public string Taxdocumebt { get; set; }

    }
}
