using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DMS.Models
{
    public partial class InvoiceLicence
    {
        public int Id { get; set; }
        public string InvoiceId { get; set; }
        public string File1 { get; set; }
        public string File2 { get; set; }
        public int? Status { get; set; }
    }
}
