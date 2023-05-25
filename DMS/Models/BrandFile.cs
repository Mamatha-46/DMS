﻿using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DMS.Models
{
    public partial class BrandFile
    {
        public int Id { get; set; }
        public int? BrandId { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string File { get; set; }
    }
}
