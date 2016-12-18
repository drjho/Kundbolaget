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
        DbStoreRepository productRepo = new DbStoreRepository();
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

            //if (customer == null)
            //    return RedirectToAction("Index", "Customer");

            var customerAddresses = customerAddressRepo.GetItems(customer?.Id);

            //if (customerAddresses?.Length == 0)
            //    return RedirectToAction("Index", "CustomerAddress");

            var allProducts = new DbStoreRepository().GetProducts();
            //var db = new StoreContext();

            List<Order> orderList = new List<Order>();

            JToken[] jOrders = jCustomerOrder["orders"].ToArray();
            foreach (var o in jOrders)
            {

                // Skulle man kolla upp att adressen stämmer?
                var addressid = (string)o["addressid"];

                var address = customerAddresses?.SingleOrDefault(c => c.Address.AddressOrderId == addressid);

                //if (address == null)
                //    return RedirectToAction("Index", "Address");


                var order = new Order
                {
                    CustomerAddressId = address?.Id,
                    //DesiredDeliveryDate = desiredDate,
                    OrderDate = DateTime.Today,
                    PlannedDeliveryDate = Convert.ToDateTime(o["date"])
                };

                if (customer != null)
                {
                    var firstPossibleDate = DateTime.Today.AddDays(customer.DaysToDelievery);
                    if (order.PlannedDeliveryDate.CompareTo(firstPossibleDate) < 0)
                        order.PlannedDeliveryDate = firstPossibleDate;
                }

                List<OrderProduct> orderProducts = new List<OrderProduct>();
                var jProducts = o["products"].ToArray();
                foreach (var jProduct in jProducts)
                {
                    var orderProduct = new OrderProduct
                    {
                        OrderId = order.Id,
                        Comment = (string)jProduct["comment"],
                        Product = productRepo.GetProduct((string)jProduct["pno"]),
                        OrderedAmount = (int)jProduct["amount"]
                    };

                    orderProduct.OrderedAmount = storageRepo.ReserveItem(orderProduct.Product?.Id, orderProduct.OrderedAmount);
                    orderProducts.Add(orderProduct);
                }

                order.OrderProducts = orderProducts;
                order.Comment = (string)o["comment"];

                orderList.Add(order);
            }
            return RedirectToAction("CreateFromFile", "Orders", orderList);
        }
    }
}