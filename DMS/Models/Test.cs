using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DMS.Models
{
    public partial class Test
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public long Number { get; set; }
    }
}
