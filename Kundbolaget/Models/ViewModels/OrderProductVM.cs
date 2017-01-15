using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Kundbolaget.Models.EntityModels
{
    public class OrderProductVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Angett fel produktorder id")]
        [Display(Name = "Produkt id")]
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        [Required]
        [Display(Name = "Beställt antal")]
        public int OrderedAmount { get; set; }

        [Required]
        [Display(Name = "Tillgängligt antal")]
        public int AvailabeAmount { get; set; }

        [Display(Name = "Mottaget antal")]
        public int AcceptedAmount { get; set; }

        [Display(Name = "á Pris")]
        public float UnitPrice { get; set; }

        [Display(Name = "Pris")]
        public float Price { get; set; }

        [Display(Name = "Kommentar")]
        public string Comment { get; set; }

    }
}