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
    internal class OrderControllerTest
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


        //Fake Controller
        private OrdersController _orderController;


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

            //Add data
            var dataStoragePlace = ResourceData.StoragePlaces.AsQueryable();
            var dataOrder = ResourceData.Orders.AsQueryable();
            var dataCustomerAddress = ResourceData.CustomerAddresses.AsQueryable();
            var dataOrderProduct = ResourceData.OrderProducts.AsQueryable();
            var dataCustomer = ResourceData.Customers.AsQueryable();
            var dataAddress = ResourceData.Addresses.AsQueryable();

            //Setup behavior
            var setupDbSp = Helper.SetupDb(_mockSetStoragePlace, dataStoragePlace);
            var setupDbOr = Helper.SetupDb(_mockSetOrder, dataOrder);
            var setupDbCA = Helper.SetupDb(_mockSetCustomerAddress, dataCustomerAddress);
            var setupDbOP = Helper.SetupDb(_mockSetOrderProduct, dataOrderProduct);
            var setupDbCu = Helper.SetupDb(_mockSetCustomer, dataCustomer);
            var setupDbAd = Helper.SetupDb(_mockSetAddress, dataAddress);


            _mockContext.Setup(x => x.StoragePlaces).Returns(setupDbSp.Object);
            _mockContext.Setup(x => x.Orders).Returns(setupDbOr.Object);
            _mockContext.Setup(x => x.CustomerAddresses).Returns(setupDbCA.Object);
            _mockContext.Setup(x => x.OrderProducts).Returns(setupDbOP.Object);
            _mockContext.Setup(x => x.Customers).Returns(setupDbCu.Object);
            _mockContext.Setup(x => x.Addresses).Returns(setupDbAd.Object);

            //This will make the mock version of the db approve any string given to the include method.
            //Without this you will get null reference exception when calling include.
            _mockSetStoragePlace.Setup(x => x.Include(It.IsAny<string>())).Returns(_mockSetStoragePlace.Object);
            _mockSetOrder.Setup(x => x.Include(It.IsAny<string>())).Returns(_mockSetOrder.Object);
            _mockSetCustomerAddress.Setup(x => x.Include(It.IsAny<string>())).Returns(_mockSetCustomerAddress.Object);
            _mockSetOrderProduct.Setup(x => x.Include(It.IsAny<string>())).Returns(_mockSetOrderProduct.Object);
            _mockSetCustomer.Setup(x => x.Include(It.IsAny<string>())).Returns(_mockSetCustomer.Object);
            _mockSetAddress.Setup(x => x.Include(It.IsAny<string>())).Returns(_mockSetAddress.Object);

            //Inject mock data via overload constructor
            var dbStoragePlaceRepository = new DbStoragePlaceRepository(_mockContext.Object);
            var dbOrderRepository = new DbOrderRepository(_mockContext.Object);
            var dbCustomerAddressRepository = new DbCustomerAddressRepository(_mockContext.Object);
            var dbOrderProductRepository = new DbOrderProductRepository(_mockContext.Object);
            var dbAddressRepository = new DbAddressRepository(_mockContext.Object);
            var dbStorageRepository = new DbStoragePlaceRepository(_mockContext.Object);
            var dbCustomerRepository = new DbCustomerRepository(_mockContext.Object);


            //Setup fakerepo via overloaded constructor
            _orderController = new OrdersController(dbOrderRepository, dbAddressRepository,
                dbStoragePlaceRepository, dbOrderProductRepository, dbCustomerRepository);
        }


        [Test]
        public void Create()
        {
            // Arrange
            var order = new Order
            {
                OrderDate = DateTime.Today,
                CustomerId = 1,
                DesiredDeliveryDate = DateTime.Today,
                AddressId = 1,
                OrderProducts = new List<OrderProduct>
                {
                    new OrderProduct
                    {
                        Id = 1,
                        OrderedAmount = 100,
                        DeliveredAmount = 100,
                        OrderId = 1,
                        ProductId = 1
                    }
                }
            };

            // Act
            _orderController.Create(order);

            // Assert
            _mockSetOrder.Verify(x => x.Add(order), Times.Once);
            _mockContext.Verify(x => x.SaveChanges(), Times.Once);
        }

        [Test]
        public void Create_Post_Redirect_To_Index()
        {
            // Arrange
            var order = new Order
            {
                OrderDate = DateTime.Today,
                CustomerId = 1,
                DesiredDeliveryDate = DateTime.Today,
                AddressId = 1,
                OrderProducts = new List<OrderProduct>
                {
                    new OrderProduct
                    {
                        Id = 1,
                        OrderedAmount = 100,
                        DeliveredAmount = 100,
                        OrderId = 1,
                        ProductId = 1
                    }
                }
            };

            // Act
            var actualResult = _orderController.Create(order) as RedirectToRouteResult;

            // Assert
            Assert.AreEqual("Index", actualResult.RouteValues["action"]);
        }

        [Test]
        public void Create_Post_Not_Valid_Redirect_To_Create()
        {
            // Arrange
            var order = new Order
            {
            };

            // Act
            var actualResult = _orderController.Create(order) as RedirectToRouteResult;

            // Assert
            Assert.AreEqual("Create", actualResult.RouteValues["action"]);
        }

        [Test]
        public void Delete_Get_Object()
        {
            // Arrange
            var expectedId = 1;

            // Act
            var actionResult = _orderController.Delete(expectedId);
            var viewResult = actionResult as ViewResult;
            var actual = (Order)viewResult.Model;

            // Assert
            Assert.AreEqual(expectedId, actual.Id);
        }

        [Test]
        public void View_Delete_With_Null_As_Id()
        {
            var result = _orderController.Delete(null) as HttpStatusCodeResult;
            
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, (System.Net.HttpStatusCode)result.StatusCode);
        }


        [Test]
        public void View_Delete_Without_Existing_Entity_Return_404_Error()
        {
            var result = _orderController.Delete(2000);
            Assert.AreEqual(typeof(HttpNotFoundResult), result.GetType());
        }

        [Test]
        public void View_Edit_With_Null_As_Id()
        {
            int? i = null;
            var result = _orderController.Edit(i) as HttpStatusCodeResult;
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, (System.Net.HttpStatusCode)result.StatusCode);
        }


        [Test]
        public void View_Edit_Without_Existing_Entity_Return_404_Error()
        {
            var result = _orderController.Edit(2000);
            Assert.AreEqual(typeof(HttpNotFoundResult), result.GetType());
        }


        [Test]
        public void Edit_Get_Object()
        {
            // Act
            var actionResult = _orderController.Edit(1);
            var viewResult = actionResult as ViewResult;
            var actualResult = (Order)viewResult.Model;

            // Assert
            Assert.AreEqual(1, actualResult.Id);
            Assert.AreEqual(ResourceData.Orders[0].AddressId, actualResult.AddressId);
            Assert.AreEqual(ResourceData.Orders[0].CustomerId, actualResult.CustomerId);
        }

        [Test]
        public void Edit_Update_Db_New_Info_In_Object()
        {
            // Arrange
            var orders = _mockSetOrder.Object.ToArray();
            var testObject = orders[0];
            testObject.AddressId = 2;

            // Act
            _orderController.Edit(testObject);

            // Assert
            Assert.AreEqual(2, orders[0].AddressId);
            _mockSetOrder.Verify(x => x.Attach(testObject), Times.Once);
            _mockContext.Verify(x => x.SaveChanges(), Times.Once);
        }

        [Test]
        public void Details_Get_Object()
        {
            // Act
            var actionResult = _orderController.Details(1);
            var viewResult = actionResult as ViewResult;
            var actual = (Order)viewResult.Model;

            // Assert
            Assert.AreEqual(ResourceData.Orders[0].AddressId, actual.AddressId);
            Assert.AreEqual(1, actual.Id);
            Assert.AreEqual(ResourceData.Orders[0].CustomerId, actual.CustomerId);
        }

        [Test]
        public void Index_Retrive_All_Data()
        {
            // Arrange 
            var expectedCount = ResourceData.Orders.Count;

            // Act
            var actionResult = _orderController.Index();
            var viewResult = actionResult as ViewResult;
            var viewResultModel = (Order[])viewResult.Model;
            var actual = viewResultModel.ToList();

            // Assert
            Assert.AreEqual(expectedCount, actual.Count);
        }

        [Test]
        public void PrepareOrder_With_Null_As_Id()
        {
            int? i = null;
            var result = _orderController.PrepareOrder(i) as HttpStatusCodeResult;
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, (System.Net.HttpStatusCode)result.StatusCode);
        }


        [Test]
        public void PrepareOrder_Without_Existing_Entity_Return_404_Error()
        {
            var result = _orderController.PrepareOrder(2000);
            Assert.AreEqual(typeof(HttpNotFoundResult), result.GetType());
        }

        [Test]
        public void PrepareOrder_Return_ViewModel()
        {
            var actionResult = _orderController.PrepareOrder(1);
            var viewResult = actionResult as ViewResult;
            var actual = (OrderVM)viewResult.Model;

            // Assert
            Assert.AreEqual(ResourceData.Orders[0].AddressId, actual.AddressId);
            Assert.AreEqual(1, actual.Id);
            Assert.AreEqual(ResourceData.Orders[0].CustomerId, actual.CustomerId);
        }
    }
}