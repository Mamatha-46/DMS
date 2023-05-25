using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DMS.Models
{
    public partial class ProsQuote
    {
        public int Id { get; set; }
        public string QuoteId { get; set; }
        public int ProspectId { get; set; }
        public string Quote { get; set; }
        public long? QuoteAmt { get; set; }
        public DateTime AddedOn { get; set; }
    }
}
