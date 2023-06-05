using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DMS.Models
{
    public class ResellerDetailsView
    {
        [Key]
        public int DisID { get; set; }
        public int DistriId { get; set; }
        public string DistributorId { get; set; }
        public string DistributorName { get; set; }
        public string DistributorType { get; set; }
        public string Country { get; set; }
        public string Zipcode { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Address1 { get; set; }    
        public string Address2 { get; set; }
        public string Email { get; set; }
        public string ContactPersonName { get; set; }
        public string PhoneNo { get; set; }
        public string VAT { get; set; }
        public string Website { get; set; }

        public string Bus_Status { get; set; }
        public string Tax_Status { get; set; }
    }
}
