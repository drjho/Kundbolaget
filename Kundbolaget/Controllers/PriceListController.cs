using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kundbolaget.EntityFramework.Repositories;
using Kundbolaget.Models.EntityModels;

namespace Kundbolaget.Controllers
{
    public class PriceListController : Controller
    {
        DbPriceListRepository repository;

        public PriceListController()
        {
            repository = new DbPriceListRepository();
        }
        // GET: PriceList
        public ActionResult Index()
        {
            var model = repository.GetItems();
            return View(model);
        }
        //GET: PriceList/Create
        public ActionResult Create()
        {
            return View();
        }
        // POST: Products/Create
        [HttpPost]
        public ActionResult Create(PriceList model)
        {
            if (!ModelState.IsValid)
                return View(model);
            repository.CreateItem(model);
            return RedirectToAction("Index");
        }

        //GET: PriceList/Edit{id}
        public ActionResult Edit(int id)
        {
            var model = repository.GetItem(id);
            return View(model);
        }
        //Post: PriceList/Edit{id}
        [HttpPost]
        public ActionResult Edit(PriceList model)
        {
            if (!ModelState.IsValid)
                return View(model);
            repository.UpdateItem(model);
            return RedirectToAction("Index");
        }

        // GET: PriceList/Details/{id}
        public ActionResult Details(int id)
        {
            var model = repository.GetItem(id);
            return View(model);
        }
        // GET: PriceList/Delete/{id}
        public ActionResult Delete(int id)
        {
            var model = repository.GetItem(id);
            return View(model);
        }
        // POST: PriceList/Delete{id}
        [HttpPost]
        public ActionResult Delete(int id, PriceList model)
        {
            if (id != model.Id)
            {
                ModelState.AddModelError("Name", "Bad request");
                return View(model);
            }
            repository.DeleteItem(id);
            return RedirectToAction("Index");
        }
    }
}