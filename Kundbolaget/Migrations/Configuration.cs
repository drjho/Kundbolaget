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
                    CustomerAuditCode = 1, OrganisationNumber = "555555-5455",
                    CustomerGroupId = 1},
                new Customer { Id = 2, Name = "ICA", CorporateStucture = "Koncern",
                    CreditLimit = -1, DaysToDelievery = 3,
                    CustomerAuditCode = 1, OrganisationNumber = "345555-7645",
                    CustomerGroupId = 1},
                new Customer { Id = 3, Name = "No License Company", CorporateStucture = "",
                    CreditLimit = 1000, DaysToDelievery = 1,
                    CustomerAuditCode = 1, OrganisationNumber = "309545-2345",
                    CustomerGroupId = 1},
                new Customer { Id = 4, Name = "Low credit Company", CorporateStucture = "",
                    CreditLimit = 100, DaysToDelievery = 1,
                    CustomerAuditCode = 1, OrganisationNumber = "308775-2675",
                    CustomerGroupId = 1}
            };

            var addresses = new Address[]
            {
                new Address { Id = 1, StreetName = "Kungsgatan", Number = 1, PostalCode = "11232", Area = "Stockholm", Country = "Sweden"},
                new Address { Id = 2, StreetName = "Kungsgatan", Number = 2, PostalCode = "11232", Area = "Stockholm", Country = "Sweden"},
                new Address { Id = 3, StreetName = "Kungsgatan", Number = 3, PostalCode = "11232", Area = "Stockholm", Country = "Sweden"},
                new Address { Id = 4, StreetName = "Bergsgatan", Number = 77, PostalCode = "11632", Area = "Stockholm", Country = "Sweden"},
                new Address { Id = 5, StreetName = "Licensgatan", Number = 101, PostalCode = "13452", Area = "Stockholm", Country = "Sweden"},
                new Address { Id = 6, StreetName = "Kreditgatan", Number = 202, PostalCode = "10982", Area = "Stockholm", Country = "Sweden"}

            };

            var products = new Product[]
            {
                new Product { Id = 1, Name = "Pripps bl�", Alcohol = 3.5f, AuditCode = 1, ConsumerPackage = ConsumerPackage.Flaska, ConsumerPerStorage = 12, ProductGroup = ProductGroup.�l, StoragePackage = StoragePackage.Back, VatCode = 1, Volume = 33},
                new Product { Id = 2, Name = "IPA", Alcohol = 4f, AuditCode = 1, ConsumerPackage = ConsumerPackage.Flaska, ConsumerPerStorage = 12, ProductGroup = ProductGroup.�l, StoragePackage = StoragePackage.Back, VatCode = 1, Volume = 33},
                new Product { Id = 3, Name = "Strongbow", Alcohol = 4.5f, AuditCode = 1, ConsumerPackage = ConsumerPackage.Flaska, ConsumerPerStorage = 12, ProductGroup = ProductGroup.Cider, StoragePackage = StoragePackage.Back, VatCode = 1, Volume = 50},
                new Product { Id = 4, Name = "Bishops finger", Alcohol = 5.5f, AuditCode = 1, ConsumerPackage = ConsumerPackage.Flaska, ConsumerPerStorage = 12, ProductGroup = ProductGroup.�l, StoragePackage = StoragePackage.Back, VatCode = 1, Volume = 50},
                new Product { Id = 5, Name = "Chapel Hill", Alcohol = 12f, AuditCode = 1, ConsumerPackage = ConsumerPackage.Flaska, ConsumerPerStorage = 12, ProductGroup = ProductGroup.Mousserande, StoragePackage = StoragePackage.Back, VatCode = 1, Volume = 75},
                new Product { Id = 6, Name = "Koskenkorva", Alcohol = 40f, AuditCode = 1, ConsumerPackage = ConsumerPackage.Flaska, ConsumerPerStorage = 12, ProductGroup = ProductGroup.Starksprit, StoragePackage = StoragePackage.Kartong, VatCode = 1, Volume = 100},
                new Product { Id = 7, Name = "Eriksberg Hovm�starlager", Alcohol = .5f, AuditCode = 1, ConsumerPackage = ConsumerPackage.Flaska, ConsumerPerStorage = 24, ProductGroup = ProductGroup.Alkoholfritt, StoragePackage = StoragePackage.Back, VatCode = 1, Volume = 33},
                new Product { Id = 8, Name = "Dom Perignon", Alcohol = 12.5f, AuditCode = 1, ConsumerPackage = ConsumerPackage.Flaska, ConsumerPerStorage = 4, ProductGroup = ProductGroup.Mousserande, StoragePackage = StoragePackage.Kartong, VatCode = 1, Volume = 75, ProductStatus = ProductStatus.Best�llningsvara}
            };

            var warehouses = new Warehouse[]
            {
                new Warehouse { Id = 1, Name = "Centrallagret", City = "Stockholm", Country = "Sweden", ZipCode = 11111 }
            };

            var storagePlaces = Enumerable.Range(0, 1452).Select(x => new StoragePlace { Id = x, WarehouseId = 1 }).ToArray();

            storagePlaces[1].ArrivalDate = DateTime.Today;
            storagePlaces[1].ProductId = products[0].Id;
            storagePlaces[1].TotalAmount = 30;
            storagePlaces[1].Vacant = false;
            storagePlaces[2].ArrivalDate = DateTime.Today;
            storagePlaces[2].ProductId = products[0].Id;
            storagePlaces[2].TotalAmount = 30;
            storagePlaces[2].Vacant = false;
            storagePlaces[3].ArrivalDate = DateTime.Today;
            storagePlaces[3].ProductId = products[0].Id;
            storagePlaces[3].TotalAmount = 20;
            storagePlaces[3].Vacant = false;
            storagePlaces[4].ArrivalDate = DateTime.Today;
            storagePlaces[4].ProductId = products[0].Id;
            storagePlaces[4].TotalAmount = 20;
            storagePlaces[4].Vacant = false;
            storagePlaces[5].ArrivalDate = DateTime.Today;
            storagePlaces[5].ProductId = products[1].Id;
            storagePlaces[5].TotalAmount = 100;
            storagePlaces[5].Vacant = false;
            storagePlaces[6].ArrivalDate = DateTime.Today;
            storagePlaces[6].ProductId = products[1].Id;
            storagePlaces[6].TotalAmount = 100;
            storagePlaces[6].Vacant = false;
            storagePlaces[7].ArrivalDate = DateTime.Today;
            storagePlaces[7].ProductId = products[1].Id;
            storagePlaces[7].TotalAmount = 100;
            storagePlaces[7].Vacant = false;
            storagePlaces[8].ArrivalDate = DateTime.Today;
            storagePlaces[8].ProductId = products[2].Id;
            storagePlaces[8].TotalAmount = 300;
            storagePlaces[8].Vacant = false;
            storagePlaces[9].ArrivalDate = DateTime.Today;
            storagePlaces[9].ProductId = products[3].Id;
            storagePlaces[9].TotalAmount = 300;
            storagePlaces[9].Vacant = false;
            storagePlaces[10].ArrivalDate = DateTime.Today;
            storagePlaces[10].ProductId = products[4].Id;
            storagePlaces[10].TotalAmount = 200;
            storagePlaces[10].Vacant = false;
            storagePlaces[11].ArrivalDate = DateTime.Today;
            storagePlaces[11].ProductId = products[5].Id;
            storagePlaces[11].TotalAmount = 100;
            storagePlaces[11].Vacant = false;
            storagePlaces[12].ArrivalDate = DateTime.Today;
            storagePlaces[12].ProductId = products[5].Id;
            storagePlaces[12].TotalAmount = 100;
            storagePlaces[12].Vacant = false;
            storagePlaces[13].ArrivalDate = DateTime.Today;
            storagePlaces[13].ProductId = products[6].Id;
            storagePlaces[13].TotalAmount = 600;
            storagePlaces[13].Vacant = false;


            var customergroup = new CustomerGroup[]
            {
                new CustomerGroup {Id = 1, Name = "Livsmedelbutik"},
                new CustomerGroup {Id = 2, Name = "H�gskola"},
                new CustomerGroup {Id = 3, Name = "Restaurang"},
            };

            var ca = new CustomerAddress[]
            {
                new CustomerAddress { Id = 1, CustomerId =1, AddressId = 1, AddressType = AddressType.Leverans },
                new CustomerAddress { Id = 2, CustomerId =1, AddressId = 2, AddressType = AddressType.Leverans },
                new CustomerAddress { Id = 3, CustomerId =1, AddressId = 3, AddressType = AddressType.Faktura },
                new CustomerAddress { Id = 4, CustomerId =1, AddressId = 3, AddressType = AddressType.Bes�k },
                new CustomerAddress { Id = 5, CustomerId =2, AddressId = 4, AddressType = AddressType.Leverans },
                new CustomerAddress { Id = 6, CustomerId =2, AddressId = 4, AddressType = AddressType.Faktura },
                new CustomerAddress { Id = 7, CustomerId =3, AddressId = 5, AddressType = AddressType.Leverans },
                new CustomerAddress { Id = 8, CustomerId =3, AddressId = 5, AddressType = AddressType.Faktura },
                new CustomerAddress { Id = 9, CustomerId =4, AddressId = 6, AddressType = AddressType.Leverans },
                new CustomerAddress { Id = 10, CustomerId =4, AddressId = 6, AddressType = AddressType.Faktura },
            };

            var priceLists = new PriceList[]
            {
                new PriceList { Id = 1, CustomerGroupId = 1, ProductId = 1, Price = 20, RebatePerPallet = 2, StartDate = DateTime.Parse("01/01/2017") },
                new PriceList { Id = 2, CustomerGroupId = 1, ProductId = 2, Price = 20, RebatePerPallet = 3, StartDate = DateTime.Parse("01/01/2017") },
                new PriceList { Id = 3, CustomerGroupId = 1, ProductId = 3, Price = 20, RebatePerPallet = 4, StartDate = DateTime.Parse("01/01/2017") },
                new PriceList { Id = 4, CustomerGroupId = 1, ProductId = 4, Price = 20, RebatePerPallet = 5, StartDate = DateTime.Parse("01/01/2017") },
                new PriceList { Id = 5, CustomerGroupId = 1, ProductId = 5, Price = 68, RebatePerPallet = 3, StartDate = DateTime.Parse("01/01/2017") },
                new PriceList { Id = 6, CustomerGroupId = 1, ProductId = 7, Price = 11.90f, RebatePerPallet = 2, StartDate = DateTime.Parse("01/01/2017") },
            };

            var licenses = new AlcoholLicense[]
            {
                new AlcoholLicense {Id = 1, CustomerId = 1, StartDate = DateTime.Parse("2016-01-01"), EndDate = DateTime.Parse("2016-12-31") },
                new AlcoholLicense {Id = 2, CustomerId = 1, StartDate = DateTime.Parse("2017-01-01"), EndDate = DateTime.Parse("2017-12-31") },
                new AlcoholLicense {Id = 3, CustomerId = 2, StartDate = DateTime.Parse("2017-01-01"), EndDate = DateTime.Parse("2017-12-31") },
            };

            context.Warehouses.AddOrUpdate(warehouses);
            context.Customers.AddOrUpdate(customers);
            context.StoragePlaces.AddOrUpdate(x => x.Id, storagePlaces);
            context.Addresses.AddOrUpdate(addresses);
            context.Products.AddOrUpdate(products);
            context.CustomerAddresses.AddOrUpdate(ca);
            context.CustomerGroups.AddOrUpdate(customergroup);
            context.PriceLists.AddOrUpdate(priceLists);
            context.AlcoholLicense.AddOrUpdate(licenses);
        }
    }
}
