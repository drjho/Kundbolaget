using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kundbolaget.Models.EntityModels
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Användare Namn")]
        public string Name { get; set; }
        public virtual List<User> Users { get; set; }

        public User()
        {
            Users = new List<User>();
        }

    }
}