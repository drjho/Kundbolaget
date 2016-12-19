using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Kundbolaget.Models.EntityModels
{
    public class OrderProduct : IValidatableObject
    {
        public int Id { get; set; }

        //[Required(ErrorMessage = "Angett fel produktorder id")]
        [Display(Name = "Produkt id")]
        public int? ProductId { get; set; }

        [Display(Name = "Produkt")]
        public virtual Product Product { get; set; }

        [Required]
        [Display(Name = "Beställt antal")]
        public int OrderedAmount { get; set; }

        [Required]
        [Display(Name = "Levererat antal")]
        public int DeliveredAmount { get; set; }

        [Display(Name = "Mottaget antal")]
        public int AcceptedAmount { get; set; }

        [Display(Name = "Kommentar")]
        public string Comment { get; set; }

        [Display(Name = "Order id")]
        public int? OrderId { get; set; }
        [Display(Name = "Order")]
        public virtual Order Order { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var result = new List<ValidationResult>();

            if (ProductId == null)
            {
                result.Add(new ValidationResult("Angett fel produktorder id.", new[] { "ProductId" }));
            }
            if (DeliveredAmount < OrderedAmount >> 1)
            {
                result.Add(new ValidationResult("Det finns mindre än halva .", new[] { "DeliveredAmount" }));
            }
            return result;

        }
    }
}