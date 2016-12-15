using Kundbolaget.Models.EntityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kundbolaget.Models.ViewModels
{
    public class CustomerAddressVM
    {
        public int Id { get; set; }

        [Display(Name = "Adresstyp")]
        public AddressType AddressType { get; set; }

        [Display( Name ="Kund")]
        public int CustomerId { get; set; }

        public List<SelectListItem> CustomerList { get; set; }

        [Display(Name = "Adress")]
        public int AddressId { get; set; }
        [Display(Name = "Adress")]

        public List<SelectListItem> AddressList { get; set; }
    }
}