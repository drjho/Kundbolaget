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
using Kundbolaget.Models.ViewModels;

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

            var model = new InvoiceVM
            {
                Id = invoice.Id,
                OrderId = invoice.Id,
                CustomerName = invoice.Order.Customer.Name,
                Address = invoice.Order.Address.AddressString,
                InvoiceDate = invoice.InvoiceDate,
                Paid = invoice.Paid,
                IsOverdue = invoice.IsOverdue,
                TotalPrice = invoice.TotalPrice,
                VAT = invoice.TotalPrice * 0.25f,
                TotalSum = invoice.TotalPrice * 1.25f
            };
            var customerGroupId = invoice.Order.Customer.CustomerGroupId;
            foreach (var op in invoice.Order.OrderProducts)
            {
                model.ProductList.Add(new OrderProductVM
                {
                    Id = op.Id,
                    ProductName = op.Product.ShortDescription,
                    OrderedAmount = op.OrderedAmount,
                    AcceptedAmount = op.AcceptedAmount,
                    UnitPrice = GetUnitPrice(customerGroupId, (int)op.ProductId, op.AcceptedAmount),
                    Price = op.Price,
                    Comment = op.Comment,
                });
            }
            return View(model);
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

        public float GetUnitPrice(int customerGroupId, int productId, int amount)
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
            return productPrice.Price * (amount > limit ? (1 - productPrice.RebatePerPallet * .01f) : 1);
        }

        public ActionResult Create(int id)
        {
            //TODO: Kolla att det inte redan finns en Faktura med detta orderID
            var order = orderRepo.GetItem(id);

            var customerGroupId = order.Customer.CustomerGroupId;

            foreach (var item in order.OrderProducts)
            {
                item.Price = GetUnitPrice(customerGroupId, (int)item.ProductId, item.AcceptedAmount);
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