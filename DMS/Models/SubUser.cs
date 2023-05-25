using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DMS.Models
{
    public partial class SubUser
    {
        public int SubUserId { get; set; }
        public int UserId { get; set; }
        public string UserType { get; set; }
        public int AddedBy { get; set; }
    }
}
