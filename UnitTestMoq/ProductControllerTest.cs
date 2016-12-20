
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using Kundbolaget.Controllers;
using Kundbolaget.EntityFramework.Context;
using Kundbolaget.Models.EntityModels;
using NUnit.Framework;
using Kundbolaget.EntityFramework.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using Assert = NUnit.Framework.Assert;
using System.Web.Routing;

namespace UnitTestMoq
{
    [TestFixture]
    //[TestClass]
    internal class ProductControllerTest
    {
        private Mock<StoreContext> _mockContext;

        private Mock<DbSet<Product>> _mockSetProduct;
        private Mock<DbSet<StoragePlace>> _mockSetStoragePlace;
        private Mock<DbSet<Warehouse>> _mockSetWarehouse;

        private ProductsController _productController;

        [SetUp]
        public void Initializer()
        {
            _mockContext = new Mock<StoreContext>();

            _mockSetProduct = new Mock<DbSet<Product>>();
            _mockSetStoragePlace = new Mock<DbSet<StoragePlace>>();
            _mockSetWarehouse = new Mock<DbSet<Warehouse>>();

            var dataProduct = ResourceData.Products.AsQueryable();
            var dataStoragePlace = ResourceData.StoragePlaces.AsQueryable();
            var dataWarehouse = ResourceData.Warehouses.AsQueryable();

            var setupDbProduct = Helper.SetupDb(_mockSetProduct, dataProduct);
            var setupDbStorageplace = Helper.SetupDb(_mockSetStoragePlace, dataStoragePlace);
            var setupDbWarehouse = Helper.SetupDb(_mockSetWarehouse, dataWarehouse);

            _mockContext.Setup(p => p.Products).Returns(setupDbProduct.Object);
            _mockContext.Setup(s => s.StoragePlaces).Returns(setupDbStorageplace.Object);
            _mockContext.Setup(w => w.Warehouses).Returns(setupDbWarehouse.Object);
            // _mockSetProduct.Setup(x => x.Include(It.IsAny<String>())).Returns(_mockSetProduct.Object);

            //TODO: Ändra DbStoreRepository till DbProduct när vi fått in den mergen i Develop
            var dbProductRepository = new DbStoreRepository(_mockContext.Object);
            var dbStorageplaceRepository = new DbStoragePlaceRepository(_mockContext.Object);
            var dbWarehouseRepository = new DbWarehouseRepository(_mockContext.Object);

            _productController = new ProductsController (dbStorageplaceRepository, dbProductRepository, dbWarehouseRepository);
        }

        [Test]
        public void Create()
        {
            var product = new Product
            {
                Id = 10,
                Name = "Sleepy Bulldog",
                ConsumerPackage = ConsumerPackage.Flaska,
                Volume = 33,
                StoragePackage = StoragePackage.Back,
                Alcohol = 12,
                ConsumerPerStorage = 6,
                ProductGroup = ProductGroup.Öl,
                AuditCode = 1,
                VatCode = 32
            };
            _productController.Create(product);
            _mockSetProduct.Verify(x => x.Add(product), Times.Once);
            _mockContext.Verify(x => x.SaveChanges(), Times.Once);
        }

        [Test]
        public void Delete_Get_Object()
        {
            var actionResult = _productController.Delete(1);
            var viewResult = actionResult as ViewResult;
            var product = (Product)viewResult.Model;
            Assert.AreEqual(1, product.Id);
        }

        //[Test]
        //public void Delete_Post_Redirect_To_Index()
        //{
        //    var result = _productController.Delete(1, ResourceData.Products[0]) as RedirectResult;
        //    Assert.AreEqual("Index", result.RouteValues["action"]);
        //}

        [Test]
        public void Create_Post_Redirect_To_Index()
        {
            var result = _productController.Create(new Product
                {
                    Id = 10,
                    Name = "Sleepy Bulldog",
                    ConsumerPackage = ConsumerPackage.Flaska,
                    Volume = 33,
                    StoragePackage = StoragePackage.Back,
                    Alcohol = 12,
                    ConsumerPerStorage = 6,
                    ProductGroup = ProductGroup.Öl,
                    AuditCode = 1,
                    VatCode = 32
                }
            ) as RedirectToRouteResult;
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        //[Test] //TODO: Ska vi ha en IsRemoved prop?
        //public void Delete_Change_IsRemoved()
        //{
        //    var product = _mockSetProduct.Object.First(x => x.Id == 1);
        //    _productController.Delete(product.Id, product);
        //    var result = _mockSetProduct.Object.First(x => x.Id == product.Id);
        //    Assert.AreEqual(true, result.IsRemoved); //En bool i Product Model som visar om en produkt är bortagen eller inte.
        //    _mockContext.Verify(x => x.SaveChanges(), Times.Once);
        //}


    }
}

