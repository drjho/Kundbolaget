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

            JObject jo = JObject.Parse(json);
            JToken jOrder = jo["customerorder"];

            var customer = new DbCustomerRepository().GetItems().
                Where(c => c.Name == (string)jOrder["name"]).FirstOrDefault();

            // Skulle man kolla upp att adressen stämmer?
            var address = jo["shipto"];
            var street = (string)address["street"];
            var no = (string)address["streetno"];
            var code = (string)address["areacode"];
            var area = (string)address["area"];
            var country = (string)address["country"];

            JToken[] products = jOrder["products"].ToArray();
            var date = (string)jOrder["date"];

            //products.Select(p => new OrderProduct
            //{
                
            //})

            var order = new Order
            {
                CustomerId = customer.Id,
                

            };


            var entity = JsonConvert.DeserializeObject<Order[]>(json);

            return RedirectToAction("Index", "Home");
        }
    }
}