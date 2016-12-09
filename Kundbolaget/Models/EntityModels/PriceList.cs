using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kundbolaget.Models.EntityModels
{
    public class PriceList
    {
        [Display(Name = "Giltig från")]
        public DateTime StartDate { get; set; }
        [Display(Name = "Giltig till")]
        public DateTime EndTime { get; set; }
        [Display(Name = "Landkod")]
        public string CountryCode { get; set; }
        [Display(Name = "Produkt nr")]
        public string ProductNr { get; set; }
        [Display(Name = "Kund")]
        public Customer Customer { get; set; }
        [Display(Name = "PrisLista")]
        public List<decimal> Price { get; set; }

    }
    /* prislista i SEK men även EUR. måste ha ett startdatum.
    prislista bör ha en landkod, kundgruppskod, produkt nr, pris/lagerförpackning, rabatt / pall
*/
}