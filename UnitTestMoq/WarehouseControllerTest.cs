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

namespace UnitTestMoq
{
    [TestFixture]
    internal class WarehouseControllerTest
    {
        //Fake Context
        private Mock<StoreContext> _mockContext;

        //Fake DbSet
        private Mock<DbSet<StoragePlace>> _mockSetStoragePlace;
        private Mock<DbSet<Warehouse>> _mockSetWarehouse;

        //Fake Controller
        private WarehouseController _warehouseController;


        [SetUp]
        public void Initializer()
        {
            //New up everytime the test runs
            _mockContext = new Mock<StoreContext>();
            _mockSetStoragePlace = new Mock<DbSet<StoragePlace>>();
            _mockSetWarehouse = new Mock<DbSet<Warehouse>>();

            //Add data
            var dataWarehouse = ResourceData.Warehouses.AsQueryable();
            var dataStoragePlace = ResourceData.StoragePlaces.AsQueryable();

            //Setup behavior
            var setupDbWh = Helper.SetupDb(_mockSetWarehouse, dataWarehouse);
            var setupDbSp = Helper.SetupDb(_mockSetStoragePlace, dataStoragePlace);

            _mockContext.Setup(x => x.Warehouses).Returns(setupDbWh.Object);
            _mockContext.Setup(x => x.StoragePlaces).Returns(setupDbSp.Object);

            //This will make the mock version of the db approve any string given to the include method.
            //Without this you will get null reference exception when calling include.
            _mockSetWarehouse.Setup(x => x.Include(It.IsAny<string>())).Returns(_mockSetWarehouse.Object);

            //Inject mock data via overload constructor
            var dbWarehouseRepository = new DbWarehouseRepository(_mockContext.Object);
            var dbStoragePlaceRepository = new DbStoragePlaceRepository(_mockContext.Object);

            //Setup fakerepo via overloaded constructor
            _warehouseController = new WarehouseController(dbWarehouseRepository, dbStoragePlaceRepository);
        }

        [Test]
        public void Create()
        {
            var warehouse = new Warehouse
            {
                Id = 10,
                City = "Alvik",
                Country = "Sverige",
                Name = "Alvik lager",
                ZipCode = 11233
            };
            _warehouseController.Create(warehouse);
            _mockSetWarehouse.Verify(x => x.Add(warehouse), Times.Once);
            _mockContext.Verify(x => x.SaveChanges(), Times.Once);
        }

        [Test]
        public void Create_Post_Redirect_To_Index()
        {
            var result = _warehouseController.Create(new Warehouse
                {
                    Id = 10,
                    City = "Alvik",
                    Country = "Sverige",
                    Name = "Alvik lager",
                    ZipCode = 11233
                }
            ) as RedirectToRouteResult;
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [Test]
        public void Delete_Get_Object()
        {
            var actionResult = _warehouseController.Delete(1);
            var viewResult = actionResult as ViewResult;
            var warehouse = (Warehouse) viewResult.Model;
            Assert.AreEqual(1, warehouse.Id);
        }
    }
}
