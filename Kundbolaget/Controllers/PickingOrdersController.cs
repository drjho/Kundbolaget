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
    public class PickingOrdersController : Controller
    {
        private DbPickingOrderRepository pickingOrderRepo;
        private DbOrderProductRepository orderProductRepo;
        private DbStoragePlaceRepository storageRepo;


        public PickingOrdersController()
        {
            StoreContext db = new StoreContext();
            pickingOrderRepo = new DbPickingOrderRepository(db);
            orderProductRepo = new DbOrderProductRepository(db);
            storageRepo = new DbStoragePlaceRepository(db);
        }

        public PickingOrdersController(
            DbPickingOrderRepository dbPickingOrderRepo,
            DbOrderProductRepository dbOrderProductRepo,
            DbStoragePlaceRepository dbstoragePlaceRepo)
        {
            pickingOrderRepo = dbPickingOrderRepo;
            orderProductRepo = dbOrderProductRepo;
            storageRepo = dbstoragePlaceRepo;
        }

        // GET: PickingOrders
        public ActionResult Index(DateTime? searchDate)
        {
            var pickingOrders = pickingOrderRepo.GetItems().Where(x => x.OrderProduct.Order.OrderStatus == OrderStatus.Plockar).ToArray();
            if (searchDate.HasValue)
                pickingOrders = pickingOrders.Where(x => x.OrderProduct.Order.PlannedDeliveryDate == searchDate).ToArray();
            return View(pickingOrders.ToList());
        }

        // GET: PickingOrders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PickingOrder pickingOrder = pickingOrderRepo.GetItem((int)id);
            if (pickingOrder == null)
            {
                return HttpNotFound();
            }
            return View(pickingOrder);
        }

        // GET: PickingOrders/Create
        public ActionResult Create()
        {
            ViewBag.OrderProductId = new SelectList(orderProductRepo.GetItems(), "Id", "Id");
            ViewBag.StoragePlaceId = new SelectList(storageRepo.GetItems(), "Id", "Id");
            return View();
        }

        // POST: PickingOrders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,StoragePlaceId,ReservedAmount,PickedAmount,Comment,OrderProductId")] PickingOrder pickingOrder)
        {
            if (pickingOrder.OrderProductId == null)
                ModelState.AddModelError("OrderProductId", "OrderProductId är inte satt");
            if (ModelState.IsValid)
            {
                pickingOrderRepo.CreateItem(pickingOrder);
                return RedirectToAction("Index");
            }

            ViewBag.OrderProductId = new SelectList(orderProductRepo.GetItems(), "Id", "Id", pickingOrder.OrderProductId);
            ViewBag.StoragePlaceId = new SelectList(storageRepo.GetItems(), "Id", "Id", pickingOrder.StoragePlaceId);
            return View(pickingOrder);
        }

        // GET: PickingOrders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PickingOrder pickingOrder = pickingOrderRepo.GetItem((int)id);
            if (pickingOrder == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrderProductId = new SelectList(orderProductRepo.GetItems(), "Id", "Id", pickingOrder.OrderProductId);
            ViewBag.StoragePlaceId = new SelectList(storageRepo.GetItems(), "Id", "Id", pickingOrder.StoragePlaceId);
            return View(pickingOrder);
        }

        // POST: PickingOrders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,StoragePlaceId,ReservedAmount,PickedAmount,Comment,OrderProductId")] PickingOrder pickingOrder)
        {
            if (ModelState.IsValid)
            {
                pickingOrderRepo.UpdateItem(pickingOrder);
                //return RedirectToAction("Index");
                var orderId = orderProductRepo.GetItem((int)pickingOrder.OrderProductId).OrderId;
                return RedirectToAction("ShowPickingOrder", "Orders", new { id = orderId });
            }
            ViewBag.OrderProductId = new SelectList(orderProductRepo.GetItems(), "Id", "Id", pickingOrder.OrderProductId);
            ViewBag.StoragePlaceId = new SelectList(storageRepo.GetItems(), "Id", "Id", pickingOrder.StoragePlaceId);
            return View(pickingOrder);
        }

        // GET: PickingOrders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PickingOrder pickingOrder = pickingOrderRepo.GetItem((int)id);
            if (pickingOrder == null)
            {
                return HttpNotFound();
            }
            return View(pickingOrder);
        }

        // POST: PickingOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var order = orderProductRepo.GetItem(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            pickingOrderRepo.DeleteItem(id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                pickingOrderRepo.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
