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
        DbProductRepository repository;

        public ProductsController()
        {
            repository = new DbProductRepository();
        }

        // GET: Products
        public ActionResult Index()
        {
            var model = repository.GetItems();
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
            repository.CreateItem(model);
            return RedirectToAction("Index");
        }

        // GET: Products/Edit/{id}
        public ActionResult Edit(int id)
        {
            var model = repository.GetItem(id);
            return View(model);
        }

        // POST: Products/Edit/{id}
        [HttpPost]
        public ActionResult Edit(Product model)
        {
            if (!ModelState.IsValid)
                return View(model);
            repository.UpdateItem(model);
            return RedirectToAction("Index");
        }

        // GET: Products/Details/{id}
        public ActionResult Details(int id)
        {
            var model = repository.GetItem(id);
            return View(model);
        }

        // GET: Products/Delete/{id}
        public ActionResult Delete(int id)
        {
            var model = repository.GetItem(id);
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
            repository.DeleteItem(id);
            return RedirectToAction("Index");
        }

        public ActionResult AddToWarehouse(int id)
        {
            var prod = repository.GetItem(id);

            List<SelectListItem> listItems = repository.GetWarehouses().Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();

            CreateProductVM model = new CreateProductVM { Id = prod.Id, Name = prod.Name,
                 WarehouseList = listItems };

            return View(model);
        }

        [HttpPost]
        public ActionResult AddToWarehouse(CreateProductVM model)
        {

            if (!ModelState.IsValid)
                return View(model);

            repository.AddToStorage(model);

            return RedirectToAction("Index");
        }

    }
}