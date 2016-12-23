using Kundbolaget.EntityFramework.Context;
using Kundbolaget.EntityFramework.Repositories;
using Kundbolaget.Models.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Kundbolaget.Controllers
{
    public class AlcoholLicenseController : Controller
    {

        DbAlcoholLicenseRepository repository;

        public AlcoholLicenseController()
        {
            repository = new DbAlcoholLicenseRepository();
        }

        public AlcoholLicenseController(DbAlcoholLicenseRepository dbAlcoholLicenseRepository)
        {
            repository = dbAlcoholLicenseRepository;
        }

        // GET: AlcoholLicense
        public ActionResult Index()
        {
            var model = repository.GetItems();
            return View(model);
        }

        public ActionResult Create()
        {
            ViewBag.Customers = new SelectList(new StoreContext().Customers, "Id", "Name");

            return View();
        }

        [HttpPost]
        public ActionResult Create(AlcoholLicense model)
        {
            if (!ModelState.IsValid)
                return View(model);
            repository.CreateItem(model);

            return RedirectToAction("Index");
        }

        // GET: Alcohollicense/Edit/{id}
        public ActionResult Edit(int id)
        {
            var model = repository.GetItem(id);
            ViewBag.Customers = new SelectList(new StoreContext().Customers, "Id", "Name");

            return View(model);
        }

        // POST: Alcohollicense/Edit/{id}
        [HttpPost]
        public ActionResult Edit(AlcoholLicense model)
        {
            if (!ModelState.IsValid)
                return View(model);

            repository.UpdateItem(model);
            return RedirectToAction("Index");
        }

        // GET: Alcohollicense/Details/{id}
        public ActionResult Details(int id)
        {
            var model = repository.GetItem(id);
            return View(model);
        }

        // GET: Adress/Delete/{id}
        public ActionResult Delete(int id)
        {
            var model = repository.GetItem(id);
            return View(model);
        }

        // POST: Alcohollicense/Delete/{id}
        [HttpPost]
        public ActionResult Delete(int id, AlcoholLicense model)
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