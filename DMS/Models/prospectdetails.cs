using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace DMS.Models
{
    public class prospectdetails
    {
        public int DisId { get; set; }
        [DisplayName("Reseller")]
        public string DistributorName { get; set; }
        [DisplayName("ProspectName")]
        public string Name { get; set; }
        [DisplayName("ContactName")]
        public string DistributorCName { get; set; }
        [DisplayName("PhoneNumber")]
        public long DistributorNumber { get; set; }
        public string CountryName { get; set; }

        public int Id { get; set; }

        public string Email { get; set; }
        [DisplayName("CreatedDate")]
        public DateTime AddedOn { get; set; }

    }
}
