using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Kundbolaget.EntityFramework.Repositories;
using Kundbolaget.Models.EntityModels;
using Moq;
using System.Collections.Generic;
using System.Data.Entity;
using Kundbolaget.EntityFramework.Context;
using Kundbolaget.Controllers;

namespace UnitTestMoq
{
    [TestClass]
    public class UnitTestAddress
    {
        [TestMethod]
        public void TestMethodAddress()
        {
            var MockSet = new Mock<DbSet<Address>>();
            var MockContext = new Mock<StoreContext>();
            MockContext.Setup(m => m.Addresses).Returns(MockSet.Object);


            var address = new Address { Id = 1, StreetName = "KungG", PostalCode = "12345", Number = 6, Area = "STHLM", Country = "SWE" }; 
            var service = new AddressController();
            //service.Create(address);
        }
    }
}
