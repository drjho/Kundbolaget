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
using Kundbolaget.Migrations;
using Kundbolaget.Models.EntityModels;
using Moq;
using NUnit.Framework;

namespace UnitTestMoq
{
    [TestFixture]
    internal class AddressControllerTest
    {
        private Mock<StoreContext> _mockContext;

        private Mock<DbSet<Address>> _mockSetAddress;
        private AddressController _adressController;

        [SetUp]
        public void Initializer()
        {
            _mockContext = new Mock<StoreContext>();
            _mockSetAddress = new Mock<DbSet<Address>>();

            var dataAddres = ResourceData.Addresses.AsQueryable();

            var setupDbAddress = Helper.SetupDb(_mockSetAddress, dataAddres);

            _mockContext.Setup(a => a.Addresses).Returns(setupDbAddress.Object);
            //_mockSetAddress.Setup(x=> x.Include(It.IsAny<String>())).Returns(_mockSetAddress.Object);

            var dbAddressRepository = new DbAddressRepository(_mockContext.Object);

            _adressController = new AddressController(dbAddressRepository);
        }

        [Test]
        public void Create()
        {
            var address = new Address
            {
                Id = 6,
                StreetName = "Knasgatan",
                Number = 6,
                PostalCode = "66669",
                Area = "Knasköping",
                Country = "Knasland"
            };
            _adressController.Create(address);
            _mockSetAddress.Verify(a => a.Add(address), Times.Once);
            _mockContext.Verify(a => a.SaveChanges(), Times.Once);
        }

        [Test]
        public void Delete_Get_Object()
        {
            var actionResult = _adressController.Delete(1);
            var viewResult = actionResult as ViewResult;
            var address = (Address) viewResult.Model;
            Assert.AreEqual(1, address.Id);
        }

        [Test]
        public void Delete_Post_Redirect_To_Index()
        {
            var result = _adressController.Delete(1, ResourceData.Addresses[0]) as RedirectToRouteResult;
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [Test]
        public void Create_Post_Redirect_To_Index()
        {
            var result = _adressController.Create(new Address
                {
                    Id = 6,
                    StreetName = "Knasgatan",
                    Number = 6,
                    PostalCode = "666 69",
                    Area = "Knasköping",
                    Country = "Knasland"
                }
            ) as RedirectToRouteResult;
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [Test]
        public void Details_Get_Object()
        {
            var actionResult = _adressController.Details(1);
            var viewResult = actionResult as ViewResult;
            var result = (Address) viewResult.Model;

            Assert.AreEqual(ResourceData.Addresses[0].StreetName, result.StreetName);
            Assert.AreEqual(ResourceData.Addresses[0].Area, result.Area);
            Assert.AreEqual(ResourceData.Addresses[0].Id, result.Id);
        }

        [Test]
        public void Edit_Post_Redirect_To_Index()
        {
            var result = _adressController.Edit(ResourceData.Addresses[0]) as RedirectToRouteResult;
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [Test]
        public void Edit_Update_Db_New_Info_In_Objekt()
        {
            var address = _mockSetAddress.Object.ToList();
            var tempObj = address[0];
            tempObj.StreetName = "Knasgatan";
            _adressController.Edit(tempObj);

            Assert.AreEqual("Knasgatan", address[0].StreetName);
            _mockSetAddress.Verify(x => x.Attach(tempObj), Times.Once);
            _mockContext.Verify(x => x.SaveChanges(), Times.Once);
        }

    }
}
