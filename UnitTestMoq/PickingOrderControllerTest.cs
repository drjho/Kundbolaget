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
    internal class PickingOrderControllerTest
    {
        //Fake Context
        private Mock<StoreContext> _mockContext;

        //Fake DbSet
        private Mock<DbSet<StoragePlace>> _mockSetStoragePlace;
        private Mock<DbSet<Order>> _mockSetOrder;
        private Mock<DbSet<CustomerAddress>> _mockSetCustomerAddress;
        private Mock<DbSet<OrderProduct>> _mockSetOrderProduct;
        private Mock<DbSet<Customer>> _mockSetCustomer;
        private Mock<DbSet<Address>> _mockSetAddress;
        private Mock<DbSet<PickingOrder>> _mockSetPickingOrder;

        //Fake Controller
        private PickingOrdersController _controller;


        [SetUp]
        public void Initializer()
        {
            //New up everytime the test runs
            _mockContext = new Mock<StoreContext>();
            _mockSetStoragePlace = new Mock<DbSet<StoragePlace>>();
            _mockSetOrder = new Mock<DbSet<Order>>();
            _mockSetCustomerAddress = new Mock<DbSet<CustomerAddress>>();
            _mockSetOrderProduct = new Mock<DbSet<OrderProduct>>();
            _mockSetCustomer = new Mock<DbSet<Customer>>();
            _mockSetAddress = new Mock<DbSet<Address>>();
            _mockSetPickingOrder = new Mock<DbSet<PickingOrder>>();

            //Add data
            var dataStoragePlace = ResourceData.StoragePlaces.AsQueryable();
            var dataOrder = ResourceData.Orders.AsQueryable();
            var dataCustomerAddress = ResourceData.CustomerAddresses.AsQueryable();
            var dataOrderProduct = ResourceData.OrderProducts.AsQueryable();
            var dataCustomer = ResourceData.Customers.AsQueryable();
            var dataAddress = ResourceData.Addresses.AsQueryable();
            var dataPickingOrder = ResourceData.PickingOrders.AsQueryable();

            //Setup behavior
            var setupDbSp = Helper.SetupDb(_mockSetStoragePlace, dataStoragePlace);
            var setupDbOr = Helper.SetupDb(_mockSetOrder, dataOrder);
            var setupDbCA = Helper.SetupDb(_mockSetCustomerAddress, dataCustomerAddress);
            var setupDbOP = Helper.SetupDb(_mockSetOrderProduct, dataOrderProduct);
            var setupDbCu = Helper.SetupDb(_mockSetCustomer, dataCustomer);
            var setupDbAd = Helper.SetupDb(_mockSetAddress, dataAddress);
            var setupDbPO = Helper.SetupDb(_mockSetPickingOrder, dataPickingOrder);

            _mockContext.Setup(x => x.StoragePlaces).Returns(setupDbSp.Object);
            _mockContext.Setup(x => x.Orders).Returns(setupDbOr.Object);
            _mockContext.Setup(x => x.CustomerAddresses).Returns(setupDbCA.Object);
            _mockContext.Setup(x => x.OrderProducts).Returns(setupDbOP.Object);
            _mockContext.Setup(x => x.Customers).Returns(setupDbCu.Object);
            _mockContext.Setup(x => x.Addresses).Returns(setupDbAd.Object);
            _mockContext.Setup(x => x.PickingOrders).Returns(setupDbPO.Object);


            //This will make the mock version of the db approve any string given to the include method.
            //Without this you will get null reference exception when calling include.
            _mockSetStoragePlace.Setup(x => x.Include(It.IsAny<string>())).Returns(_mockSetStoragePlace.Object);
            _mockSetOrder.Setup(x => x.Include(It.IsAny<string>())).Returns(_mockSetOrder.Object);
            _mockSetCustomerAddress.Setup(x => x.Include(It.IsAny<string>())).Returns(_mockSetCustomerAddress.Object);
            _mockSetOrderProduct.Setup(x => x.Include(It.IsAny<string>())).Returns(_mockSetOrderProduct.Object);
            _mockSetCustomer.Setup(x => x.Include(It.IsAny<string>())).Returns(_mockSetCustomer.Object);
            _mockSetAddress.Setup(x => x.Include(It.IsAny<string>())).Returns(_mockSetAddress.Object);
            _mockSetPickingOrder.Setup(x => x.Include(It.IsAny<string>())).Returns(_mockSetPickingOrder.Object);

            //Inject mock data via overload constructor
            var dbStoragePlaceRepository = new DbStoragePlaceRepository(_mockContext.Object);
            var dbOrderRepository = new DbOrderRepository(_mockContext.Object);
            var dbCustomerAddressRepository = new DbCustomerAddressRepository(_mockContext.Object);
            var dbOrderProductRepository = new DbOrderProductRepository(_mockContext.Object);
            var dbAddressRepository = new DbAddressRepository(_mockContext.Object);
            var dbStorageRepository = new DbStoragePlaceRepository(_mockContext.Object);
            var dbCustomerRepository = new DbCustomerRepository(_mockContext.Object);
            var dbPickingOrderRepository = new DbPickingOrderRepository(_mockContext.Object);

            //Setup fakerepo via overloaded constructor
            _controller = new PickingOrdersController(dbPickingOrderRepository, dbOrderProductRepository, dbStoragePlaceRepository);
        }


        [Test]
        public void Create()
        {
            // Arrange
            var order = new PickingOrder
            {
                Id = 3,
                OrderProductId = 1,
                StoragePlaceId = 1,
                ReservedAmount = 100
            };

            // Act
            _controller.Create(order);

            // Assert
            _mockSetPickingOrder.Verify(x => x.Add(order), Times.Once);
            _mockContext.Verify(x => x.SaveChanges(), Times.Once);
        }

        [Test]
        public void Create_Post_Success_Redirect_To_Index()
        {
            // Arrange
            var order = new PickingOrder
            {
                Id = 3,
                OrderProductId = 1,
                StoragePlaceId = 1,
                ReservedAmount = 100
            };

            // Act
            var actualResult = _controller.Create(order) as RedirectToRouteResult;

            // Assert
            Assert.AreEqual("Index", actualResult.RouteValues["action"]);

        }

        [Test]
        public void Create_Post_Not_Valid_Return_View_Model()
        {
            // Arrange
            var order = new PickingOrder
            {
                Id = 5,
                
                StoragePlaceId = 1,
                ReservedAmount = 100
            };

            // Act
            var actualResult = _controller.Create(order) as ViewResult;
            var actual = (PickingOrder)actualResult.Model;

            // Assert
            Assert.AreEqual(order.Id, actual.Id);
        }

        [Test]
        public void Delete_Get_Object()
        {
            // Arrange
            var expectedId = 1;

            // Act
            var actionResult = _controller.Delete(expectedId);
            var viewResult = actionResult as ViewResult;
            var actual = (PickingOrder)viewResult.Model;

            // Assert
            Assert.AreEqual(expectedId, actual.Id);
        }

        [Test]
        public void View_Delete_With_Null_As_Id()
        {
            var result = _controller.Delete(null) as HttpStatusCodeResult;

            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, (System.Net.HttpStatusCode)result.StatusCode);
        }


        [Test]
        public void View_Delete_Without_Existing_Entity_Return_404_Error()
        {
            var result = _controller.Delete(2000);
            Assert.AreEqual(typeof(HttpNotFoundResult), result.GetType());
        }

        [Test]
        public void View_Edit_With_Null_As_Id()
        {
            int? i = null;
            var result = _controller.Edit(i) as HttpStatusCodeResult;
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, (System.Net.HttpStatusCode)result.StatusCode);
        }


        [Test]
        public void View_Edit_Without_Existing_Entity_Return_404_Error()
        {
            var result = _controller.Edit(2000);
            Assert.AreEqual(typeof(HttpNotFoundResult), result.GetType());
        }


        [Test]
        public void Edit_Get_Object()
        {
            // Act
            var actionResult = _controller.Edit(1);
            var viewResult = actionResult as ViewResult;
            var actualResult = (PickingOrder)viewResult.Model;

            // Assert
            Assert.AreEqual(1, actualResult.Id);
            Assert.AreEqual(ResourceData.PickingOrders[0].StoragePlaceId, actualResult.StoragePlaceId);
            Assert.AreEqual(ResourceData.PickingOrders[0].OrderProductId, actualResult.OrderProductId);
        }

        [Test]
        public void Edit_Update_Db_New_Info_In_Object()
        {
            // Arrange
            var orders = _mockSetPickingOrder.Object.ToArray();
            var testObject = orders[0];
            testObject.StoragePlaceId = 2;

            // Act
            _controller.Edit(testObject);

            // Assert
            Assert.AreEqual(2, orders[0].StoragePlaceId);
            _mockSetPickingOrder.Verify(x => x.Attach(testObject), Times.Once);
            _mockContext.Verify(x => x.SaveChanges(), Times.Once);
        }

        [Test]
        public void Details_Get_Object()
        {
            // Act
            var actionResult = _controller.Details(1);
            var viewResult = actionResult as ViewResult;
            var actual = (PickingOrder)viewResult.Model;

            // Assert
            Assert.AreEqual(ResourceData.PickingOrders[0].StoragePlaceId, actual.StoragePlaceId);
            Assert.AreEqual(1, actual.Id);
            Assert.AreEqual(ResourceData.PickingOrders[0].OrderProductId, actual.OrderProductId);
        }

        [Test]
        public void Index_Retrive_All_Data()
        {
            // Arrange 
            var expectedCount = ResourceData.PickingOrders.Count;

            // Act
            var actionResult = _controller.Index();
            var viewResult = actionResult as ViewResult;
            var viewResultModel = viewResult.Model;
            var actual = viewResultModel as List<PickingOrder>;

            // Assert
            Assert.AreEqual(expectedCount, actual.Count);
        }

        [Test]
        public void DeleteConfirmed_Without_Existing_Entity_Return_404_Error()
        {
            var result = _controller.DeleteConfirmed(2000);
            Assert.AreEqual(typeof(HttpNotFoundResult), result.GetType());
        }

        /// <summary>
        /// Check Remove order using order id and if StoragePlace, PickingOrder is updated.
        /// </summary>
        [Test]
        public void DeleteConfirmed()
        {
            // Act
            var result = _controller.DeleteConfirmed(1);

            // Assert
            _mockSetPickingOrder.Verify(x => x.Remove(It.IsAny<PickingOrder>()), Times.Once);
            _mockContext.Verify(x => x.SaveChanges(), Times.AtLeastOnce);
        }
        
    }
}