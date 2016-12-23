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
        DbCustomerAddressRepository customerAddressRepo;
        DbCustomerRepository customerRepo;
        DbAddressRepository addressRepo;

        public CustomerAddressController()
        {
            customerAddressRepo = new DbCustomerAddressRepository();
            customerRepo = new DbCustomerRepository();
            addressRepo = new DbAddressRepository();
        }

        public CustomerAddressController(DbCustomerAddressRepository dbCustomerAdressRepository)
        {
            customerAddressRepo = dbCustomerAdressRepository;
        }

        public CustomerAddressController(DbCustomerAddressRepository dbCustomerAdressRepository, DbCustomerRepository dbCustomerRepository, DbAddressRepository dbAddressRepository)
        {
            customerAddressRepo = dbCustomerAdressRepository;
            customerRepo = dbCustomerRepository;
            addressRepo = dbAddressRepository;
        }

        // GET: CustomerAddress
        public ActionResult Index()
        {
            var model = customerAddressRepo.GetItems();
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
                return View(customerAddressRepo.GetItems());
            customerAddressRepo.CreateItem(modelCustomerAddress);
            return RedirectToAction("Index");

        }

        public ActionResult Edit(int id)
        {
            var model = customerAddressRepo.GetItem(id);
            if (model == null)
            {
                return HttpNotFound();
            }

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
            customerAddressRepo.UpdateItem(model);
            return RedirectToAction("Index");
        }

        //public ActionResult CustomerDetails(int id)
        //{
        //    var model = customerAddressRepo.GetItems(id);
        //    return View(model);
        //}

        // GET: CustomerAddress/Details/{id}
        public ActionResult Details(int id)
        {
            var model = customerAddressRepo.GetItem(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        // GET: CustomerAddress/Delete/{id}
        public ActionResult Delete(int id)
        {
            var model = customerAddressRepo.GetItem(id);
            if (model == null)
            {
                return HttpNotFound();
            }
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
            customerAddressRepo.DeleteItem(id);
            return RedirectToAction("Index");
        }
    }
}