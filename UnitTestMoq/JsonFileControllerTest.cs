using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Kundbolaget.Controllers;
using Kundbolaget.EntityFramework.Context;
using Kundbolaget.EntityFramework.Repositories;
using Kundbolaget.Models.EntityModels;
using NUnit.Framework;
using Moq;
using Kundbolaget.Models.ViewModels;
using System.IO;

namespace UnitTestMoq
{
    [TestFixture]
    internal class JsonFileControllerTest
    {
        //Fake Context
        private Mock<StoreContext> _mockContext;

        //Fake DbSet
        private Mock<DbSet<StoragePlace>> _mockSetStoragePlace;
        private Mock<DbSet<Order>> _mockSetOrder;
        private Mock<DbSet<CustomerAddress>> _mockSetCustomerAddress;
        private Mock<DbSet<OrderProduct>> _mockSetOrderProduct;

        //Fake Controller
        private JsonFileController _jsonFileController;



        [SetUp]
        public void Initializer()
        {
            //New up everytime the test runs
            _mockContext = new Mock<StoreContext>();
            _mockSetStoragePlace = new Mock<DbSet<StoragePlace>>();
            _mockSetOrder = new Mock<DbSet<Order>>();
            _mockSetCustomerAddress = new Mock<DbSet<CustomerAddress>>();
            _mockSetOrderProduct = new Mock<DbSet<OrderProduct>>();

            //Add data
            var dataStoragePlace = ResourceData.StoragePlaces.AsQueryable();
            var dataOrder = ResourceData.Orders.AsQueryable();
            var dataCustomerAddress = ResourceData.CustomerAddresses.AsQueryable();
            var dataProduct = ResourceData.OrderProducts.AsQueryable();

            //Setup behavior
            var setupDbSp = Helper.SetupDb(_mockSetStoragePlace, dataStoragePlace);
            var setupDbOr = Helper.SetupDb(_mockSetOrder, dataOrder);
            var setupDbCA = Helper.SetupDb(_mockSetCustomerAddress, dataCustomerAddress);
            var setupDbOP = Helper.SetupDb(_mockSetOrderProduct, dataProduct);

            _mockContext.Setup(x => x.StoragePlaces).Returns(setupDbSp.Object);
            _mockContext.Setup(x => x.Orders).Returns(setupDbOr.Object);
            _mockContext.Setup(x => x.CustomerAddresses).Returns(setupDbCA.Object);
            _mockContext.Setup(x => x.OrderProducts).Returns(setupDbOP.Object);

            //This will make the mock version of the db approve any string given to the include method.
            //Without this you will get null reference exception when calling include.
            _mockSetStoragePlace.Setup(x => x.Include(It.IsAny<string>())).Returns(_mockSetStoragePlace.Object);
            _mockSetOrder.Setup(x => x.Include(It.IsAny<string>())).Returns(_mockSetOrder.Object);
            _mockSetCustomerAddress.Setup(x => x.Include(It.IsAny<string>())).Returns(_mockSetCustomerAddress.Object);
            _mockSetOrderProduct.Setup(x => x.Include(It.IsAny<string>())).Returns(_mockSetOrderProduct.Object);

            //Inject mock data via overload constructor
            var dbStoragePlaceRepository = new DbStoragePlaceRepository(_mockContext.Object);
            var dbOrderRepository = new DbOrderRepository(_mockContext.Object);
            var dbCustomerAddressRepository = new DbCustomerAddressRepository(_mockContext.Object);
            var dbProductRepository = new DbProductRepository(_mockContext.Object);

            //Setup fakerepo via overloaded constructor
            _jsonFileController = new JsonFileController(dbStoragePlaceRepository, dbOrderRepository, dbCustomerAddressRepository, dbProductRepository);
        }

        [Test]
        public void View_UploadJson_File_Is_Null()
        {
            //Arrange
            var testVM = new OrderUploadVM
            {
                File = null
            };

            // Act
            var result = _jsonFileController.UploadJson(testVM) as ViewResult;

            // Assert
            Assert.AreEqual("", result.ViewName);
        }

        [Test]
        public void View_UploadJson_File_Is_Valid()
        {
            // Arrange
            
            string filePath = System.IO.Path.GetFullPath(@"C:\Users\m97_j\OneDrive\Dokument\Agil2\Demo\Kundbolaget\Kundbolaget\Data\test_beta.json");
            System.IO.FileStream fileStream = new System.IO.FileStream(filePath, System.IO.FileMode.Open);
            Mock<System.Web.HttpPostedFileBase> mockFile = new Mock<System.Web.HttpPostedFileBase>();

            mockFile.Setup(f => f.FileName).Returns("test_beta.json");

            mockFile.Setup(f => f.InputStream).Returns(fileStream);

            var testVM = new OrderUploadVM
            {
                File = mockFile.Object
            };
        
            // Act
            var redirectResult = _jsonFileController.UploadJson(testVM) as RedirectResult;

            // Assert
            Assert.That(redirectResult.Url, Is.EqualTo("/Orders/Index"));
            _mockSetOrder.Verify(x => x.Add(It.IsIn<Order>()), Times.Once);
            _mockSetOrderProduct.Verify(x => x.Add(It.IsIn<OrderProduct>()), Times.AtLeastOnce);
            _mockContext.Verify(x => x.SaveChanges(), Times.Once);
        }
    }
}