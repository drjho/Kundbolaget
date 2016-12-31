using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Kundbolaget.EntityFramework.Context;
using Kundbolaget.Models.EntityModels;
using Kundbolaget.EntityFramework.Repositories;

namespace Kundbolaget.Controllers
{
    public class OrderProductsController : Controller
    {
        private DbOrderProductRepository orderProductRepo;
        private DbOrderRepository orderRepo;
        private DbProductRepository productRepo;

        public OrderProductsController()
        {
            var db = new StoreContext();
            orderProductRepo = new DbOrderProductRepository(db);
            orderRepo = new DbOrderRepository(db);
            productRepo = new DbProductRepository(db);
        }

        public OrderProductsController(DbOrderProductRepository dbOrderProductRepository, DbOrderRepository dbOrderRepository, DbProductRepository dbProductRepository)
        {
            orderProductRepo = dbOrderProductRepository;
            orderRepo = dbOrderRepository;
            productRepo = dbProductRepository;
        }

        // GET: OrderProducts
        public ActionResult Index()
        {
            //var orderProducts = db.OrderProducts.Include(o => o.Order).Include(o => o.Product);
            var orderProducList = orderProductRepo.GetItems().ToArray();
            return View(orderProducList);
        }

        // GET: OrderProducts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            OrderProduct orderProduct = orderProductRepo.GetItem((int)id);
            if (orderProduct == null)
            {
                return HttpNotFound();
            }
            return View(orderProduct);
        }

        // GET: OrderProducts/Create
        public ActionResult Create()
        {
            ViewBag.OrderId = new SelectList(orderRepo.GetItems(), "Id", "Id");
            ViewBag.ProductId = new SelectList(productRepo.GetItems(), "Id", "Name");
            return View();
        }

        // POST: OrderProducts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ProductId,OrderedAmount,AvailabeAmount,DeliveredAmount,AcceptedAmount,Comment,OrderId")] OrderProduct newOrderProduct)
        {
            if (newOrderProduct.ProductId == null)
                ModelState.AddModelError("ProductId", "Produktid ej satt");
            if (ModelState.IsValid)
            {
                //db.OrderProducts.Add(orderProduct);
                //db.SaveChanges();
                orderProductRepo.CreateItem(newOrderProduct);
                return RedirectToAction("Index");
            }

            ViewBag.OrderId = new SelectList(orderRepo.GetItems(), "Id", "Id", newOrderProduct.OrderId);
            ViewBag.ProductId = new SelectList(productRepo.GetItems(), "Id", "Name", newOrderProduct.ProductId);
            return View(newOrderProduct);
        }

        // GET: OrderProducts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderProduct orderProduct = orderProductRepo.GetItem((int)id);
            if (orderProduct == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrderId = new SelectList(orderRepo.GetItems(), "Id", "Id", orderProduct.OrderId);
            ViewBag.ProductId = new SelectList(productRepo.GetItems(), "Id", "Name", orderProduct.ProductId);
            return View(orderProduct);
        }

        // POST: OrderProducts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ProductId,OrderedAmount,AvailabeAmount,DeliveredAmount,AcceptedAmount,Comment,OrderId")] OrderProduct updatedOrderProduct)
        {
            if (ModelState.IsValid)
            {
                orderProductRepo.UpdateItem(updatedOrderProduct);
                return RedirectToAction("FinalizeDelivery", "Orders", new { id = updatedOrderProduct.OrderId });
                //return RedirectToAction("Index");
            }
            ViewBag.OrderId = new SelectList(orderRepo.GetItems(), "Id", "Id", updatedOrderProduct.OrderId);
            ViewBag.ProductId = new SelectList(productRepo.GetItems(), "Id", "Name", updatedOrderProduct.ProductId);
            return View(updatedOrderProduct);
        }

        // GET: OrderProducts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderProduct orderProduct = orderProductRepo.GetItem((int)id);
            if (orderProduct == null)
            {
                return HttpNotFound();
            }
            return View(orderProduct);
        }

        // POST: OrderProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OrderProduct orderProduct = orderProductRepo.GetItem(id);
            if (orderProduct == null)
            {
                return HttpNotFound();
            }
            orderProductRepo.DeleteItem(orderProduct.Id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                orderProductRepo.Dispose();
                orderRepo.Dispose();
                productRepo.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
