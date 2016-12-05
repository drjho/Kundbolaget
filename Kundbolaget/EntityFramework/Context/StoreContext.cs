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
        public StoreContext()
            : base("KundbolagetDB")
        {

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Adress> Adresses { get; set; }
        public DbSet<Customer> Customers { get; set; }

    }
}