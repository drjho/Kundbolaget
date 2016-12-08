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
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerAddress> CustomerAddresses { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<StoragePlace> StoragePlaces { get; set; }
        
    }
}