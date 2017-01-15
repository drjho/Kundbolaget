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

        DbStoragePlaceRepository storageRepo;
        DbOrderRepository orderRepo;
        DbOrderProductRepository orderProductRepo;
        DbCustomerAddressRepository customerAddressRepo;
        DbProductRepository productRepo;


        public JsonFileController()
        {
            var db = new StoreContext();
            orderRepo = new DbOrderRepository(db);
            orderProductRepo = new DbOrderProductRepository(db);
            storageRepo = new DbStoragePlaceRepository(db);
            customerAddressRepo = new DbCustomerAddressRepository();
            productRepo = new DbProductRepository(db);
        }

        public JsonFileController(DbStoragePlaceRepository dbStoragePlaceRepository, DbOrderRepository dbOrderRepository, DbOrderProductRepository dbOrderProductRepository,
            DbCustomerAddressRepository dbCustomerAddressRepository, DbProductRepository dbProductRepository)
        {
            storageRepo = dbStoragePlaceRepository;
            orderRepo = dbOrderRepository;
            orderProductRepo = dbOrderProductRepository;
            customerAddressRepo = dbCustomerAddressRepository;
            productRepo = dbProductRepository;
        }

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
            {
                ModelState.AddModelError("NullFile", "Inget filnamn angivet");
                return View();
            }

            using (MemoryStream ms = new MemoryStream())
            {
                model.File.InputStream.CopyTo(ms);
                data = ms.ToArray();
            }

            var json = Encoding.Default.GetString(data);

            JObject jCustomerOrder = JObject.Parse(json);

            var items = customerAddressRepo.GetItems();

            var cid = (string)jCustomerOrder["customerid"];
            var aid = (string)jCustomerOrder["addressid"];

            var customerAddress = customerAddressRepo.GetItems().Where(
                a => a.AddressType == AddressType.Leverans &&
                a.Address.AddressOrderId == aid &&
                a.Customer.CustomerOrderId == cid).SingleOrDefault();

            if (customerAddress == null)
            {
                ModelState.AddModelError("", $"Kundorderid: '{cid}' eller Adressorderid: '{aid}' är felaktigt.");
                return View();
            }

            var customer = customerAddress.Customer;

            var allProducts = productRepo.GetItems();

            string dStr = (string)jCustomerOrder["date"];
            DateTime dDate;

            if (dStr.Length < 1)
            {
                dDate = DateTime.Now;
            }
            else if (!DateTime.TryParse(dStr, out dDate))
            {
                ModelState.AddModelError("", $"Önskad leveransdatum: '{dStr}' är felaktigt.");
                return View();
            }

            var order = new Order
            {
                CustomerId = customerAddress.Customer.Id,
                AddressId = customerAddress.Address.Id,
                OrderDate = DateTime.Today,
                DesiredDeliveryDate = dDate,
                Comment = (string)jCustomerOrder["comment"]
            };

            if (customer != null)
            {
                var firstPossibleDate = DateTime.Today.AddDays(customer.DaysToDelievery);
                order.PlannedDeliveryDate = (order.DesiredDeliveryDate.CompareTo(firstPossibleDate) <= 0) ? firstPossibleDate : order.DesiredDeliveryDate;
            }

            var jProducts = jCustomerOrder["products"].ToArray();
            foreach (var jProduct in jProducts)
            {
                var prodString = (string)jProduct["pno"];
                var product = productRepo.GetItem(prodString);
                if (product == null)
                {
                    ModelState.AddModelError("", $"Produktorderid: '{prodString}' finns inte.");
                    return View();
                }

                var aStr = (string)jProduct["amount"];
                uint oa;
                if (!uint.TryParse(aStr, out oa))
                {
                    ModelState.AddModelError("", $"Beställt antal: '{aStr}' är felaktigt.");
                    return View();
                }
                if (oa < 1)
                {
                    ModelState.AddModelError("", $"Beställt antal: '{aStr}' mindre än 1.");
                    return View();
                }
                var orderProduct = new OrderProduct
                {
                    OrderId = order.Id,
                    Comment = (string)jProduct["comment"],
                    ProductId = productRepo.GetItem((string)jProduct["pno"]).Id,
                    OrderedAmount = (int)oa
                };
                order.OrderProducts.Add(orderProduct);
            }

            var existingOrders = orderRepo.GetItems().Where(o => o.AddressId == order.AddressId && o.CustomerId == order.CustomerId);
            foreach (var item in existingOrders)
            {
                if (item.DesiredDeliveryDate.Equals(order.DesiredDeliveryDate) && item.Comment.Equals(order.Comment))
                {
                    ModelState.AddModelError("", "En order med samma kundorderid, adressorderid och önskat leveransdatum är redan registrerad.");
                    return View();
                }
            }

            orderRepo.CreateItem(order);
            //foreach (var item in order.OrderProducts)
            //{
            //    orderProductRepo.CreateItem(item);
            //}
            //orderRepo.HandleOrder(order);
            return RedirectToAction("Index", "Orders");
        }
    }
}