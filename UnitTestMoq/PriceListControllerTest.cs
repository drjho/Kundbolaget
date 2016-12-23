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
    internal class PriceListControllerTest
    {
        private Mock<StoreContext> _mockContext;

        private Mock<DbSet<PriceList>> _mockSetPriceList;

        private PriceListController _priceListController;

        [SetUp]
        public void Initializer()
        {
            _mockContext = new Mock<StoreContext>();

            _mockSetPriceList = new Mock<DbSet<PriceList>>();

            var dataPriceList = ResourceData.PriceLists.AsQueryable();

            var setupDbPriceList = Helper.SetupDb(_mockSetPriceList, dataPriceList);

            _mockContext.Setup(pl => pl.PriceLists).Returns(setupDbPriceList.Object);

            var dbPriceListRepository = new DbPriceListRepository(_mockContext.Object);

            _priceListController = new PriceListController(dbPriceListRepository);
        }

        [Test]
        public void Create_PriceList()
        {
            var priceList = new PriceList
            {
                Id = 10,
                ProductId = 2,
                Price = 10,
                RebatePerPallet = 20
            };
            _priceListController.Create(priceList);
            _mockSetPriceList.Verify(x => x.Add(priceList), Times.Once);
            _mockContext.Verify(x => x.SaveChanges(), Times.Once);
        }

        [Test]
        public void Delete_PriceList()
        {
            var actionResult = _priceListController.Delete(1);
            var viewResult = actionResult as ViewResult;
            var priceList = (PriceList) viewResult.Model;
            Assert.AreEqual(1, priceList.Id);
        }

        [Test]
        public void Edit_PriceList()
        {
            var actionResult = _priceListController.Edit(1);
            var viewResult = actionResult as ViewResult;
            var priceList = (PriceList)viewResult.Model;
            Assert.AreEqual(1, priceList.Id);
        }

        [Test]
        public void Details_PriceList()
        {
            var actionResult = _priceListController.Details(1);
            var viewResult = actionResult as ViewResult;
            var priceList = (PriceList) viewResult.Model;
            Assert.AreEqual(1, priceList.Id);
        }

        [Test]
        public void Create_Post_Redirect_To_Index()
        {
            var result = _priceListController.Create(new PriceList
            {
                Id = 3,
                ProductId = 713,
                Price = 12,
                RebatePerPallet = 13
            }
                ) as RedirectToRouteResult;
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [Test]
        public void Index_Retrieve_All_Data()
        {
            var actionResult = _priceListController.Index();
            var viewResult = actionResult as ViewResult;
            var viewResultModel = (PriceList[]) viewResult.Model;
            var priceListInfo = viewResultModel.ToList();

            Assert.AreEqual(1, priceListInfo.Count);
        }
    }
}
