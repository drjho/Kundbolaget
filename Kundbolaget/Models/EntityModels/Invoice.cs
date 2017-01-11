using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kundbolaget.Models.EntityModels
{
    public class Invoice
    {
        [Display(Name = "Order id")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Faktureringsdatum")]
        public DateTime InvoiceDate { get; set; }

        [Display(Name = "Produkt")]
        public virtual List<Product> Product { get; set; }

        [Display(Name = "Betald")]
        public bool Paid { get; set; }

        [Display(Name = "Förfallodag")]
        public DateTime PayBefore {
            get { return this.InvoiceDate.Date.AddDays(15); }
            set { this.InvoiceDate.Date.AddDays(15); } }

        [Display(Name = "Kund")]
        public virtual Customer Customer { get; set; }

        [Display(Name = "Kundadress")]
        public Address Address { get; set; }

        

        //public PriceList PriceList { get; set; }

        //[Required]
        //[Display(Name = "Rabatt")]
        //public int Rebate { get; set; }

        //[Display(Name = "Antal")]
        //public int ProductAmount { get; set; }
        //[Display(Name = "Momskod")]
        //public decimal VatCode { get; set; }

        //[Required]
        //[Display(Name = "Pris")]
        //public decimal Price { get; set; }
    }
}