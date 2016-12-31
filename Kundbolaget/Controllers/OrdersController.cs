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
using System.Reflection;
using Kundbolaget.Models.ViewModels;

namespace Kundbolaget.Controllers
{
    public class OrdersController : Controller
    {
        //private StoreContext db = new StoreContext();
        private DbOrderRepository orderRepo;
        private DbStoragePlaceRepository storageRepo;
        private DbOrderProductRepository orderProductRepo;
        private DbPickingOrderRepository pickingOrderRepo;

        private DbAddressRepository addressRepo;
        private DbCustomerRepository customerRepo;

        public OrdersController()
        {
            StoreContext db = new StoreContext();
            orderRepo = new DbOrderRepository(db);
            storageRepo = new DbStoragePlaceRepository(db);
            orderProductRepo = new DbOrderProductRepository(db);
            pickingOrderRepo = new DbPickingOrderRepository(db);

            addressRepo = new DbAddressRepository();
            customerRepo = new DbCustomerRepository();
        }

        public OrdersController(DbOrderRepository dbOrderRepo, DbAddressRepository dbAddressRepo,
            DbStoragePlaceRepository dbStorageRepo, DbOrderProductRepository dbOrderProductRepo,
            DbCustomerRepository dbCustomerRepo, DbPickingOrderRepository dbPickingOrderRepo)
        {
            // TODO: update input for test.
            orderRepo = dbOrderRepo;
            storageRepo = dbStorageRepo;
            orderProductRepo = dbOrderProductRepo;
            pickingOrderRepo = dbPickingOrderRepo;

            addressRepo = dbAddressRepo;
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

        public ActionResult ShowPickingOrder(int? id)
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
            var model = new List<PickingOrder>();
            foreach (var item in order.OrderProducts)
            {
                model.AddRange(item.PickList);
            }
            return View(model);
        }

        //[HttpPost]
        public ActionResult CreateDeliveryNote(int? id)
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
            var pickList = new List<PickingOrder>();

            foreach (var item in order.OrderProducts)
            {
                pickList.AddRange(item.PickList);
                item.DeliveredAmount = item.PickList.Sum(x => x.PickedAmount);
            }

            UpdateStoragePlaceAmount(pickList);

            orderProductRepo.UpdateItems(order.OrderProducts);

            order.OrderStatus = OrderStatus.Fraktar;
            orderRepo.UpdateItem(order);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Anropas när man har plockat varorna från lagret och totala antalet minskas med 
        /// antalet plockade.
        /// </summary>
        /// <remarks>
        /// Vi antar att man plockar det reserverade antalet, går det inte så beror det på att 
        /// det har försvunnit eller det har gått sönder. Därför uppdateras storagePlace med 
        /// bara det reserverade antalet.
        /// </remarks>
        /// <param name="pickList"></param>
        public void UpdateStoragePlaceAmount(List<PickingOrder> pickList)
        {
            var updatedStorages = new List<StoragePlace>();
            foreach (var item in pickList)
            {
                var storage = storageRepo.GetItem((int)item.StoragePlaceId);
                storage.ReservedAmount -= item.ReservedAmount;
                storage.TotalAmount -= item.ReservedAmount;
                updatedStorages.Add(storage);
            }
            storageRepo.UpdateItems(updatedStorages);
        }

        /// <summary>
        /// Anropas när man vill visa följsedel av en order
        /// </summary>
        /// <param name="id">Id av en order</param>
        /// <returns></returns>
        public ActionResult ShowDeliveryNote(int? id)
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


        /// <summary>
        /// Denna anropas när en order är levererad och man kan ändra antal levererade varor.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult FinalizeDelivery(int? id)
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
            order.OrderStatus = OrderStatus.Levererad;
            orderRepo.UpdateItem(order);
            return View(order);
        }


