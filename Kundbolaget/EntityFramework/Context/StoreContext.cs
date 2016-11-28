using Kundbolaget.Models.EntityModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Kundbolaget.EntityFramework.Context
{
    public class StoreContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
    }
}