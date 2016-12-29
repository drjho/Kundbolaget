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

        public static List<Order> Orders => new List<Order>
        {
            new Order
            {
                Id = 1,
                CustomerId = 1,
                AddressId = 1,
                Comment = "none",
                OrderDate = DateTime.Now,
                DesiredDeliveryDate = DateTime.Today,
                PlannedDeliveryDate = DateTime.Today.AddDays(1),
                OrderStatus = OrderStatus.Behandlar,
                OrderProducts = new List<OrderProduct> { OrderProducts[0] }

            },
            new Order
            {
                Id = 2,
                CustomerId = 2,
                AddressId = 2,
                Comment = "ring Johan",
                OrderDate = DateTime.Now,
                DesiredDeliveryDate = DateTime.Today.AddDays(7),
                PlannedDeliveryDate = DateTime.Today.AddDays(7),
                OrderStatus = OrderStatus.Plockar,
                OrderProducts = new List<OrderProduct> { OrderProducts[1] }
            },
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
                Vacant = false,
                TotalAmount = 1000,
                ReservedAmount = 500
            },
            new StoragePlace
            {
                Id = 2,
                AisleNr = 1,
                ProductId = 2,
                ShelfNr = 1,
                Side = Side.Right,
                Spot = 1,
                Vacant = true,
                TotalAmount = 1000,
                ReservedAmount = 500
            }
        };
        public static List<CustomerAddress> CustomerAddresses => new List<CustomerAddress>
        {
            new CustomerAddress
            {
                Id = 1,
                AddressId = 1,
                CustomerId = 1,
                AddressType = AddressType.Leverans
            },
            new CustomerAddress
            {
                Id = 2,
                AddressId = 2,
                CustomerId = 2,
                AddressType = AddressType.Faktura
            },
            new CustomerAddress
            {
                Id = 3,
                AddressId = 2,
                CustomerId = 2,
                AddressType = AddressType.Leverans
            }
        };

        public static List<PickingOrder> PickingOrders => new List<PickingOrder>
        {
            new PickingOrder
            {
                Id = 1,
                OrderProductId = 1,
                StoragePlaceId = 1,
                ReservedAmount = 100
            },
            new PickingOrder
            {
                Id = 2,
                OrderProductId = 2,
                StoragePlaceId = 2,
                ReservedAmount = 100
            }
        };


        public static List<OrderProduct> OrderProducts = new List<OrderProduct>
        {
            new OrderProduct
            {
                Id = 1,
                OrderedAmount = 100,
                AvailabeAmount = 100,
                OrderId = 1,
                ProductId = 1,
                PickList = new List<PickingOrder> { PickingOrders[0] }
            },
            new OrderProduct
            {
                Id = 2,
                OrderedAmount = 200,
                AvailabeAmount = 100,
                OrderId = 2,
                ProductId = 2,
                PickList = new List<PickingOrder> { PickingOrders[1] }
            }
        };

        public static List<PriceList> PriceLists => new List<PriceList>
        {
            new PriceList
            {
                Id = 1,
                Price = 100,
                ProductId = 3,
                RebatePerPallet = 10,
                StartDate = DateTime.Now
            }
        };

        public static List<AlcoholLicense> AlcoholLicenses => new List<AlcoholLicense>
        {
            new AlcoholLicense
            {
                Id = 1,
                StartDate = DateTime.Today
            }
        };

        public static List<CustomerGroup> CustomerGroups => new List<CustomerGroup>
        {
            new CustomerGroup
            {
                Id = 1,
                Name = "Ica"
            }
        };


    }
}
