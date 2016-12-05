using Kundbolaget.EntityFramework.Repositories;
using Kundbolaget.Models.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kundbolaget.Controllers
{
    public class AddressController : Controller
    {

        DbAddressRepository repository;

        public AddressController()
        {
            repository = new DbAddressRepository();
        }

        // GET: Address
        public ActionResult Index()
        {
            var model = repository.GetItems();
            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Address model)
        {
            if (!ModelState.IsValid)
                return View(model);
            repository.CreateItem(model);
            return RedirectToAction("Index");
        }

        // GET: Addresses/Edit/{id}
        public ActionResult Edit(int id)
        {
            var model = repository.GetItem(id);
            return View(model);
        }

        // POST: Addresses/Edit/{id}
        [HttpPost]
        public ActionResult Edit(Address model)
        {
            if (!ModelState.IsValid)
                return View(model);
            repository.UpdateItem(model);
            return RedirectToAction("Index");
        }

        // GET: Addresses/Details/{id}
        public ActionResult Details(int id)
        {
            var model = repository.GetItem(id);
            return View(model);
        }

        // GET: Address/Delete/{id}
        public ActionResult Delete(int id)
        {
            var model = repository.GetItem(id);
            return View(model);
        }

        // POST: Address/Delete{id}
        [HttpPost]
        public ActionResult Delete(int id, Address model)
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