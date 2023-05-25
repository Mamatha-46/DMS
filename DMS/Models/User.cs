using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DMS.Models
{
    public partial class User
    {
        public int UserId { get; set; }
        public string UserFname { get; set; }
        public string UserLname { get; set; }
        public string UserMail { get; set; }
        public string Pwd { get; set; }
        public string UserPwd { get; set; }
        public long UserNumber { get; set; }
        public string UserType { get; set; }
        public string UserLogo { get; set; }
        public string UserPic { get; set; }
        public string Bank { get; set; }
        public int UserStatus { get; set; }
        public int Readed { get; set; }
        public int? AReaded { get; set; }
        public DateTime AddedOn { get; set; }
    }
}
