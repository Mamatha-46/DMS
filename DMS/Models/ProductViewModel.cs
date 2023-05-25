using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.Models
{
    public class ProductViewModel
    {
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public int? Brand { get; set; }
        public int? Model { get; set; }
        public string Variant { get; set; }
        public string ProductName { get; set; }
        public string ProductDesc { get; set; }
        public string ProductImg { get; set; }
        public int ProductPrice { get; set; }
        public int? FromNode { get; set; }
        public int? ToNode { get; set; }
        public DateTime? AddedOn { get; set; }
        public string AddedBy { get; set; }
        public int Status { get; set; }
        public string FileName { get; set; }

        public List<ProductfileViewModel> Files { get; set; }
        public IFormFile File { get; set; }
    }
}
