using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Kundbolaget.EntityFramework.Context;
using Moq;
using System.Data.Entity;
using Kundbolaget.Models.EntityModels;
using Kundbolaget.Controllers;
using System.Linq;
using Kundbolaget.EntityFramework.Repositories;
using System.Web.Mvc;
using Assert = NUnit.Framework.Assert;

namespace UnitTestMoq
{
    [TestFixture]
    public class CustomerAddressControllerTest
    {
        //Fake Context
        private Mock<StoreContext> _mockContext;

        //Fake DbSet
        private Mock<DbSet<CustomerAddress>> _mockSetCustomerAddress;
        private Mock<DbSet<Customer>> _mockSetCustomer;
        private Mock<DbSet<Address>> _mockSetAddress;

        private CustomerAddressController _customerAddressController;

        [SetUp]
        public void Initializer()
        {
            //New up everytime the test runs
            _mockContext = new Mock<StoreContext>();
            _mockSetCustomerAddress = new Mock<DbSet<CustomerAddress>>();
            _mockSetCustomer = new Mock<DbSet<Customer>>();
            _mockSetAddress = new Mock<DbSet<Address>>();

            //Add data
            var dataCustomerAddress = ResourceData.CustomerAddresses.AsQueryable();
            var dataCustomer = ResourceData.Customers.AsQueryable();
            var dataAddress = ResourceData.Addresses.AsQueryable();

            //Setup behavior
            var setupDbCA = Helper.SetupDb(_mockSetCustomerAddress, dataCustomerAddress);
            var setupDbCu = Helper.SetupDb(_mockSetCustomer, dataCustomer);
            var setupDbAd = Helper.SetupDb(_mockSetAddress, dataAddress);

            _mockContext.Setup(x => x.CustomerAddresses).Returns(setupDbCA.Object);
            _mockContext.Setup(x => x.Customers).Returns(setupDbCu.Object);
            _mockContext.Setup(x => x.Addresses).Returns(setupDbAd.Object);

            
            //This will make the mock version of the db approve any string given to the include method.
            //Without this you will get null reference exception when calling include.
            _mockSetCustomerAddress.Setup(x => x.Include(It.IsAny<string>())).Returns(_mockSetCustomerAddress.Object);
            _mockSetCustomer.Setup(x => x.Include(It.IsAny<string>())).Returns(_mockSetCustomer.Object);
            _mockSetAddress.Setup(x => x.Include(It.IsAny<string>())).Returns(_mockSetAddress.Object);

            //Inject mock data via overload constructor
            var dbCustomerAddressRepository = new DbCustomerAddressRepository(_mockContext.Object);
            var dbCustomerRepository = new DbCustomerRepository(_mockContext.Object);
            var dbAddressRepository = new DbAddressRepository(_mockContext.Object);

            //Setup fakerepo via overloaded constructor
            _customerAddressController= new CustomerAddressController(dbCustomerAddressRepository, dbCustomerRepository, dbAddressRepository);

            
        }

        [Test]
        public void Create()
        {
            // Arrange
            var testObject = new CustomerAddress
            {
                Id = 3,
                CustomerId = 1,
                AddressId = 1,
                AddressType = AddressType.Faktura
            };

            // Act
            _customerAddressController.Create(testObject);

            // Assert
            _mockSetCustomerAddress.Verify(x => x.Add(testObject), Times.Once);
            _mockContext.Verify(x => x.SaveChanges(), Times.Once);

            //Funderingar:  borde man kolla om count är +1.
            //              borde man testa med ett "dåligt" objekt?
        }

        [Test]
        public void Create_Post_Redirect_To_Index()
        {
            // Arrange
            var testObject = new CustomerAddress
            {
                Id = 3,
                CustomerId = 1,
                AddressId = 1,
                AddressType = AddressType.Faktura
            };
            
            // Act
            var actualResult = _customerAddressController.Create(testObject) as RedirectToRouteResult;

            // Assert
            Assert.AreEqual("Index", actualResult.RouteValues["action"]);
        }

