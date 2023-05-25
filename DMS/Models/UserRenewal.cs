 using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DMS.Models
{
    public partial class UserRenewal
    {
        public int UserRenewalId { get; set; }
        public int UserId { get; set; }
        public DateTime UserFrom { get; set; }
        public DateTime UserTo { get; set; }
        public string Terms { get; set; }
        public string Terms2 { get; set; }
        public string Terms3 { get; set; }
        public string Agreement { get; set; }
        public string RSignedAgreement { get; set; }
        public int RSign { get; set; }
        public int ConfirmAdmin { get; set; }
        public string Remarks { get; set; }
        public string CompAgreement { get; set; }
        public string CompAgreement2 { get; set; }
        public string CompRemarks { get; set; }
        public string CidAgreement { get; set; }
        public string CidRSignedAgreement { get; set; }
        public int CidRSign { get; set; }
        public int CidConfirmAdmin { get; set; }
        public string CidRemarks { get; set; }
        public int Readed { get; set; }
    }
}
