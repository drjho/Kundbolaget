using System.Linq;
using Moq;
using System.Data.Entity;
using System.Web.Mvc;
using Kundbolaget.Controllers;
using Kundbolaget.EntityFramework.Context;
using Kundbolaget.Models.EntityModels;
using NUnit.Framework;
using Kundbolaget.EntityFramework.Repositories;
namespace UnitTestMoq
{
    [TestFixture]
    internal class AlcoholLicenseControllerTest
    {
        private Mock<StoreContext> _mockContext;

        private Mock<DbSet<AlcoholLicense>> _mockSetAlcoholLicense;

        private AlcoholLicenseController _alcoholLicenseController;

        [SetUp]
        public void Initializer()
        {
            _mockContext = new Mock<StoreContext>();

            _mockSetAlcoholLicense = new Mock<DbSet<AlcoholLicense>>();

            var dataAlcoholLicense = ResourceData.AlcoholLicenses.AsQueryable();

            var setupDbAlcoholLicense = Helper.SetupDb(_mockSetAlcoholLicense, dataAlcoholLicense);

            _mockContext.Setup(al => al.AlcoholLicense).Returns(setupDbAlcoholLicense.Object);

            var dbAlcoholLicenseRepository = new DbAlcoholLicenseRepository(_mockContext.Object);

            _alcoholLicenseController = new AlcoholLicenseController(dbAlcoholLicenseRepository);
        }

        
    }
}
