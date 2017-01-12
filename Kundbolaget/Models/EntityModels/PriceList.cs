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
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [Display(Name = "á pris")]
        public float Price { get; set; }

        [Display(Name = "Kundgrupp")]
        public virtual CustomerGroup CustomerGroup { get; set; }
        [Display(Name = "Produkt")]
        public virtual Product Product { get; set; }

        public virtual int? ProductId { get; set; }
        public virtual int? CustomerGroupId { get; set; }

        [Display(Name = "Rabatt i %")]
        public int RebatePerPallet { get; set; }

    }
   
}