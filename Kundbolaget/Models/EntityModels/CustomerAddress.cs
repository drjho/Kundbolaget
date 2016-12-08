using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kundbolaget.Models.EntityModels
{
    public enum AddressType
    {
        Faktura, Leverans, Besök
    }

    public class CustomerAddress
    {
        public int Id { get; set; }

        [Display(Name ="Kund id")]
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        [Display(Name = "Adress id")]
        public int AddressId { get; set; }
        public virtual Address Address { get; set; }

        [Display(Name = "Adresstyp")]
        public AddressType AddressType { get; set; }

    }
}