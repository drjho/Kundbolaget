using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kundbolaget.Models.EntityModels
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual List<Adress> DeliveryAdresses{ get; set; }
        public virtual Adress InvoiceAdress { get; set; }
        public virtual Adress VisitAdress { get; set; }

    }
}