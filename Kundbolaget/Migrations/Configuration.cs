namespace Kundbolaget.Migrations
{
    using Models.EntityModels;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Kundbolaget.EntityFramework.Context.StoreContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Kundbolaget.EntityFramework.Context.StoreContext context)
        {

            //var customers = new Customer[]
            //{
            //    new Customer { Id = 1, Name = "Coop"}
            //};

            //var addresses = new Address[]
            //{
            //    new Address { Id = 1, StreetName = "Kungsgatan", Number = 1, PostalCode = "11232", Area = "Stockholm"},
            //    new Address { Id = 2, StreetName = "Kungsgatan", Number = 2, PostalCode = "11232", Area = "Stockholm"},
            //    new Address { Id = 3, StreetName = "Kungsgatan", Number = 3, PostalCode = "11232", Area = "Stockholm"}
            //};

            //var warehouses = new Warehouse[]
            //{
            //    new Warehouse { Id = 1, Name = "W1", City = "Stockholm", Country = "Sweden", ZipCode = 11111 },
            //    new Warehouse { Id = 2, Name = "W2", City = "Stockholm", Country = "Sweden", ZipCode = 11760 }
            //};


            ////var products = new Product[]
            ////{
            ////    new Product { Id = 1, Name = "P1", Price = 100},
            ////    new Product { Id = 2, Name = "P2", Price = 100},
            ////    new Product { Id = 3, Name = "P3", Price = 100},
            ////    new Product { Id = 4, Name = "P4", Price = 100}
            ////};

            ////customers[0].Addresses.AddRange(addresses);

            //context.Customers.AddOrUpdate(customers);
            //context.Addresses.AddOrUpdate(addresses);

            ////context.Products.AddOrUpdate(products);
            //context.Warehouses.AddOrUpdate(warehouses);

        }
    }
}
