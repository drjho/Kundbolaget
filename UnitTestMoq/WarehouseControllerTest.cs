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
            _mockSetStoragePlace.Setup(x => x.Include(It.IsAny<string>())).Returns(_mockSetStoragePlace.Object);

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
                ZipCode = 11233,
                

            };
            _warehouseController.Create(warehouse);
            _mockSetWarehouse.Verify(x => x.Add(warehouse), Times.Once);
            _mockContext.Verify(x => x.SaveChanges(), Times.Exactly(1453));
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

        [Test]
        public void Delete_Post_Redirect_To_Index()
        {
            var result = _warehouseController.Delete(1, ResourceData.Warehouses[0]) as RedirectToRouteResult;
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [Test]
        public void Details_Get_Object()
        {
            var actionResult = _warehouseController.Details(1);
            var viewResult = actionResult as ViewResult;
            var result = (Warehouse) viewResult.Model;

            Assert.AreEqual(ResourceData.Warehouses[0].City, result.City);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual(ResourceData.Warehouses[0].Name, result.Name);
        }

        [Test]
        public void Edit_Get_Object()
        {
            var actionResult = _warehouseController.Edit(1);
            var viewResult = actionResult as ViewResult;
            var result = (Warehouse) viewResult.Model;
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual(ResourceData.Warehouses[0].Name, result.Name);
            Assert.AreEqual(ResourceData.Warehouses[0].City, result.City);
            Assert.AreEqual(ResourceData.Warehouses[0].ZipCode, result.ZipCode);
        }

        //TODO: Storageplace måste få ett lagerID, då bör dessa 2 tester fungera.
        [Test]
        public void Edit_Post_Redirect_To_Index()
        {
            var result = _warehouseController.Edit(ResourceData.Warehouses[0]) as RedirectToRouteResult;
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [Test]
        public void Edit_Update_Db_New_Info_In_Object()
        {
            var warehouses = _mockSetWarehouse.Object.ToList();
            var tempObj = warehouses[0];
            tempObj.City = "Göteborg";
            _warehouseController.Edit(tempObj);

            Assert.AreEqual("Göteborg", warehouses[0].City);
            _mockSetWarehouse.Verify(x => x.Attach(tempObj), Times.Once);
            _mockContext.Verify(x => x.SaveChanges(), Times.Once);
        }


        [Test]
        public void Index_Retrive_All_Data()
        {
            var actionResult = _warehouseController.Index();
            var viewResult = actionResult as ViewResult;
            var viewResultModel = (Warehouse[]) viewResult.Model;
            var warehouses = viewResultModel.ToList();
            Assert.AreEqual(2, warehouses.Count);
        }

        [Test]
        public void View_Delete_With_Existing_Entity_Does_Not_Return_404_Error()
        {
            var result = _warehouseController.Delete(1);
            Assert.AreNotEqual(typeof(HttpNotFoundResult), result.GetType());
        }

        //TODO: Implemetera en null check i Delete, Edit och Details.
        [Test]
        public void View_Delete_Without_Existing_Entity_Return_404_Error()
        {
            var result = _warehouseController.Delete(2000);
            Assert.AreEqual(typeof(HttpNotFoundResult), result.GetType());
        }
        [Test]
        public void View_Detail_With_Existing_Does_Not_Return_404_Error()
        {
            var result = _warehouseController.Details(1);
            Assert.AreNotEqual(typeof(HttpNotFoundResult), result.GetType());
        }

    }
}