        [Test]
        public void Delete_Get_Object()
        {
            // Arrange
            var expectedId = 1;

            // Act
            var actionResult = _customerAddressController.Delete(expectedId);
            var viewResult = actionResult as ViewResult;
            var actual = (CustomerAddress)viewResult.Model;

            // Assert
            Assert.AreEqual(expectedId, actual.Id);
        }

        [Test]
        public void Delete_Post_Redirect_To_Index()
        {
            // Act
            var actualResult = _customerAddressController.Delete(1, ResourceData.CustomerAddresses[0]) as RedirectToRouteResult;
            
            // Assert
            Assert.AreEqual("Index", actualResult.RouteValues["action"]);
        }

        [Test]
        public void Delete()
        {
            // Arrange
            _mockSetCustomerAddress.Setup(x => x.Remove(It.IsAny<CustomerAddress>()));
            var testObject = ResourceData.CustomerAddresses.First();

            // Act
            _customerAddressController.Delete(testObject.Id, testObject);

            // Assert
            //_mockSetCustomerAddress.Verify(x => x.Remove(testObject), Times.Once);
            _mockSetCustomerAddress.Verify(x => x.Remove(It.IsAny<CustomerAddress>()), Times.Once);
            _mockContext.Verify(x => x.SaveChanges(), Times.Once);
        }

        [Test]
        public void Edit_Get_Object()
        {
            // Act
            var actionResult = _customerAddressController.Edit(1);
            var viewResult = actionResult as ViewResult;
            var actualResult = (CustomerAddress)viewResult.Model;

            // Assert
            Assert.AreEqual(1, actualResult.Id);
            Assert.AreEqual(ResourceData.CustomerAddresses[0].AddressId, actualResult.AddressId);
            Assert.AreEqual(ResourceData.CustomerAddresses[0].CustomerId, actualResult.CustomerId);
            Assert.AreEqual(ResourceData.CustomerAddresses[0].AddressType, actualResult.AddressType);
        }

        [Test]
        public void Edit_Update_Db_New_Info_In_Object()
        {
            // Arrange
            var customAddresses = _mockSetCustomerAddress.Object.ToArray();
            var testObject = customAddresses[0];
            testObject.AddressId = 2;

            // Act
            _customerAddressController.Edit(testObject);

            // Assert
            Assert.AreEqual(2, customAddresses[0].AddressId);
            _mockSetCustomerAddress.Verify(x => x.Attach(testObject), Times.Once);
            _mockContext.Verify(x => x.SaveChanges(), Times.Once);
        }

        [Test]
        public void Details_Get_Object()
        {
            // Act
            var actionResult = _customerAddressController.Details(1);
            var viewResult = actionResult as ViewResult;
            var actual = (CustomerAddress)viewResult.Model;

            // Assert
            Assert.AreEqual(ResourceData.CustomerAddresses[0].AddressId, actual.AddressId);
            Assert.AreEqual(1, actual.Id);
            Assert.AreEqual(ResourceData.CustomerAddresses[0].CustomerId, actual.CustomerId);
        }

        [Test]
        public void Index_Retrive_All_Data()
        {
            // Arrange 
            var expectedCount = ResourceData.CustomerAddresses.Count;
            
            // Act
            var actionResult = _customerAddressController.Index();
            var viewResult = actionResult as ViewResult;
            var viewResultModel = (CustomerAddress[])viewResult.Model;
            var actual = viewResultModel.ToList();

            // Assert
            Assert.AreEqual(expectedCount, actual.Count);
        }

        [Test]
        public void View_Delete_Without_Existing_Entity_Return_404_Error()
        {
            var actual = _customerAddressController.Delete(2000);
            Assert.AreEqual(typeof(HttpNotFoundResult), actual.GetType());
        }

        [Test]
        public void View_Details_Without_Existing_Entity_Return_404_Error()
        {
            var actual = _customerAddressController.Details(2000);
            Assert.AreEqual(typeof(HttpNotFoundResult), actual.GetType());
        }

        [Test]
        public void View_Edit_Without_Existing_Entity_Return_404_Error()
        {
            var actual = _customerAddressController.Edit(2000);
            Assert.AreEqual(typeof(HttpNotFoundResult), actual.GetType());
        }
    }
}
