using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            //Inject mock data via overload constructor
            var dbWarehouseRepository = new DbWarehouseRepository(_mockContext.Object);
            var dbStoragePlaceRepository = new DbStoragePlaceRepository(_mockContext.Object);

            //Setup fakerepo via overloaded constructor
            _warehouseController = new WarehouseController(dbWarehouseRepository, dbStoragePlaceRepository);
        }

        [Test]
        public void Create()
        {
            
        }
    }
}
