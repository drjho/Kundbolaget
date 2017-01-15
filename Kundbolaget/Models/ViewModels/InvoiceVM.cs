using Kundbolaget.Models.EntityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kundbolaget.Models.ViewModels
{
    public class InvoiceVM
    {
        [Display(Name = "Fakturaid")]
        public int Id { get; set; }

        [Display(Name = "Kund")]
        public string CustomerName { get; set; }

        [Display(Name = "Adress")]
        public string Address { get; set; }

        [Display(Name = "Orderid")]
        public int OrderId { get; set; }

        public List<OrderProductVM> ProductList { get; set; } = new List<OrderProductVM>();

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Faktureringsdatum")]
        public DateTime InvoiceDate { get; set; }

        [Display(Name = "Betald")]
        public bool Paid { get; set; }

        [Display(Name = "Förfallen")]
        public bool IsOverdue { get; set; }

        [Display(Name = "Totalpris")]
        public float TotalPrice { get; set; }

        [Display(Name = "Moms")]
        public float VAT { get; set; }

        [Display(Name = "Summa")]
        public float TotalSum { get; set; }


    }
}