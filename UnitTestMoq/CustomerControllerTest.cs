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

        private CustomerController _customerController;

        [SetUp]
        public void Initializer()
        {
            _mockContext = new Mock<StoreContext>();
            _mockSetCustomer = new Mock<DbSet<Customer>>();

            var dataCustomer = ResourceData.Customers.AsQueryable();
            var setupDbCustomer = Helper.SetupDb(_mockSetCustomer, dataCustomer);

            _mockContext.Setup(c => c.Customers).Returns(setupDbCustomer.Object);

            var dbCustomerRepository = new DbCustomerRepository(_mockContext.Object);

            _customerController = new CustomerController(dbCustomerRepository);
        }

        [Test]
        public void Create()
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
    }
}
