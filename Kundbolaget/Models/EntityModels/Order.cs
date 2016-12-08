using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kundbolaget.Models.EntityModels
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        [Display( Name ="Orderdatum")]
        public DateTime OrderDate { get; set; }

        [Required]
        [Display( Name ="Önskad leveransdatum")]
        public DateTime DesiredDeliveryDate { get; set; }
        
        [Display( Name ="Planerad leveransdatum")]
        public DateTime PlannedDeliveryDate { get; set; }    

        [Required, Display(Name ="Kund id")]
        public int  CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        public virtual List<OrderProduct> OrderProducts { get; set; }
    }
}