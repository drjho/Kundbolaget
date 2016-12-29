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

            var customerAddress = customerAddressRepo.GetItems().Where(
                a => a.AddressType == AddressType.Leverans &&
                a.Address.AddressOrderId == (string)jCustomerOrder["addressid"] &&
                a.Customer.CustomerOrderId == (string)jCustomerOrder["customerid"]).SingleOrDefault();

            if (customerAddress == null)
            {
                ModelState.AddModelError("CustomerAddress", "V.g. kontrollera angivet CustomerOrderId eller AddressOrderId");
                return View();
            }

            var customer = customerAddress.Customer;

            var allProducts = productRepo.GetItems();

            var deliveryDate = Convert.ToDateTime(jCustomerOrder["date"]);

            var jProducts = jCustomerOrder["products"].ToArray();
            var order = new Order
            {
                CustomerId = customerAddress.Customer.Id,
                AddressId = customerAddress.Address.Id,
                OrderDate = DateTime.Today,
                DesiredDeliveryDate = Convert.ToDateTime(jCustomerOrder["date"]),
                Comment = (string)jCustomerOrder["comment"],
            };

            if (customer != null)
            {
                var firstPossibleDate = DateTime.Today.AddDays(customer.DaysToDelievery);
                order.PlannedDeliveryDate = (order.DesiredDeliveryDate.CompareTo(firstPossibleDate) < 0) ? firstPossibleDate : order.DesiredDeliveryDate;
            }

            foreach (var jProduct in jProducts)
            {
                var orderProduct = new OrderProduct
                {
                    OrderId = order.Id,
                    Comment = (string)jProduct["comment"],
                    ProductId = productRepo.GetItem((string)jProduct["pno"]).Id,
                    OrderedAmount = (int)jProduct["amount"]
                };
                order.OrderProducts.Add(orderProduct);
            }

            var existingOrders = orderRepo.GetItems().Where(o => o.AddressId == order.AddressId && o.CustomerId == order.CustomerId);
            foreach (var item in existingOrders)
            {
                if (item.DesiredDeliveryDate.Equals(order.DesiredDeliveryDate) && item.Comment.Equals(order.Comment))
                {
                    ModelState.AddModelError("SimilarOrder", "Kanske upprepad order");
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