        /// <summary>
        /// När en order är fakturerad, ändras statusen till Arkiverad
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ArchiveOrder(int? id)
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
            order.OrderStatus = OrderStatus.Arkiverad;
            orderRepo.UpdateItem(order);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Förbereder order med produkterna i en viewmodel innan man går vidare att skapa en plocksedel.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Användaren har kollat att ordern stämmer. Då blir varorna reserverade i lagret.
        /// Antalet reserverade kolli registreras i 'OrderProduct'.
        /// En plock sedel skapas och orderns status ändras till 'Plockar'. 
        /// </summary>
        /// <remarks>
        /// Denna version gör inget speciellt om varorna inte finns i lagret. 
        /// </remarks>
        /// <param name="orderVM"></param>
        /// <returns></returns>
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
            //for (int i = 0; i < order.OrderProducts.Count; i++)
            //var orderProduct = order.OrderProducts[i];
            foreach (var orderProduct in order.OrderProducts)
            {
                orderProduct.PickList = ReserveItem(orderProduct.ProductId, orderProduct.OrderedAmount);
                orderProduct.PickList.ForEach(x => x.OrderProductId = orderProduct.Id);
                orderProduct.AvailabeAmount = orderProduct.PickList.Sum(x => x.ReservedAmount);

                // Vad gör man om det blir en tomlista?

                // Funkar inte? Hur visar vi att availableAmount är 0?                
                //if (orderProduct.AvailabeAmount == 0)
                //{
                //    ModelState.AddModelError("AvailabeAmount", "Det finns inga varor i lager");
                //    // Varför har orderVM inget orderproducts helt plötsligt?!
                //    return View(orderVM);
                //}

                pickingOrderRepo.CreateItems(orderProduct.PickList);
            }

            order.OrderStatus = OrderStatus.Plockar;
            orderRepo.UpdateItem(order);
            orderProductRepo.UpdateItems(order.OrderProducts);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Indata: Produktid och beställt antal.
        /// Om det inte finns i lager blir det en tom lista.
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

            foreach (var sp in storagePlaces)
            {
                int pickAmount = Math.Min(sp.AvailableAmount, remainAmount);
                if (pickAmount > 0)
                {
                    pickList.Add(new PickingOrder { StoragePlaceId = sp.Id, ReservedAmount = pickAmount });
                    sp.ReservedAmount += pickAmount;
                    remainAmount -= pickAmount;
                }
                if (remainAmount < 1)
                    break;
            }
            storageRepo.UpdateItems(storagePlaces);
            return pickList;
        }

        /// <summary>
        /// Anropas när man tar bort en order så först ska reserverade varorna i lagret släppas,
        /// sedan ta man bort plocksedlarna från systemet.
        /// </summary>
        /// <param name="pickList"></param>
        public void ReleaseItem(List<PickingOrder> pickList)
        {
            var storages = new List<StoragePlace>();
            for (int i = 0; i < pickList.Count; i++)
            {
                var p = pickList[i];
                var s = storageRepo.GetItem((int)p.StoragePlaceId);
                // Avoid having negative reserved amount (should not happen but just in case)
                s.ReservedAmount -= Math.Min(s.ReservedAmount, p.ReservedAmount);
                storages.Add(s);
            }
            storageRepo.UpdateItems(storages);
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

        /// <summary>
        /// Anropas när en order ska tas bort från systemet. Om statusen är innan frakt så 
        /// släpps de reserverade varorna i lagret.
        /// Tillhörande 'OrderPrducts' och 'PickingOrder' tas bort från databasen.
        /// Till slut tas ordern bort från databasen
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
            var pickList = new List<PickingOrder>();
            for (int i = 0; i < products.Count; i++)
            {
                var item = products[i];
                pickList.AddRange(item.PickList);
                if (order.OrderStatus < OrderStatus.Fraktar)
                    ReleaseItem(item.PickList);
            }
            pickingOrderRepo.DeleteItems(pickList);
            orderProductRepo.DeleteItems(products);
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
