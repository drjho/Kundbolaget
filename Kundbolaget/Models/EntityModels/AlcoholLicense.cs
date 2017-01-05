using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kundbolaget.Models.EntityModels
{
    public class AlcoholLicense
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Giltig fr.o.m")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "Giltig t.o.m")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        [Display(Name = "Kundid")]
        public int? CustomerId { get; set; }

        [Display(Name = "Kund")]
        public virtual Customer Customer { get; set; }

        public string AlcoLicenseString
        {
            get { return $"{StartDate} {EndDate} {Customer}"; }
        }


    }
}