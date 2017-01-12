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
        private DbPriceListRepository priceListRepo;


        public InvoiceController()
        {
            StoreContext db = new StoreContext();
            invoiceRepo = new DbInvoiceDepository(db);
            orderRepo = new DbOrderRepository(db);
            priceListRepo = new DbPriceListRepository(db);

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

        public ActionResult Delete(int? id)
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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var invoice = invoiceRepo.GetItem(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }
            invoiceRepo.DeleteItem(id);
            return RedirectToAction("Index");

        }

        public float GetTotalPrice(int customerGroupId, int productId, int amount)
        {
            var productPrice = priceListRepo.GetItems().SingleOrDefault(x => x.CustomerGroupId == customerGroupId && x.ProductId == productId);
            var limit = 0;
            switch (productPrice.Product.StoragePackage)
            {
                case StoragePackage.Back:
                    limit = 384;
                    break;
                case StoragePackage.Kartong:
                    limit = 240;
                    break;
                case StoragePackage.Flak:
                    limit = 480;
                    break;
            }
            var unitPrice = productPrice.Price * (amount > limit ? (1 - productPrice.RebatePerPallet * .01f) : 1);
            return amount * unitPrice;
        }

        public ActionResult Create(int id)
        {
            //TODO: Kolla att det inte redan finns en Faktura med detta orderID
            var order = orderRepo.GetItem(id);

            var customerGroupId = order.Customer.CustomerGroupId;

            foreach (var item in order.OrderProducts)
            {
                item.Price = GetTotalPrice(customerGroupId, (int)item.ProductId, item.AcceptedAmount);
            }

            order.OrderStatus = OrderStatus.Fakturerar;
            orderRepo.UpdateItem(order);

            var invoice = new Invoice
            {
                OrderId = order.Id,
                InvoiceDate = DateTime.Today.Date,
                Paid = false,
            };

            invoiceRepo.CreateItem(invoice);
            return RedirectToAction("Index");
        }

        public ActionResult ChangeToPaid(int id)
        {
            var invoice = invoiceRepo.GetItem(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }
            invoice.Paid = true;
            invoice.Order.OrderStatus = OrderStatus.Arkiverad;
            invoiceRepo.UpdateItem(invoice);
            return RedirectToAction("Index");
        }

    }
}