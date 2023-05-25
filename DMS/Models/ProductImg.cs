using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DMS.Models
{
    public partial class ProductImg
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Image { get; set; }
    }
}
