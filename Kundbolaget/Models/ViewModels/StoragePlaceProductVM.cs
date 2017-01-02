using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kundbolaget.Models.ViewModels
{
    public class StoragePlaceProductVM
    {
        public int Id { get; set; }

        [Required, Display( Name = "Produkt id")]
        public int ProductId { get; set; }

        [Required, Display( Name = "Lagerhus id")]
        public int WarehouseId { get; set; }

        [Required, Display( Name = "Totalt")] 
        public int TotalAmount { get; set; }

        [Required, Display(Name = "Reserverat")]
        public int ReservedAmount { get; set; }
    }
}