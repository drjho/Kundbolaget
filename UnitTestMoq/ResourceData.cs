using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kundbolaget.Models.EntityModels;

namespace UnitTestMoq
{
    internal static class ResourceData
    {
        public static List<Product> Products => new List<Product>
        {
            new Product
            {
                Id = 1,
                ConsumerPackage = ConsumerPackage.Flaska,
                Alcohol = 5,
                AuditCode = 123,
                ConsumerPerStorage = 12,
                Name = "Carlsberg",
                ProductGroup = ProductGroup.Öl,
                StoragePackage = StoragePackage.Flak,
            },
            new Product
            {
                Id = 2,
                ConsumerPackage = ConsumerPackage.Burk,
                Alcohol = 4,
                AuditCode = 234,
                ConsumerPerStorage = 24,
                Name = "Rekorderlig",
                ProductGroup = ProductGroup.Cider,
                StoragePackage = StoragePackage.Flak,
            }
        };

        public static List<Customer> Customers => new List<Customer>
        {
            new Customer
            {
                Id = 1,
                Name = "Ica Sollentuna",
                CorporateStucture = "Ica",
                CreditLimit = 10000,
                CustomerAuditCode = 1,
                DaysToDelievery = 12,
                OrganisationNumber = "100"
            },
            new Customer
            {
                Id = 2,
                Name = "Ica Solna",
                CorporateStucture = "Ica",
                CreditLimit = 10000,
                CustomerAuditCode = 1,
                DaysToDelievery = 12,
                OrganisationNumber = "100"
            },
            new Customer
            {
                Id = 3,
                Name = "Coop Häggvik",
                CorporateStucture = "Coop",
                CreditLimit = 20000,
                CustomerAuditCode = 2,
                DaysToDelievery = 18,
                OrganisationNumber = "200"
            }
        };

        public static List<Warehouse> Warehouses => new List<Warehouse>
        {
            new Warehouse
            {
                Id = 1,
                Name = "Årsta",
                City = "Stockholm",
                Country = "Sverige",
                ZipCode = 11111
            },
             new Warehouse
            {
                Id = 2,
                Name = "Rosersberg",
                City = "Stockholm",
                Country = "Sverige",
                ZipCode = 11112
            }
        };
        public static List<Address> Addresses => new List<Address>
        {
            new Address
            {
                Id = 1,
                StreetName = "Kungsgatan",
                Number = 1,
                Area = "Stockholm",
                PostalCode = "11111",
                Country = "Sweden"
            },
            new Address
            {
                Id = 2,
                StreetName = "Kungsgatan",
                Number = 2,
                Area = "Stockholm",
                PostalCode = "11111",
                Country = "Sweden"
            }
        };
        public static List<StoragePlace> StoragePlaces => new List<StoragePlace>
        {
            new StoragePlace
            {
                Id = 1,
                AisleNr = 1,
                ProductId = 1,
                ShelfNr = 1,
                Side = Side.Left,
                Spot = 1,
                Vacant = false          
            },
            new StoragePlace
            {
                Id = 2,
                AisleNr = 1,
                ProductId = 2,
                ShelfNr = 1,
                Side = Side.Right,
                Spot = 1,
                Vacant = true
            }
        };
    }
}
