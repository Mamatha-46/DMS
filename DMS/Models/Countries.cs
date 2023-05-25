using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DMS.Models
{
    public partial class Countries
    {
        public Countries()
        {
            Cities = new HashSet<Cities>();
            States = new HashSet<States>();
        }

        //public int Id { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Shortname { get; set; }
        public string NumericCode { get; set; }
        public string Iso2 { get; set; }
        public int? TaxPerc { get; set; }
        public string Phonecode { get; set; }
        public string Capital { get; set; }
        public string Currency { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencySymbol { get; set; }
        public string Tld { get; set; }
        public string Native { get; set; }
        public string Region { get; set; }
        public string Subregion { get; set; }
        public string Timezones { get; set; }
        public string Translations { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string Emoji { get; set; }
        public string EmojiU { get; set; }
        public bool? Flag { get; set; }
        public string Flagico { get; set; }
        public string WikiDataId { get; set; }

        public virtual ICollection<Cities> Cities { get; set; }
        public virtual ICollection<States> States { get; set; }


    }
}
