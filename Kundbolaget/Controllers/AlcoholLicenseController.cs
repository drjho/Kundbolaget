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

        DbAlcoholLicenseRepository licenseRepo;
        DbCustomerRepository customerRepo;

        public AlcoholLicenseController()
        {
            var db = new StoreContext();
            licenseRepo = new DbAlcoholLicenseRepository(db);
            customerRepo = new DbCustomerRepository(db);
        }

        public AlcoholLicenseController(DbAlcoholLicenseRepository dbAlcoholLicenseRepository)
        {
            licenseRepo = dbAlcoholLicenseRepository;
        }

        // GET: AlcoholLicense
        public ActionResult Index()
        {
            var model = licenseRepo.GetItems();
            return View(model);
        }

        public ActionResult Create()
        {
            ViewBag.CustomerId = new SelectList(customerRepo.GetItems(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult Create(AlcoholLicense model)
        {
            if (!ModelState.IsValid)
                return View(model);
            licenseRepo.CreateItem(model);

            return RedirectToAction("Index");
        }

        // GET: Alcohollicense/Edit/{id}
        public ActionResult Edit(int id)
        {
            var model = licenseRepo.GetItem(id);
            ViewBag.CustomerId = new SelectList(customerRepo.GetItems(), "Id", "Name", model.CustomerId);
            return View(model);
        }

        // POST: Alcohollicense/Edit/{id}
        [HttpPost]
        public ActionResult Edit(AlcoholLicense model)
        {
            if (!ModelState.IsValid)
                return View(model);

            licenseRepo.UpdateItem(model);
            return RedirectToAction("Index");
        }

        // GET: Alcohollicense/Details/{id}
        public ActionResult Details(int id)
        {
            var model = licenseRepo.GetItem(id);
            return View(model);
        }

        // GET: Adress/Delete/{id}
        public ActionResult Delete(int id)
        {
            var model = licenseRepo.GetItem(id);
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
            licenseRepo.DeleteItem(id);
            return RedirectToAction("Index");
        }
            
    }
}