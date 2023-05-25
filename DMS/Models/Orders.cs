using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DMS.Models
{
    public partial class Orders
    {
        public int OrdersId { get; set; }
        public int OrderedBy { get; set; }
        public string Company { get; set; }
        public int CType { get; set; }
        public int Country { get; set; }
        public int State { get; set; }
        public int City { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Email { get; set; }
        public string CPerson { get; set; }
        public long CPhone { get; set; }
        public string CVat { get; set; }
        public int? OrderAmt { get; set; }
        public int Status { get; set; }
        public DateTime AddedOn { get; set; }
    }
}
