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
    internal class CustomerControllerTest
    {
        private Mock<StoreContext> _mockContext;

        private Mock<DbSet<Customer>> _mockSetCustomer;
        private Mock<DbSet<CustomerAddress>> _mockSetCustomerAdress;

        private CustomerController _customerController;
        private CustomerAddressController _customerAddressController;

        [SetUp]
        public void Initializer()
        {
            _mockContext = new Mock<StoreContext>();

            _mockSetCustomer = new Mock<DbSet<Customer>>();
            _mockSetCustomerAdress = new Mock<DbSet<CustomerAddress>>();

            var dataCustomer = ResourceData.Customers.AsQueryable();
            var dataCustomerAdress = ResourceData.CustomerAddresses.AsQueryable();

            var setupDbCustomer = Helper.SetupDb(_mockSetCustomer, dataCustomer);
            var setupDbCustomerAdress = Helper.SetupDb(_mockSetCustomerAdress, dataCustomerAdress);

            _mockContext.Setup(c => c.Customers).Returns(setupDbCustomer.Object);
            _mockContext.Setup(ca => ca.CustomerAddresses).Returns(setupDbCustomerAdress.Object);

            var dbCustomerRepository = new DbCustomerRepository(_mockContext.Object);
            var dbCustomerAdressRepository = new DbCustomerAddressRepository(_mockContext.Object);

            _customerController = new CustomerController(dbCustomerRepository, dbCustomerAdressRepository);
            _customerAddressController = new CustomerAddressController(dbCustomerAdressRepository);
            
        }

        [Test]
        public void Create_Customer()
        {
            var customer = new Customer
            {
                Id = 10,
                Name = "Ica Sollentuna",
                CorporateStucture = "Ica",
                CreditLimit = -1,
                CustomerAuditCode = 12,
                OrganisationNumber = "SE0001"
            };
            _customerController.Create(customer);
            _mockSetCustomer.Verify(x => x.Add(customer), Times.Once);
            _mockContext.Verify(x => x.SaveChanges(), Times.Once);
        }
        [Test]
        public void Delete_Customer()
        {
            var actionResult = _customerController.Delete(1);
            var viewResult = actionResult as ViewResult;
            var customer = (Customer) viewResult.Model;
            Assert.AreEqual(1, customer.Id);
        }

        [Test]
        public void Edit_Customer()
        {
            var actioResult = _customerController.Edit(1);
            var viewResult = actioResult as ViewResult;
            var customer = (Customer) viewResult.Model;
            Assert.AreEqual(1, customer.Id);
        }
 
        [Test]
        public void Details_Customer()
        {
            var actionResult = _customerController.Details(1);
            var actionResult2 = _customerAddressController.Details(1);
            var viewResult = actionResult as ViewResult;
            var viewResult2 = actionResult2 as ViewResult;
            var customer = (Customer)viewResult.Model;
            var adress = (CustomerAddress) viewResult2.Model;
            Assert.AreEqual(1, customer.Id);
            Assert.AreEqual(1, adress.Id);
        }

        [Test]
        public void Create_Post_Redirect_To_Index()
        {
            var result = _customerController.Create(new Customer
            {
                Id = 1,
                Name = "Ica Sollentuna",
                CorporateStucture = "Ica",
                CreditLimit = -1,
                CustomerAuditCode = 12,
                OrganisationNumber = "SE0001"
            }
            ) as RedirectToRouteResult;
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [Test]
        public void Index_Retrieve_All_Data()
        {
            var actionResult = _customerController.Index();
            var viewResult = actionResult as ViewResult;
            var viewResultModel = (Customer[]) viewResult.Model;
            var customerInfo = viewResultModel.ToList();

            Assert.AreEqual(3, customerInfo.Count);
        }

        [Test]
        public void Index_Retrieve_All_Data_Fail()
        {
            var actionResult = _customerController.Index();
            var viewResult = actionResult as ViewResult;
            var viewResultModel = (Customer[])viewResult.Model;
            var customerInfo = viewResultModel.ToList();

            Assert.AreNotEqual(1, customerInfo.Count);
        }

    }
}
