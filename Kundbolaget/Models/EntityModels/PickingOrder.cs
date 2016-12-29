using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Kundbolaget.Models.EntityModels
{
    /// <summary>
    /// Plockorder visar från vilken pallplats och hur mycket man ska plocka.
    /// Det är underordnat "Orderproduct".
    /// </summary>
    public class PickingOrder
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Pallplats id")]
        public int? StoragePlaceId { get; set; }

        [Display(Name = "Pallplats")]
        public virtual StoragePlace StoragePlace { get; set; }

        [Required]
        [Display(Name = "Reserverat antal")]
        public int ReservedAmount { get; set; }

        [Required]
        [Display(Name = "Plockat antal")]
        public int PickedAmount { get; set; }

        [Display(Name = "Kommentar")]
        public string Comment { get; set; }

        [Required]
        [Display(Name = "Orderrad id")]
        public int? OrderProductId { get; set; }

        [Display(Name = "Orderrad")]
        public virtual OrderProduct OrderProduct { get; set; }
    }
}