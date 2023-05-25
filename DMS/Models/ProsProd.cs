using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DMS.Models
{
    public partial class ProsProd
    {
        public int Id { get; set; }
        public int ProsId { get; set; }
        public int ProductId { get; set; }
        public int Discount { get; set; }
        public int? EQty { get; set; }
        public int? UPrice { get; set; }
        public int STotal { get; set; }
        public int Status { get; set; }
        public string FStage { get; set; }
        public int? InvoiceId { get; set; }
        public DateTime AddedOn { get; set; }
    }
}
