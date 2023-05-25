using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DMS.Models
{
    public partial class PropInvoice
    {
        public int Id { get; set; }
        public string InvoiceId { get; set; }
        public int ProspectId { get; set; }
        public string Invoice { get; set; }
        public long? InvoiceAmt { get; set; }
        public int Status { get; set; }
        public DateTime AddedOn { get; set; }
        public string PaymentMode { get; set; }
        public string PaymentTranId { get; set; }
        public string PaymentRefImg { get; set; }
        public DateTime? PaymentAddedon1 { get; set; }
        public DateTime? PaymentAddedon { get; set; }
        public int Readed { get; set; }
        public int? Licence { get; set; }
        public int? ARead { get; set; }
    }
}
