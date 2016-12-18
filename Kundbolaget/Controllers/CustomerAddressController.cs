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
    public class CustomerAddressController : Controller
    {
        DbCustomerAddressRepository repository;
        DbAddressRepository addressRepo = new DbAddressRepository();
        DbCustomerRepository customerRepo = new DbCustomerRepository();

        public CustomerAddressController()
        {
            repository = new DbCustomerAddressRepository();
        }

        // GET: CustomerAddress
        public ActionResult Index()
        {
            var model = repository.GetItems();
            return View(model);
        }

        public ActionResult Create()
        {

            ViewBag.AddressId = new SelectList(addressRepo.GetItems(), "Id", "AddressString");
            ViewBag.CustomerId = new SelectList(customerRepo.GetItems(), "Id", "Name");

            return View();
        }

        // POST: CustomerAddress/Create/{id}
        [HttpPost]
        public ActionResult Create(CustomerAddress modelCustomerAddress)
        {
            if (!ModelState.IsValid)
                return View(repository.GetItems());
            repository.CreateItem(modelCustomerAddress);
            return RedirectToAction("Index");

        }

        public ActionResult Edit(int id)
        {
            var model = repository.GetItem(id);

            ViewBag.AddressId = new SelectList(addressRepo.GetItems(), "Id", "AddressString", model.AddressId);
            ViewBag.CustomerId = new SelectList(customerRepo.GetItems(), "Id", "Name", model.CustomerId);

            return View(model);
        }

        // POST: CustomerAddress/Edit/{id}
        [HttpPost]
        public ActionResult Edit(CustomerAddress model)
        {
            if (!ModelState.IsValid)
                return View(model);
            repository.UpdateItem(model);
            return RedirectToAction("Index");
        }

        public ActionResult CustomerDetails(int id)
        {
            var model = repository.GetItems(id);
            return View(model);
        }

        // GET: CustomerAddress/Details/{id}
        public ActionResult Details(int id)
        {
            var model = repository.GetItem(id);
            return View(model);
        }

        // GET: CustomerAddress/Delete/{id}
        public ActionResult Delete(int id)
        {
            var model = repository.GetItem(id);
            return View(model);
        }

        // POST: Products/Delete{id}
        [HttpPost]
        public ActionResult Delete(int id, CustomerAddress model)
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