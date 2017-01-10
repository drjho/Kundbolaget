﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Kundbolaget.Models.EntityModels
{
    public class OrderProduct
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
        [Display(Name = "Reserverat antal")]
        public int AvailabeAmount { get; set; }

        [Required]
        [Display(Name = "Plockat antal")]
        public int DeliveredAmount { get; set; }

        [Display(Name = "Mottaget antal")]
        public int AcceptedAmount { get; set; }

        [Display(Name = "Kommentar")]
        public string Comment { get; set; }

        [Display(Name = "Order id")]
        public int? OrderId { get; set; }
        [Display(Name = "Order")]
        public virtual Order Order { get; set; }

        [Display(Name = "Pris")]
        public float Price { get; set; }

        [Display(Name = "Plockordrar")]
        public virtual List<PickingOrder> PickList { get; set; } = new List<PickingOrder>();
    }
}