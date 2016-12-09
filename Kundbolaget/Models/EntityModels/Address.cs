using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Security.AccessControl;
using System.Web;

namespace Kundbolaget.Models.EntityModels
{
    public enum AddressType
    {
        Fakturaadress, Leveransadress, Besöksadress
    }

    public class Address
    {

        public int Id { get; set; }

        [Required]
        [Display(Name = "Gatuadress")]
        public string StreetName { get; set; }

        [Required]
        [Display(Name = "Nr.")]
        public int Number { get; set; }

        [Required]
        [Display(Name = "Postnummer")]
        public string PostalCode { get; set; }

        [Required]
        [Display(Name = "Ort")]
        public string Area { get; set; }

        /// <summary>
        /// Type: 0 = Faktura, 1 = Leverans, 2 = Besök.
        /// </summary>
        [Display(Name = "Typ")]
        public AddressType Type { get; set; }

        public int CustomerId { get; set; }
        [Display(Name = "Kund")]
        public virtual Customer Customer { get; set; }

    }
}