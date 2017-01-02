using System;
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

        [Test]
        public void Create_AlcoholLicense()
        {
            var alcoholLicense = new AlcoholLicense
            {
                Id = 1,
                StartDate = DateTime.Now,
                EndDate = DateTime.MaxValue,
            };
            _alcoholLicenseController.Create(alcoholLicense);
            _mockSetAlcoholLicense.Verify(x => x.Add(alcoholLicense), Times.Once);
            _mockContext.Verify(x => x.SaveChanges(), Times.Once);
        }

        [Test]
        public void Delete_AlcoholLicense()
        {
            var actionResult = _alcoholLicenseController.Delete(1);
            var viewResult = actionResult as ViewResult;
            var alcoholLicense = (AlcoholLicense) viewResult.Model;
            Assert.AreEqual(1, alcoholLicense.Id);
        }
        [Test]
        public void Edit_AlcoholLicense()
        {
            var actionResult = _alcoholLicenseController.Edit(1);
            var viewResult = actionResult as ViewResult;
            var alcoholLicense = (AlcoholLicense)viewResult.Model;
            Assert.AreEqual(1, alcoholLicense.Id);
        }

        [Test]
        public void Details_AlcoholLicense()
        {
            var actionResult = _alcoholLicenseController.Details(1);
            var viewResult = actionResult as ViewResult;
            var priceList = (AlcoholLicense)viewResult.Model;
            Assert.AreEqual(1, priceList.Id);
        }
        [Test]
        public void Create_Post_Redirect_To_Index()
        {
            var result = _alcoholLicenseController.Create(new AlcoholLicense
            {
                Id = 3,
                StartDate = DateTime.Now,
                EndDate = DateTime.MaxValue
            }
                ) as RedirectToRouteResult;
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [Test]
        public void Index_Retrieve_All_Data()
        {
            var actionResult = _alcoholLicenseController.Index();
            var viewResult = actionResult as ViewResult;
            var viewResultModel = (AlcoholLicense[])viewResult.Model;
            var AlcoholLicenseInfo = viewResultModel.ToList();

            Assert.AreEqual(1, AlcoholLicenseInfo.Count);
        }
    }
}
