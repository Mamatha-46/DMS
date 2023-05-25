using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.Models
{
    public class ProductMapView
    {
        [Key]
        public int productMapId { get; set; }
        public int ProductId { get; set; }
        public int RId { get; set; }
        public string ProductName { get; set; }
        public int FromQty { get; set; }
        public int ToQty { get; set; }
        public int Discount { get; set; }
        public int UnitPrice { get; set; }
        public int AfterDiscount { get; set; }
    }
}
