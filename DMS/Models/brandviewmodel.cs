using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.Models
{
    public class brandviewmodel
    {
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public string BrandType { get; set; }
        public string CountryName { get; set; }
        public string StateName { get; set; }
        public string CityName { get; set; }

        public int? Country { get; set; }
        public int? State { get; set; }
        public int? City { get; set; }
        public string Zipcode { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Province { get; set; }
        public string Logo { get; set; }
        public string Material { get; set; }
        public int Status { get; set; }
        public int? AddedBy { get; set; }
        public DateTime? AddedOn { get; set; }
        public string IndustryName { get; set; }

        public string FileName { get; set; }

        public List<BrandFileViewModel> Files { get; set; }
        public IFormFile File { get; set; }





    }
}