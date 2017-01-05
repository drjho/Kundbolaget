using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kundbolaget.EntityFramework.Repositories;
using Kundbolaget.Models.EntityModels;
using Kundbolaget.EntityFramework.Context;

namespace Kundbolaget.Controllers
{
    public class CustomerController : Controller
    {
        DbCustomerRepository customerRepo;
        DbCustomerAddressRepository customerAdressRepo;
        DbCustomerGroupRepository customerGroupRepo;

        public CustomerController()
        {
            var db = new StoreContext();
            customerRepo = new DbCustomerRepository(db);
            customerAdressRepo = new DbCustomerAddressRepository(db);
            customerGroupRepo = new DbCustomerGroupRepository(db);
        }

        public CustomerController(DbCustomerRepository dbCustomer, DbCustomerAddressRepository dbCustomerAddress)
        {
            customerRepo = dbCustomer;
            customerAdressRepo = dbCustomerAddress;
        }

        // GET: Customer
        public ActionResult Index()
        {
            var model = customerRepo.GetItems();
            return View(model);
        }

        // GET: Customer/Create
        public ActionResult Create()
        {
            ViewBag.CustomerGroupId = new SelectList(customerGroupRepo.GetItems(), "Id", "Name");
            return View();
        }

        // Get: Customers/Create
        [HttpPost]
        public ActionResult Create(Customer model)
        {
            if (!ModelState.IsValid)
                return View(model);
            customerRepo.CreateItem(model);
            return RedirectToAction("Index");
        }

        // GET: Customers/Edit/{id}
        public ActionResult Edit(int id)
        {
            var model = customerRepo.GetItem(id);
            ViewBag.CustomerGroupId = new SelectList(customerGroupRepo.GetItems(), "Id", "Name");
            return View(model);
        }

        // POST: Customers/Edit/{id}
        [HttpPost]
        public ActionResult Edit(Customer model)
        {
            if (!ModelState.IsValid)
                return View(model);
            customerRepo.UpdateItem(model);
            return RedirectToAction("Index");
        }

        // GET: Customers/Details/{id}
        public ActionResult Details(int id)
        {
            // TODO: Address list as partial view
            var model = customerRepo.GetItem(id);
            var addresses = customerAdressRepo.GetItems(id);
            ViewBag.Addresses = addresses;
            ViewBag.Descriptions = new string[] { "AdressId för kundorder", (addresses.Length > 1) ? "Adresser" : "Adress", "Adresstyp" };
            return View(model);
        }

        // GET: Customer/Delete/{id}
        public ActionResult Delete(int id)
        {
            var model = customerRepo.GetItem(id);
            return View(model);
        }

        // POST: Customer/Delete{id}
        [HttpPost]
        public ActionResult Delete(int id, Customer model)
        {
            if (id != model.Id)
            {
                ModelState.AddModelError("Name", "Bad request");
                return View(model);
            }
            customerRepo.DeleteItem(id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            customerAdressRepo.Dispose();
            customerRepo.Dispose();
            base.Dispose(disposing);
        }
    }
}