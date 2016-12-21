using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kundbolaget.EntityFramework.Repositories;
using Kundbolaget.Models.EntityModels;

namespace Kundbolaget.Controllers
{
    public class CustomerController : Controller
    {
        IGenericRepository<Customer> repository;
        DbCustomerAddressRepository customerAdresses;

        public CustomerController()
        {
            repository = new DbCustomerRepository();
            customerAdresses = new DbCustomerAddressRepository();
        }

        public CustomerController(DbCustomerRepository dbCustomer, DbCustomerAddressRepository dbCustomerAddress)
        {
            repository = dbCustomer;
            customerAdresses = dbCustomerAddress;
        }

        // GET: Customer
        public ActionResult Index()
        {
            var model = repository.GetItems();
            return View(model);
        }

        // GET: Customer/Create
        public ActionResult Create()
        {
            return View();
        }

        // Get: Customers/Create
        [HttpPost]
        public ActionResult Create(Customer model)
        {
            if (!ModelState.IsValid)
                return View(model);
            repository.CreateItem(model);
            return RedirectToAction("Index");
        }

        // GET: Customers/Edit/{id}
        public ActionResult Edit(int id)
        {
            var model = repository.GetItem(id);
            return View(model);
        }

        // POST: Customers/Edit/{id}
        [HttpPost]
        public ActionResult Edit(Customer model)
        {
            if (!ModelState.IsValid)
                return View(model);
            repository.UpdateItem(model);
            return RedirectToAction("Index");
        }

        // GET: Customers/Details/{id}
        public ActionResult Details(int id)
        {
            // TODO: Address list as partial view
            var model = repository.GetItem(id);
            var addresses = customerAdresses.GetItems(id);
            ViewBag.Addresses = addresses;
            ViewBag.Descriptions = new string[] { "AdressId för kundorder", (addresses.Length > 1) ? "Adresser" : "Adress", "Adresstyp" };
            return View(model);
        }

        // GET: Customer/Delete/{id}
        public ActionResult Delete(int id)
        {
            var model = repository.GetItem(id);
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
            repository.DeleteItem(id);
            return RedirectToAction("Index");
        }
    }
}