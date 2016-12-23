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
        public void Delete_Get_Object()
        {
            var expectedId = 1;

            // Act
            var actionResult = _customerAddressController.Delete(expectedId);
            var viewResult = actionResult as ViewResult;
            var actual = (CustomerAddress)viewResult.Model;

            // Assert
            Assert.AreEqual(expectedId, actual.Id);
        }

        [Test]
        public void Delete()
        {
            // Arrange
            var testObject = ResourceData.CustomerAddresses.First();
            var expectedCount = ResourceData.CustomerAddresses.Count - 1;

            // Act
            _customerAddressController.Delete(testObject.Id, testObject);

            // Assert
            _mockSetCustomerAddress.Verify(x => x.Remove(testObject), Times.Once);
            _mockContext.Verify(x => x.SaveChanges(), Times.Once);

            

        }
    }
}
