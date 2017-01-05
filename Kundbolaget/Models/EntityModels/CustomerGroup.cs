using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Kundbolaget.Models.EntityModels
{
    public class CustomerGroup
    {
        [Display (Name = "Kundgruppsid")]
        public int Id { get; set; }

        [Display(Name = "Namn")]
        public string Name { get; set; }

        [Display (Name = "Kunder")]
        public virtual List<Customer> CustomerList { get; set; }
    }
}