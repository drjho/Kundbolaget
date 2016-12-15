using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Security.AccessControl;
using System.Web;

namespace Kundbolaget.Models.EntityModels
{

    public class Address
    {

        public int Id { get; set; }

        [Required]
        [Display(Name = "Gatunamn")]
        public string StreetName { get; set; }

        [Required]
        [Display(Name = "Gatunummer")]
        public int Number { get; set; }

        [Required]
        [Display(Name = "Postnummer")]
        public string PostalCode { get; set; }

        [Required]
        [Display(Name = "Ort")]
        public string Area { get; set; }

        [Required]
        [Display(Name = "Land")]
        public string Country { get; set; }

        [Display(Name ="Adress")]
        public string AddressString
        {
            get
            {
                return $"{StreetName} {Number}, {PostalCode} {Area} {Country}";
            }

        }
    }
}