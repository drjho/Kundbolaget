using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kundbolaget.EntityFramework.Repositories;
using Kundbolaget.Models.EntityModels;
using Kundbolaget.Models.ViewModels;

namespace Kundbolaget.Controllers
{
    public class WarehouseController : Controller
    {
        DbWarehouseRepository warehouseRepo;
        DbProductRepository productRepo;
        DbStoragePlaceRepository storageRepo;

        int storageplacesPerWarehouse = 1452;

        public WarehouseController()
        {
            warehouseRepo = new DbWarehouseRepository();
            productRepo = new DbProductRepository();
            storageRepo = new DbStoragePlaceRepository();
        }

        public WarehouseController(DbWarehouseRepository fakeDbWarehouseRepository, DbStoragePlaceRepository fakeDbStoragePlaceRepository)
        {
            repository = fakeDbWarehouseRepository;
            storagePlaceRepository = fakeDbStoragePlaceRepository;
        }

        // GET: Warehouse
        public ActionResult Index()
        {
            var model = warehouseRepo.GetItems();
            return View(model);
        }

        //Get: Warehouse/Create
        public ActionResult Create()
        {
            return View();
        }

        // Get: Warehouse/Create
        [HttpPost]
        public ActionResult Create(Warehouse modelWarehouse)
        {
            if (!ModelState.IsValid)
                return View(modelWarehouse);
            warehouseRepo.CreateItem(modelWarehouse);
            for (int i = 0; i < storageplacesPerWarehouse; i++)
            {
                storageRepo.CreateItem(new StoragePlace { Id = i, WarehouseId = modelWarehouse.Id });
            }
            return RedirectToAction("Index");

        }


        // Get: Warehouse/Edit{id}
        public ActionResult Edit(int id)
        {
            var model = warehouseRepo.GetItem(id);
            return View(model);
        }

        //Post: Warehouse/Edit/{Id}
        [HttpPost]
        public ActionResult Edit(Warehouse modelWarehouse)
        {
            if (!ModelState.IsValid)
                return View(modelWarehouse);
            warehouseRepo.UpdateItem(modelWarehouse);
            return RedirectToAction("Index");
        }

        //Get: Warehouse/Details/{id}
        public ActionResult Details(int id)
        {
            var model = warehouseRepo.GetItem(id);
            return View(model);
        }

        protected override void Dispose(bool disposing)
        {
            warehouseRepo.Dispose();
            productRepo.Dispose();
            base.Dispose(disposing);
        }

        // GET: Warehouse/Delete/{id}
        public ActionResult Delete(int id)
        {
            var model = warehouseRepo.GetItem(id);
            return View(model);
        }

        // Post: Customer/Delete{Id}
        [HttpPost]
        public ActionResult Delete(int id, Warehouse model)
        {
            if (id != model.Id)
            {
               ModelState.AddModelError("Name", "Bad request");
                return View(model);
            }
            warehouseRepo.DeleteItem(id);
            var places = storageRepo.GetItems().Where(x => x.WarehouseId == id);
            foreach (var place in places)
            {
                storageRepo.DeleteItem(place.Id);
            }
            return RedirectToAction("Index");
        }

        public ActionResult UpdateStoragePlace(int id)
        {
            var model = storageRepo.GetItem(id);
            ViewBag.PlaceName = model.StoragePlaceId;
            ViewBag.ProductId = new SelectList(productRepo.GetItems(), "Id", "Name", model.ProductId);
            return View(new StoragePlaceProductVM { Id = id, ProductId = (int)model.ProductId, WarehouseId = model.WarehouseId, TotalAmount = model.TotalAmount, ReservedAmount = model.ReservedAmount });
        }

        [HttpPost]
        public ActionResult UpdateStoragePlace(StoragePlaceProductVM model)
        {
            ViewBag.ProductId = new SelectList(productRepo.GetItems(), "Id", "Name");
            if (!ModelState.IsValid)
                return View(model);
            if (!storageRepo.UpdateProduct(model.Id, model.ProductId, model.TotalAmount, model.ReservedAmount))
                return View(model);
            return RedirectToAction("Details", new { id = model.WarehouseId });
        }

        public ActionResult UnregisterProduct(int id)
        {
            var model = storageRepo.GetItem(id);
            ViewBag.PlaceName = model.StoragePlaceId;
            ViewBag.ProductId = new SelectList(productRepo.GetItems(), "Id", "Name", model.ProductId);
            return View(new StoragePlaceProductVM { Id = id, ProductId = (int)model.ProductId, WarehouseId = id, TotalAmount = model.TotalAmount, ReservedAmount = model.ReservedAmount });
        }

        [HttpPost]
        public ActionResult UnregisterProduct(StoragePlaceProductVM model)
        {
            ViewBag.ProductId = new SelectList(productRepo.GetItems(), "Id", "Name");
            if (!ModelState.IsValid)
                return View(model);
            if (!storageRepo.RemoveProduct(model.Id))
                return View(model);
            return RedirectToAction("Index");
        }

        public ActionResult AddProductToWareHouse(int id)
        {
            var model = warehouseRepo.GetItem(id);
            ViewBag.WarehouseName = model.Name;
            ViewBag.ProductId = new SelectList(productRepo.GetItems(), "Id", "Name");
            return View(new StoragePlaceProductVM { WarehouseId = id } );
        }

        [HttpPost]
        public ActionResult AddProductToWareHouse(StoragePlaceProductVM model)
        {
            ViewBag.ProductId = new SelectList(productRepo.GetItems(), "Id", "Name");
            if (!ModelState.IsValid)
                return View(model);
            if (!storageRepo.AddProduct(model.WarehouseId, model.ProductId, model.TotalAmount))
                return View(model);
            return RedirectToAction("Index");
        }
    }
}