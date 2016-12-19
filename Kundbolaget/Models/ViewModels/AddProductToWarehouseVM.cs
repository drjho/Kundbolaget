using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kundbolaget.Models.ViewModels
{
    public class AddProductToWarehouseVM
    {
        public int Id { get; set; }

        [Required, Display( Name = "Produkt id")]
        public int ProductId { get; set; }

        [Required, Display( Name = "Lagerhus id")]
        public int WarehouseId { get; set; }

        [Required, Display( Name = "Antal")] 
        public int Amount { get; set; }
    }
}