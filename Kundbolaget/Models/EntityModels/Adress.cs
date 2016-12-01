using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Security.AccessControl;
using System.Web;

namespace Kundbolaget.Models.EntityModels
{
    public class Adress
    {
        public int Id { get; set; }
        public virtual Customer Customer { get; set; }


    }
}