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

            using (MemoryStream ms = new MemoryStream())
            {
                model.File.InputStream.CopyTo(ms);
                data = ms.ToArray();
            }

            var json = Encoding.Default.GetString(data);

            JObject jCustomerOrder = JObject.Parse(json);

            var customer = new DbCustomerRepository().GetItems().
                Where(c => c.CustomerOrderId == (string)jCustomerOrder["customerid"]).FirstOrDefault();

            if (customer == null)
                return RedirectToAction("Index", "Customer");

            var customerAddresses = new DbCustomerAddressRepository().GetItems().
                Where(c => c.CustomerId == customer.Id).ToList();

            if (customerAddresses.Count == 0)
                return RedirectToAction("Index", "CustomerAddress");

            var allProducts = new DbStoreRepository().GetProducts();
            var db = new StoreContext();

            JToken[] jOrders = jCustomerOrder["orders"].ToArray();
            foreach (var o in jOrders)
            {

                // Skulle man kolla upp att adressen stämmer?
                var addressid = (string)o["addressid"];

                var address = customerAddresses.Where(c => c.Address.AddressOrderId == addressid).FirstOrDefault();

                if (address == null)
                    return RedirectToAction("Index", "Address");

                var dDate = Convert.ToDateTime(o["date"]);

                var order = new Order
                {
                    CustomerAddressId = address.Id,
                    DesiredDeliveryDate = dDate,
                    OrderDate = DateTime.Today,
                    PlannedDeliveryDate = dDate.AddDays(customer.DaysToDelievery),
                };

                List<OrderProduct> orderProducts = new List<OrderProduct>();
                var jProducts = o["products"].ToArray();
                foreach (var prod in jProducts)
                {
                    var p = allProducts.SingleOrDefault(a => a.ProductOrderId == (string)prod["pno"]);
                    if (p != null)
                        orderProducts.Add(new OrderProduct {OrderId = order.Id, Comment = (string)o["comments"],
                            ProductId = p.Id, OrderedAmount = (int)prod["amount"] });
                    else
                        return RedirectToAction("Index", "Order");

                }
                order.OrderProducts = orderProducts;

                db.OrderProducts.AddRange(order.OrderProducts);
                db.Orders.Add(order);
                db.SaveChanges();

            }



            return RedirectToAction("Index", "Home");
        }
    }
}