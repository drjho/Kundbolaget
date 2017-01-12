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
using System.Runtime.CompilerServices;
using Castle.Core.Internal;
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
        private DbAlcoholLicenseRepository licenseRepo;
        private DbProductRepository productRepository;
        private DbPriceListRepository priceListRepository;

        public OrdersController()
        {
            StoreContext db = new StoreContext();
            orderRepo = new DbOrderRepository(db);
            storageRepo = new DbStoragePlaceRepository(db);
            orderProductRepo = new DbOrderProductRepository(db);
            pickingOrderRepo = new DbPickingOrderRepository(db);

            addressRepo = new DbAddressRepository(db);
            customerRepo = new DbCustomerRepository(db);
            licenseRepo = new DbAlcoholLicenseRepository(db);
            productRepository = new DbProductRepository(db);
            priceListRepository = new DbPriceListRepository(db);
        }

        public OrdersController(DbOrderRepository dbOrderRepo, DbAddressRepository dbAddressRepo,
            DbStoragePlaceRepository dbStorageRepo, DbOrderProductRepository dbOrderProductRepo,
            DbCustomerRepository dbCustomerRepo, DbPickingOrderRepository dbPickingOrderRepo,
            DbAlcoholLicenseRepository dbLicenseRepo)
        {
            // TODO: update input for test.
            orderRepo = dbOrderRepo;
            storageRepo = dbStorageRepo;
            orderProductRepo = dbOrderProductRepo;
            pickingOrderRepo = dbPickingOrderRepo;

            addressRepo = dbAddressRepo;
            customerRepo = dbCustomerRepo;
            licenseRepo = dbLicenseRepo;
        }

        // GET: Orders
        public ActionResult Index(DateTime? searchDate)
        {
            //var model = orderRepo.GetItems().Where(p => p.OrderStatus == OrderStatus.Behandlar);
            var model = orderRepo.GetItems();
            if (searchDate.HasValue)
                model = model.Where(x => x.PlannedDeliveryDate == searchDate).ToArray();
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

            ViewBag.OrderId = order.Id;

            var model = new List<PickingOrder>();
            foreach (var item in order.OrderProducts)
            {
                model.AddRange(item.PickList);
            }
            if (model.Count < 1)
            {
                // TODO: Meddela användare att det inte finns några plockordrar när man behandlade ordern.
                ModelState.AddModelError("", "Order har ingen plockordrar.");
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

        public ActionResult DeliveryNotes(DateTime? searchDate)
        {
            var orders = orderRepo.GetItems().Where(x => x.OrderStatus == OrderStatus.Fraktar).ToArray();
            if (searchDate.HasValue)
                orders = orders.Where(x => x.PlannedDeliveryDate == searchDate).ToArray();
            return View(orders);
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
        /// Ändrar statusen av en order till fakturerar. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ReadyForInvoice(int? id)
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
            order.OrderStatus = OrderStatus.Fakturerar;
            orderRepo.UpdateItem(order);
            return RedirectToAction("Index");
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
                Comment = order.Comment
            };

            // Kolla alkolicens
            if (licenseRepo.GetItems().SingleOrDefault(x => x.CustomerId == order.CustomerId && x.StartDate.CompareTo(order.OrderDate) <= 0 && x.EndDate.CompareTo(order.OrderDate) >= 0) == null)
            {
                ModelState.AddModelError("", "Kunden har ingen alkohollicens.");
                orderVM.CannotContinueRegsitration = true;
            }

            var priceList = priceListRepository.GetItems().Where(x => x.CustomerGroupId == order.Customer.CustomerGroupId);

            var orderProductVMs = new List<OrderProductVM>();
            var backOrder = new Order
            {
                CustomerId = order.CustomerId,
                AddressId = order.AddressId,
                Comment = "*Restorder* " + order.Comment,
                OrderDate = DateTime.Now.Date,
                DesiredDeliveryDate = order.DesiredDeliveryDate,
                OrderStatus = OrderStatus.Behandlar,
                OrderProducts = new List<OrderProduct>()
            };

            // Kolla lagersaldot och försök att reservera.
            foreach (var op in order.OrderProducts)
            {
                // Kolla om produkten säljs.
                if (op.Product.ProductStatus != ProductStatus.Normal)
                {
                    backOrder.OrderProducts.Add(CreateBackOrder(op));
                    continue;
                }

                // Försök att reservera om det går.
                ReserveItem(op);
                // Få fram plockordrar på nytt.
                var pickList = pickingOrderRepo.GetItems().Where(x => x.OrderProductId == op.Id);
                // Ta fram reserverat antal och uppdatera availableAmount
                op.AvailabeAmount = pickList.Sum(x => x.ReservedAmount);
                orderProductRepo.UpdateItem(op);
                // Räkna fram priset.
                float productTotalPrice = 0;
                float unitPrice = 0;
                var productPrice = priceList.SingleOrDefault(x => x.ProductId == op.ProductId);
                if (productPrice == null)
                {
                    ModelState.AddModelError("", $"Kunden har inget pris för {op.Product.ShortDescription}.");
                    orderVM.CannotContinueRegsitration = true;

                }
                else
                {
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
                    unitPrice = productPrice.Price * (op.AvailabeAmount > limit ? (1 - productPrice.RebatePerPallet * .01f) : 1);
                    productTotalPrice = op.AvailabeAmount * unitPrice;
                }

                // Om man har lyckats reservera varor i lagret.
                if (op.AvailabeAmount > 0)
                {
                    var opvm = new OrderProductVM
                    {
                        Id = op.Id,
                        ProductId = (int)op.ProductId,
                        OrderedAmount = op.OrderedAmount,
                        AvailabeAmount = op.AvailabeAmount,
                        Price = productTotalPrice,
                        UnitPrice = unitPrice,
                        Comment = op.Comment
                    };
                    orderProductVMs.Add(opvm);

                    // Skapa rest order om lagret inte har tillräckligt många varor. 
                    if (op.OrderedAmount > op.AvailabeAmount)
                    {
                        // Sparar undan restorder tills vidare.
                        backOrder.OrderProducts.Add(CreateBackOrder(op));  
                        // Uppdatera beställt antal till det reserverade eftersom resten finns på restorder.
                        op.OrderedAmount = op.AvailabeAmount;
                        // Uppdatera databasen.
                        orderProductRepo.UpdateItem(op);
                    }
                }

            }
            orderVM.OrderProducts = orderProductVMs;

            // Jämför kreditgräns.
            var totalPrice = orderProductVMs.Sum(x => x.Price);

            if (order.Customer.CreditLimit != -1 && totalPrice > order.Customer.CreditLimit)
            {
                orderVM.CannotContinueRegsitration = true;
                ModelState.AddModelError("Price", $"Kundens kredigräns är överstigen ({order.Customer.CreditLimit}).");
            }
            orderVM.Price = totalPrice;

            if (totalPrice == 0)
            {
                orderVM.CannotContinueRegsitration = true;
                ModelState.AddModelError("", "Det finns ingen plockorder att skapa.");
            }
            else
            {
                if (backOrder.OrderProducts.Count > 0)
                {
                    orderRepo.CreateItem(backOrder);
                    ModelState.AddModelError("", "Restorder har genererats.");
                }
            }

            return View(orderVM);
        }

        public OrderProduct CreateBackOrder(OrderProduct op)
        {
            var backOrderProduct = new OrderProduct
            {
                OrderId = op.OrderId,
                ProductId = op.ProductId,
                OrderedAmount = op.OrderedAmount - op.AvailabeAmount,
                Comment = "Autogenererad restorder",
            };

            return backOrderProduct;
        }

        /// <summary>
        /// Reserverar en orderrad och lägg även till plockorder, sparas direkt i db.
        /// </summary>
        /// <param name="orderProduct"></param>
        public void ReserveItem(OrderProduct orderProduct)
        {
            // This assume that there is only 1 warehouse!!!
            int remainAmount = orderProduct.OrderedAmount - orderProduct.PickList.Sum(x => x.ReservedAmount);
            var storagePlaces = storageRepo.GetItems().Where(x => x.ProductId == orderProduct.ProductId).ToArray();
            var pickList = new List<PickingOrder>();
            var updatedSP = new List<StoragePlace>();
            foreach (var storage in storagePlaces)
            {
                int reserveAmount = Math.Min(storage.AvailableAmount, remainAmount);
                if (reserveAmount > 0)
                {
                    storage.ReservedAmount += reserveAmount;
                    updatedSP.Add(storage);
                    remainAmount -= reserveAmount;
                    pickList.Add(new PickingOrder { OrderProductId = orderProduct.Id, StoragePlaceId = storage.Id, ReservedAmount = reserveAmount });
                }
                if (remainAmount < 1)
                    break;
            }
            storageRepo.UpdateItems(updatedSP);
            pickingOrderRepo.CreateItems(pickList);
            return;
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
            if (!ModelState.IsValid)
            {
                return PrepareOrder(orderVM.Id);
            }

            var order = orderRepo.GetItem(orderVM.Id);
            order.OrderDate = orderVM.OrderDate;
            order.CustomerId = orderVM.CustomerId;
            order.PlannedDeliveryDate = orderVM.PlannedDeliveryDate;
            order.AddressId = orderVM.AddressId;
            order.Comment = orderVM.Comment;

            order.OrderStatus = OrderStatus.Plockar;
            orderRepo.UpdateItem(order);
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
            var updatedSP = new List<StoragePlace>();
            foreach (var sp in storagePlaces)
            {
                int pickAmount = Math.Min(sp.AvailableAmount, remainAmount);
                if (pickAmount > 0)
                {
                    pickList.Add(new PickingOrder { StoragePlaceId = sp.Id, PickedAmount = pickAmount, ReservedAmount = pickAmount });
                    sp.ReservedAmount += pickAmount;
                    updatedSP.Add(sp);
                    remainAmount -= pickAmount;
                }
                if (remainAmount < 1)
                    break;
            }
            storageRepo.UpdateItems(updatedSP);
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
            ViewBag.AddressId = new SelectList(addressRepo.GetItems(), "Id", "AddressString", order.AddressId);
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
