using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kundbolaget.Models.ViewModels
{
    public class AlcoholLicenseVM
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int CustomerId { get; set; }
        //public List< SelectListItem> CustomerList { get; set; }
    }
}