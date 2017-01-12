using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kundbolaget.Models.EntityModels
{
    public class Invoice
    {
        public int Id { get; set; }

        [Display(Name = "Order Id")]
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Faktureringsdatum")]
        public DateTime InvoiceDate { get; set; }

        [Display(Name = "Produkt")]
        public virtual List<OrderProduct> Product { get; set; }

        [Display(Name = "Betald")]
        public bool Paid { get; set; }

        public bool IsOverdue => DateTime.Today.Date.CompareTo(InvoiceDate.Date.AddDays(15)) > 0;

        public int? CustomerId { get; set; }
        [Display(Name = "Kund")]
        public virtual Customer Customer { get; set; }

        public int? AddressId { get; set; }
        [Display(Name = "Kundadress")]
        public virtual Address Address { get; set; }

        public virtual List<PriceList> PriceList { get; set; }

        
    }
}