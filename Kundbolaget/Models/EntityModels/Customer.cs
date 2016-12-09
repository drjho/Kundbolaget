﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kundbolaget.Models.EntityModels
{
    public class Customer
    {
        public int Id { get; set; }
        [Required]
        [Display(Name ="Kundnamn")]
        public string Name { get; set; }
        [Display(Name = "Adresser")]
        public virtual List<Address> Addresses{ get; set; }


        public Customer()
        {
            Addresses = new List<Address>();
        }



    }

    


}