﻿using System;
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
using System.Reflection;

namespace Kundbolaget.Controllers
{
    public class OrdersController : Controller
    {
        //private StoreContext db = new StoreContext();
        private DbOrderRepository orderRepo;
        private DbStoragePlaceRepository storageRepo;
        private DbOrderProductRepository orderProductRepo;

        private DbAddressRepository addressRepo;
        private DbCustomerRepository customerRepo;

        public OrdersController()
        {
            StoreContext db = new StoreContext();
            orderRepo = new DbOrderRepository(db);
            addressRepo = new DbAddressRepository();
            storageRepo = new DbStoragePlaceRepository(db);
            orderProductRepo = new DbOrderProductRepository(db);
            customerRepo = new DbCustomerRepository();
        }

        public OrdersController(DbOrderRepository dbOrderRepo, DbAddressRepository dbAddressRepo,
            DbStoragePlaceRepository dbStorageRepo, DbOrderProductRepository dbOrderProductRepo,
            DbCustomerRepository dbCustomerRepo)
        {
            orderRepo = dbOrderRepo;
            addressRepo = dbAddressRepo;
            storageRepo = dbStorageRepo;
            orderProductRepo = dbOrderProductRepo;
            customerRepo = dbCustomerRepo;
        }

        // GET: Orders
        public ActionResult Index()
        {
            //var model = orderRepo.GetItems().Where(p => p.OrderStatus == OrderStatus.Behandlar);
            var model = orderRepo.GetItems();
            return View(model);
        }

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // TODO: uppdatera tillgängligt antal när man klicka på details!
            var order = orderRepo.GetItem((int)id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        public ActionResult PrepareOrder(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var order = orderRepo.GetItem((int)id);

            if (order == null)
            {
                return HttpNotFound();
            }
            var orderVM = new OrderVM
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                CustomerId = order.CustomerId,
                PlannedDeliveryDate = order.PlannedDeliveryDate,
                AddressId = order.AddressId,
                Comment = order.Comment,
                OrderProducts = order.OrderProducts.Select(p => new OrderProductVM
                {
                    Id = p.Id,
                    ProductId = (int)p.ProductId,
                    OrderedAmount = p.OrderedAmount,
                    AvailabeAmount = p.AvailabeAmount,
                    Comment = p.Comment
                }).ToList()
            };
            return View(orderVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PrepareOrder(OrderVM orderVM)
        {
            var order = orderRepo.GetItem(orderVM.Id);
            order.OrderDate = orderVM.OrderDate;
            order.CustomerId = orderVM.CustomerId;
            order.PlannedDeliveryDate = orderVM.PlannedDeliveryDate;
            order.AddressId = orderVM.AddressId;
            order.Comment = orderVM.Comment;
            order.OrderStatus = OrderStatus.Plockar;

            orderRepo.UpdateItem(order);

            for (int i = 0; i < order.OrderProducts.Count; i++)
            {
                var item = order.OrderProducts[i];
                item.PickList = ReserveItem(item.ProductId, item.OrderedAmount);
                item.AvailabeAmount = item.PickList.Sum(x => x.PickingAmount);
                // TODO: kolla om AvailableAmount är för litet eller noll, notera det?
            }
            orderProductRepo.UpdateItems(order.OrderProducts);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Indata: Produktid och beställt antal.
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="orderedAmount"></param>
        /// <returns>
        /// Utdata: En lista av plockordrar.
        /// </returns>
        public List<PickingOrder> ReserveItem(int? productId, int orderedAmount)
        {
            // This assume that there is only 1 warehouse!!!

            int remainAmount = orderedAmount;

            var storagePlaces = storageRepo.GetItems().Where(sp => sp.ProductId == productId).ToArray();

            var pickList = new List<PickingOrder>();

            for (int i = 0; i < storagePlaces.Length; i++)
            {
                int pickAmount = Math.Min(storagePlaces[i].AvailableAmount, remainAmount);
                if (pickAmount > 0)
                {
                    pickList.Add(new PickingOrder { StoragePlaceId = i, PickingAmount = pickAmount });
                    storagePlaces[i].ReservedAmount += pickAmount;
                    remainAmount -= pickAmount;
                }
                if (remainAmount < 1)
                    break;
            }
            storageRepo.UpdateItems(storagePlaces);
            return pickList;
        }

        public void ReleaseItem(int? productId, int reservedAmount)
        {
            // This assume that there is only 1 warehouse!!!

            int remainAmount = reservedAmount;

            var storagePlaces = storageRepo.GetItems().Where(sp => sp.ProductId == productId).ToArray();

            for (int i = 0; i < storagePlaces.Length; i++)
            {
                int subAmount = Math.Min(storagePlaces[i].ReservedAmount, remainAmount);
                remainAmount -= subAmount;
                storagePlaces[i].ReservedAmount -= subAmount;
                if (remainAmount < 1)
                    break;
            }
            storageRepo.UpdateItems(storagePlaces);
        }


        // GET: Orders/Create
        public ActionResult Create()
        {
            ViewBag.AddressId = new SelectList(addressRepo.GetItems(), "Id", "AddressString");
            ViewBag.CustomerId = new SelectList(customerRepo.GetItems(), "Id", "Name");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,OrderDate,CustomerId,PlannedDeliveryDate,AddressId,Comment")] Order order)
        {
            if (order.CustomerId == null)
                ModelState.AddModelError("CustomerId", "Kundid är inte satt");
            if (order.AddressId == null)
                ModelState.AddModelError("AddressId", "Adressid är inte satt");
            if (ModelState.IsValid)
            {
                orderRepo.CreateItem(order);
                return RedirectToAction("Index");
            }
            return RedirectToAction("Create");
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var order = orderRepo.GetItem((int)id);

            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.AddressId = new SelectList(addressRepo.GetItems(), "Id", "StreetName", order.AddressId);
            ViewBag.CustomerId = new SelectList(customerRepo.GetItems(), "Id", "Name", order.CustomerId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,OrderDate,CustomerId,PlannedDeliveryDate,AddressId,Comment")] Order order)
        {
            if (ModelState.IsValid)
            {
                orderRepo.UpdateItem(order);
                return RedirectToAction("Index");
            }
            ViewBag.AddressId = new SelectList(addressRepo.GetItems(), "Id", "StreetName", order.AddressId);
            ViewBag.CustomerId = new SelectList(customerRepo.GetItems(), "Id", "Name", order.CustomerId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var order = orderRepo.GetItem((int)id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var order = orderRepo.GetItem(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            var products = order.OrderProducts;
            foreach (var item in products)
            {
                ReleaseItem(item.ProductId, item.AvailabeAmount);
            }
            orderRepo.DeleteItem(order.Id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                orderRepo.Dispose();
                orderProductRepo.Dispose();
                storageRepo.Dispose();
                customerRepo.Dispose();
                addressRepo.Dispose();

            }
            base.Dispose(disposing);
        }
    }

}
