using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kundbolaget.Models.EntityModels
{
    public class Invoice
    {
        [Display(Name = "Fakturaid")]
        public int Id { get; set; }

        [Display(Name = "Orderid")]
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Faktureringsdatum")]
        public DateTime InvoiceDate { get; set; }

        [Display(Name = "Betald")]
        public bool Paid { get; set; }

        [Display(Name = "Förfallen")]
        public bool IsOverdue => DateTime.Today.Date.CompareTo(InvoiceDate.Date.AddDays(15)) > 0;

        [Display(Name = "Totalpris")]
        public float TotalPrice => (Order != null && Order.OrderProducts != null) ? Order.OrderProducts.Sum(x => x.Price) : 0;

    }
}