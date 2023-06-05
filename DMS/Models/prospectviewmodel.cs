using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DMS.Models
{
    public class prospectviewmodel
    {
        public int Id { get; set; }
        public string UserFname { get; set; }
        public string DistributorName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public long Number { get; set; }
        public string IndustryType { get; set; }
        public string CountryName { get; set; }
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
        //public int? Status { get; set; }
        public string Terms { get; set; }
        public int AddedId { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int ProsId { get; set; }
        public int ProductId { get; set; }
        public int Discount { get; set; }
        public int? EQty { get; set; }
        public int? UPrice { get; set; }
        public int STotal { get; set; }
        
        public string Stage { get; set; }
        public DateTime Date { get; set; }
        public string Comments { get; set; }
      
        
 }


}