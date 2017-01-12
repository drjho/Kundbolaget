using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Kundbolaget.EntityFramework.Context;
using Kundbolaget.EntityFramework.Repositories;
using Kundbolaget.Models.EntityModels;


namespace Kundbolaget.Controllers
{
    public class InvoiceController : Controller
    {
        private DbInvoiceDepository invoiceRepo;
        private DbOrderRepository orderRepo;
        private DbPriceListRepository priceListRepository;


        public InvoiceController()
        {
            StoreContext db = new StoreContext();
            invoiceRepo = new DbInvoiceDepository(db);
            orderRepo = new DbOrderRepository(db);
            priceListRepository = new DbPriceListRepository(db);

        }
        // GET: Invoice
        public ActionResult Index(DateTime? searchDate)
        {
            var model = invoiceRepo.GetItems();
            if (searchDate.HasValue)
            {
                model = model.Where(x => x.InvoiceDate == searchDate).ToArray();
            }
            return View(model);
        }

        //GET: Invoice/Details
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var invoice = invoiceRepo.GetItem((int)id);
            if (invoice == null)
            {
                return HttpNotFound();
            }
            return View(invoice);
        }

        
        public ActionResult Create(int id)
        {
            //TODO: Kolla att det inte redan finns en Faktura med detta orderID
            var order = orderRepo.GetItem(id);


            //FileStream fs = new FileStream(@"C:\Users\pontu\Source\Repos\Kundbolaget\Kundbolaget\PDF\" + "\\" + "First PDF document.pdf", FileMode.Create);
            //var pricelists = priceListRepository.GetItems().ToList();
            //var priceList = new List<PriceList>();
            //foreach (var price in pricelists)
            //{
            //    foreach (var product in order.OrderProducts)
            //    {
            //        if (price.Product.Id == product.Product.Id && order.Customer.CustomerGroupId == price.CustomerGroupId)
            //        {
            //            priceList.Add(price);
            //        }
            //    }
            //}

            var invoice = new Invoice {
                OrderId = order.Id,
                InvoiceDate = DateTime.Today.Date,
                Paid = false,
                CustomerId = order.CustomerId,
                AddressId = order.AddressId
            };

            invoiceRepo.CreateItem(invoice);
            return RedirectToAction("Index");
        }
        
    }
}