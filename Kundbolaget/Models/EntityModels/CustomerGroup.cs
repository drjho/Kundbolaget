using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kundbolaget.Models.EntityModels
{
    public class CustomerGroup
    {
        public int Id { get; set; }

        [Display(Name = "Namn")]
        public string Name { get; set; }

        public int? PriceListId { get; set; }
        public int? CustomerId { get; set; }

        public List<Customer> ListOfCustomers { get; set; }
        
    }
}