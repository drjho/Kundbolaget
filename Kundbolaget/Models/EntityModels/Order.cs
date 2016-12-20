﻿using Newtonsoft.Json.Linq;
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

        [Required (ErrorMessage = "Angett fel kundorder id")]
        [Display(Name = "Kund id")]
        public int? CustomerId { get; set; }
        [Display(Name = "Kund")]
        public virtual Customer Customer { get; set; }

        [Required]
        [Display(Name = "Önskad leveransdatum")]
        public DateTime DesiredDeliveryDate { get; set; }

        [Display( Name ="Planerad leveransdatum")]
        public DateTime PlannedDeliveryDate { get; set; }

        [Required(ErrorMessage = "Angett fel adressorder id")]
        [Display(Name ="Kundadress id")]
        public int? AddressId { get; set; }
        [Display(Name = "Kundadress")]
        public virtual Address Address { get; set; }

        [Display(Name = "Kommentarer")]
        public string Comment { get; set; }

        [Display(Name = "Importkommentarer")]
        public string ImportComments { get; set; }

        public virtual List<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();

    }
}