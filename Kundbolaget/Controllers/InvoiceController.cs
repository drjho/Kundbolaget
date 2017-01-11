using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Kundbolaget.EntityFramework.Context;
using Kundbolaget.EntityFramework.Repositories;

namespace Kundbolaget.Controllers
{
    public class InvoiceController : Controller
    {
        private DbInvoiceDepository invoiceRepo;
        private DbOrderRepository orderRepo;
        private DbOrderProductRepository orderProductRepo;
        private DbCustomerRepository customerRepo;
        private DbProductRepository productRepository;
        private DbPriceListRepository priceListRepository;


        public InvoiceController()
        {
            StoreContext db = new StoreContext();
            invoiceRepo = new DbInvoiceDepository(db);
            orderRepo = new DbOrderRepository(db);

            
        }
        // GET: Invoice
        public ActionResult Index(DateTime? searchDate)
        {
            var model = invoiceRepo.GetItems();
            if (searchDate.HasValue)
            {
                model = model.Where(x => x.PayBefore == searchDate).ToArray();
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
            var invoice = invoiceRepo.GetItem((int) id);
            if (invoice == null)
            {
                return HttpNotFound();
            }
            return View();
        }
    }
}