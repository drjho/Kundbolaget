using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kundbolaget.Models.EntityModels
{
    public class Order
    {
        [Display(Name = "Order id")]
        public int Id { get; set; }

        [Required]
        [Display( Name ="Orderdatum")]
        public DateTime OrderDate { get; set; }

        [Required]
        [Display( Name ="Önskad leveransdatum")]
        public DateTime DesiredDeliveryDate { get; set; }
        
        [Display( Name ="Planerad leveransdatum")]
        public DateTime PlannedDeliveryDate { get; set; }    

        [Required, Display(Name ="Kundadress id")]
        public int CustomerAddressId { get; set; }
        [Display(Name = "Kundadress")]
        public virtual CustomerAddress CustomerAddress { get; set; }

        [Required, Display(Name = "Kommentarer")]
        public string Comments { get; set; }

        public virtual List<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();

    }
}