using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kundbolaget.Models.EntityModels
{
    public class PriceList
    {
        public int Id { get; set; }

        [Display(Name = "Giltig från")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Pris")]
        public decimal Price { get; set; }

        [Display(Name = "Kundgrupp")]
        public virtual CustomerGroup CustomerGroup { get; set; }

        public virtual OrderProduct Product { get; set; }

        public virtual int? ProductId { get; set; }

        [Display(Name = "Rabatt i %")]
        public int RebatePerPallet { get; set; }

    }
   
}