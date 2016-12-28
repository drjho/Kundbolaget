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
    }

}
