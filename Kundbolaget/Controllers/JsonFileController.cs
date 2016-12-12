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

            JObject jOrder = JObject.Parse(json);
            //JToken jOrder = jo["customerorder"];

            var customer = new DbCustomerRepository().GetItems().
                Where(c => c.Name == (string)jOrder["customerid"]).FirstOrDefault();

            if (customer == null)
                return RedirectToAction("Index", "Customer");

            var customerAddresses = new DbCustomerAddressRepository().GetItems().
                Where(c => c.CustomerId == customer.Id).ToList();

            if (customerAddresses.Count == 0)
                return RedirectToAction("Index", "CustomerAddress");

            // Skulle man kolla upp att adressen stämmer?
            var shipto = jOrder["shipto"];
            var street = (string)shipto["street"];
            var no = (int)shipto["streetno"];
            var code = (string)shipto["areacode"];
            var area = (string)shipto["area"];
            var country = (string)shipto["country"];

            var address = customerAddresses.Where(c => c.Address.StreetName == street &&
            c.Address.Number == no && c.Address.PostalCode == code && c.Address.Area == area).FirstOrDefault();

            if (address == null)
                return RedirectToAction("Index", "Address");

            JToken[] products = jOrder["products"].ToArray();
            
            var order = new Order
            {
                CustomerAddressId = address.Id,
                //DesiredDeliveryDate = Convert.ToDateTime(jOrder["date"]),
                DesiredDeliveryDate = DateTime.Today,
                OrderDate = DateTime.Today,
                PlannedDeliveryDate = DateTime.Today,
                OrderProducts = products.Select(p => new OrderProduct
                {
                    ProductId = (int)p["pno"],
                    OrderedAmount = (int)p["amount"]
                }).ToList()
            };

            //var entity = JsonConvert.DeserializeObject<Order[]>(json);

            var db = new StoreContext();
            db.OrderProducts.AddRange(order.OrderProducts);
            db.Orders.Add(order);
            db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
    }
}