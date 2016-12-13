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

            var customers = new Customer[]
            {
                new Customer { Id = 1, Name = "Coop", CorporateStucture = "Koncern",
                    CreditLimit = -1, DaysToDelievery = 3,
                    CustomerAuditCode = 1, OrganisationNumber = 55555555}
            };

            var addresses = new Address[]
            {
                new Address { Id = 1, StreetName = "Kungsgatan", Number = 1, PostalCode = "11232", Area = "Stockholm", Country = "Sweden"},
                new Address { Id = 2, StreetName = "Kungsgatan", Number = 2, PostalCode = "11232", Area = "Stockholm", Country = "Sweden"},
                new Address { Id = 3, StreetName = "Kungsgatan", Number = 3, PostalCode = "11232", Area = "Stockholm", Country = "Sweden"}
            };

            var warehouses = new Warehouse[]
            {
                new Warehouse { Id = 1, Name = "Centrallagret", City = "Stockholm", Country = "Sweden", ZipCode = 11111 }
            };


            var products = new Product[]
            {
                new Product { Id = 1, Name = "Pripps blå", Alcohol = 3.5f, AuditCode = 1, ConsumerPackage = ConsumerPackage.Flaska, ConsumerPerStorage = 12, ProductGroup = ProductGroup.Öl, StoragePackage = StoragePackage.Back, VatCode = 1, Volume = 33},
                new Product { Id = 2, Name = "IPA", Alcohol = 4f, AuditCode = 1, ConsumerPackage = ConsumerPackage.Flaska, ConsumerPerStorage = 12, ProductGroup = ProductGroup.Öl, StoragePackage = StoragePackage.Back, VatCode = 1, Volume = 33},
                new Product { Id = 3, Name = "Strongbow", Alcohol = 4.5f, AuditCode = 1, ConsumerPackage = ConsumerPackage.Flaska, ConsumerPerStorage = 12, ProductGroup = ProductGroup.Cider, StoragePackage = StoragePackage.Back, VatCode = 1, Volume = 50},
                new Product { Id = 4, Name = "Bishops finger", Alcohol = 5.5f, AuditCode = 1, ConsumerPackage = ConsumerPackage.Flaska, ConsumerPerStorage = 12, ProductGroup = ProductGroup.Öl, StoragePackage = StoragePackage.Back, VatCode = 1, Volume = 50}
            };

            var ca = new CustomerAddress[]
            {
                new CustomerAddress { Id = 1, CustomerId =1, AddressId = 1 },
                new CustomerAddress { Id = 1, CustomerId =1, AddressId = 2 },
                new CustomerAddress { Id = 1, CustomerId =1, AddressId = 3 }

            };

            context.Warehouses.AddOrUpdate(warehouses);
            context.Customers.AddOrUpdate(customers);
            context.Addresses.AddOrUpdate(addresses);
            context.Products.AddOrUpdate(products);
            context.CustomerAddresses.AddOrUpdate(ca);

        }
    }
}
