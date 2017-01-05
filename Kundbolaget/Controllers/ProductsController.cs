using Kundbolaget.EntityFramework.Context;
using Kundbolaget.EntityFramework.Repositories;
using Kundbolaget.Models.EntityModels;
using Kundbolaget.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kundbolaget.Controllers
{
    public class ProductsController : Controller
    {
        DbProductRepository productRepo;
        DbStoragePlaceRepository storageRepo;
        DbWarehouseRepository warehouseRepo;

        public ProductsController()
        {
            var db = new StoreContext();
            productRepo = new DbProductRepository(db);
            storageRepo = new DbStoragePlaceRepository(db);
            warehouseRepo = new DbWarehouseRepository(db);
        }

        public ProductsController(DbStoragePlaceRepository dbStorage, DbProductRepository dbproduct, DbWarehouseRepository dbwarehouse)
        {
            productRepo = dbproduct;
            storageRepo = dbStorage;
            warehouseRepo = dbwarehouse;
        }

       

        // GET: Products
        public ActionResult Index()
        {
            var model = productRepo.GetItems();
            return View(model);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            return View();
        }
        
        // POST: Products/Create
        [HttpPost]
        public ActionResult Create(Product model)
        {
            if (!ModelState.IsValid)
                return View(model);
            productRepo.CreateItem(model);
            return RedirectToAction("Index");
        }

        // GET: Products/Edit/{id}
        public ActionResult Edit(int id)
        {
            var model = productRepo.GetItem(id);
            return View(model);
        }

        // POST: Products/Edit/{id}
        [HttpPost]
        public ActionResult Edit(Product model)
        {
            if (!ModelState.IsValid)
                return View(model);
            productRepo.UpdateItem(model);
            return RedirectToAction("Index");
        }

        // GET: Products/Details/{id}
        public ActionResult Details(int id)
        {
            var model = productRepo.GetItem(id);
            return View(model);
        }

        // GET: Products/Delete/{id}
        public ActionResult Delete(int id)
        {
            var model = productRepo.GetItem(id);
            return View(model);
        }

        // POST: Products/Delete{id}
        [HttpPost]
        public ActionResult Delete(int id, Product model)
        {
            if (id != model.Id)
            {
                ModelState.AddModelError("Name", "Bad request");
                return View(model);
            }
            productRepo.DeleteItem(id);
            return RedirectToAction("Index");
        }
        //Todo: Detta används ej?
        //public ActionResult AddToWarehouse(int id)
        //{
        //    var prod = productRepo.GetItem(id);

        //    List<SelectListItem> listItems = productRepo.GetWarehouses().Select(c => new SelectListItem
        //    {
        //        Value = c.Id.ToString(),
        //        Text = c.Name
        //    }).ToList();

        //    CreateProductVM model = new CreateProductVM { Id = prod.Id, Name = prod.Name,
        //         WarehouseList = listItems };

        //    return View(model);
        //}

        //[HttpPost]
        //public ActionResult AddToWarehouse(CreateProductVM model)
        //{

        //    if (!ModelState.IsValid)
        //        return View(model);

        //    productRepo.AddToStorage(model);

        //    return RedirectToAction("Index");
        //}

    }
}