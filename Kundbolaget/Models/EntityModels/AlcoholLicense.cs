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
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "Giltig t.o.m")]
        public DateTime EndDate { get; set; }

        public virtual Customer Customer { get; set; }
    }
}