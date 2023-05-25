using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DMS.Models
{
    public partial class ProsProdLoop
    {
        public int Id { get; set; }
        public int ProsProdId { get; set; }
        public string Stage { get; set; }
        public DateTime Date { get; set; }
        public string Comments { get; set; }
    }
}
