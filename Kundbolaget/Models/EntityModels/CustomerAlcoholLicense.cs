using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kundbolaget.Models.EntityModels
{
    public class CustomerAlcoholLicense
    {
        public int Id { get; set; }

        [Display(Name = "Kund id")]
        public int? CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        [Display(Name = "Alkohollicens id")]
        public int? AlcoholLicenseId { get; set; }
        public virtual AlcoholLicense AlcoholLicense { get; set; }
    }
}