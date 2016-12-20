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

        [Display(Name = "Produkt")]
        public Product Product { get; set; }

        [Required]
        [Display(Name = "Beställt antal")]
        public int OrderedAmount { get; set; }

        [Required]
        [Display(Name = "Tillgängligt antal")]
        public int AvailabeAmount { get; set; }

        [Display(Name = "Kommentar")]
        public string Comment { get; set; }

    }
}