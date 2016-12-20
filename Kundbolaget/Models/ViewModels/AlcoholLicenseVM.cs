using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kundbolaget.Models.ViewModels
{
    public class AlcoholLicenseVM
    {
        public int Id { get; set; }

        [Display(Name = "Giltig fr.o.m")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Giltig t.o.m")]
        public DateTime EndDate { get; set; }

        public List<SelectListItem> CustomerList { get; set; }

        [Display(Name = "Kund")]
        public int CustomerId { get; set; }
    }

}