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
        Invoice, Delievery, Visit
    }

    public class Address
    {

        public int Id { get; set; }

        [Required]
        public string StreetName { get; set; }

        [Required]
        public int Number { get; set; }

        [Required]
        public string PostalCode { get; set; }

        [Required]
        public string Area { get; set; }

        /// <summary>
        /// Type: 0 = Faktura, 1 = Leverans, 2 = Besök.
        /// </summary>
        public AddressType Type { get; set; }

        public virtual Customer Customer { get; set; }

    }
}