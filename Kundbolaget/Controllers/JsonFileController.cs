using Kundbolaget.EntityFramework.Context;
using Kundbolaget.EntityFramework.Repositories;
using Kundbolaget.Models.EntityModels;
using Kundbolaget.Models.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Kundbolaget.Controllers
{
    public class JsonFileController : Controller
    {

        DbCustomerRepository customerRepo = new DbCustomerRepository();
        DbCustomerAddressRepository customerAddressRepo = new DbCustomerAddressRepository();
        DbProductRepository productRepo = new DbProductRepository();
        DbStoragePlaceRepository storageRepo = new DbStoragePlaceRepository();

        // GET: JsonFile
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UploadJson()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadJson(OrderUploadVM model)
        {
            byte[] data;

            if (model.File == null)
                return View();

            using (MemoryStream ms = new MemoryStream())
            {
                model.File.InputStream.CopyTo(ms);
                data = ms.ToArray();
            }

            var json = Encoding.Default.GetString(data);

            JObject jCustomerOrder = JObject.Parse(json);

            var customer = customerRepo.GetItem((string)jCustomerOrder["customerid"]);

            var customerAddresses = customerAddressRepo.GetItems(customer?.Id);

            var allProducts = productRepo.GetItems();


            var addressOrderId = (string)jCustomerOrder["addressid"];
            var address = customerAddresses?.SingleOrDefault(
                c => c.AddressType == AddressType.Leverans && c.Address.AddressOrderId == addressOrderId)?.Address;

            var order = new Order
            {
                CustomerId = customer?.Id,
                AddressId = address?.Id,
                OrderDate = DateTime.Today,
                PlannedDeliveryDate = Convert.ToDateTime(jCustomerOrder["date"])
            };

            if (customer != null)
            {
                var firstPossibleDate = DateTime.Today.AddDays(customer.DaysToDelievery);
                if (order.PlannedDeliveryDate.CompareTo(firstPossibleDate) < 0)
                    order.PlannedDeliveryDate = firstPossibleDate;
            }

            List<OrderProduct> orderProducts = new List<OrderProduct>();
            var jProducts = jCustomerOrder["products"].ToArray();
            foreach (var jProduct in jProducts)
            {
                var orderProduct = new OrderProduct
                {
                    OrderId = order.Id,
                    Comment = (string)jProduct["comment"],
                    Product = productRepo.GetItem((string)jProduct["pno"]),
                    OrderedAmount = (int)jProduct["amount"]
                };

                orderProduct.DeliveredAmount = storageRepo.ReserveItem(orderProduct.Product?.Id, orderProduct.OrderedAmount);
                orderProducts.Add(orderProduct);

            }

            order.OrderProducts = orderProducts;
            order.Comment = (string)jCustomerOrder["comment"];

            // Lägg till importkommentarer i order när delieveredamount blir mindre än hälften av beställt antal.

            return RedirectToAction("CreateFromFile", "Orders", order);
        }
    }
}