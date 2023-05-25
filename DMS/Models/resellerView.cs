using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.Models
{
    public class resellerView
    {
        [Key]
        public int DistributorID { get; set; }
        public string DID { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public int Status { get; set; }
    }
}
