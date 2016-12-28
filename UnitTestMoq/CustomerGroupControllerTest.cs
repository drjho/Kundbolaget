using System.Linq;
using Moq;
using System.Data.Entity;
using System.Web.Mvc;
using Kundbolaget.Controllers;
using Kundbolaget.EntityFramework.Context;
using Kundbolaget.Models.EntityModels;
using NUnit.Framework;
using Kundbolaget.EntityFramework.Repositories;

using Assert = NUnit.Framework.Assert;

namespace UnitTestMoq
{
    [TestFixture]
    internal class CustomerGroupControllerTest
    {
        private Mock<StoreContext> _mockContext;

        private Mock<DbSet<CustomerGroup>> _mockSetCustomerGroup;

        private CustomerGroupController _customerGroupController;

        [SetUp]
        public void Initializer()
        {
            _mockContext = new Mock<StoreContext>();

            _mockSetCustomerGroup = new Mock<DbSet<CustomerGroup>>();

            var dataCustomerGroup = ResourceData.CustomerGroups.AsQueryable();

            var setupDbCustomerGroup = Helper.SetupDb(_mockSetCustomerGroup, dataCustomerGroup);

            _mockContext.Setup(cg => cg.CustomerGroups).Returns(setupDbCustomerGroup.Object);

            var dbCustomerGroupRepository = new DbCustomerGroupRepository(_mockContext.Object);

            _customerGroupController = new CustomerGroupController(dbCustomerGroupRepository);
        }

        [Test]
        public void Create_CustomerGroup()
        {
            var customergroup = new CustomerGroup
            {
                Id = 1,
                Name = "Coop"          
            };
            _customerGroupController.Create(customergroup);
            _mockSetCustomerGroup.Verify(x => x.Add(customergroup), Times.Once);
            _mockContext.Verify(x => x.SaveChanges(), Times.Once);
        }

        [Test]
        public void Delete_CustomerGroup()
        {
            var actionResult = _customerGroupController.Delete(1);
            var viewResult = actionResult as ViewResult;
            var customerGroup = (CustomerGroup) viewResult.Model;
            Assert.AreEqual(1, customerGroup.Id);
        }

        [Test]
        public void Edit_CustomerGroup()
        {
            var actionResult = _customerGroupController.Edit(1);
            var viewResult = actionResult as ViewResult;
            var customerGroup = (CustomerGroup)viewResult.Model;
            Assert.AreEqual(1, customerGroup.Id);
        }

        [Test]
        public void Details_CustomerGroup()
        {
            var actionResult = _customerGroupController.Details(1);
            var viewResult = actionResult as ViewResult;
            var customerGroup = (CustomerGroup)viewResult.Model;
            Assert.AreEqual(1, customerGroup.Id);
        }

        [Test]
        public void Create_Post_Redirect_To_Index()
        {
            var result = _customerGroupController.Create(new CustomerGroup
            {
                Id = 3,
                Name = "Coop"
            }
                ) as RedirectToRouteResult;
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [Test]
        public void Index_Retrieve_All_Data()
        {
            var actionResult = _customerGroupController.Index();
            var viewResult = actionResult as ViewResult;
            var viewResultModel = (CustomerGroup[])viewResult.Model;
            var customerGroupInfo = viewResultModel.ToList();

            Assert.AreEqual(1, customerGroupInfo.Count);
        }
    }
}
