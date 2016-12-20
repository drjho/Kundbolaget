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
    public class CustomerAlcoholLicenseController : Controller
    {
        DbCustomerAlcoholLicenseRepository repository;

        public CustomerAlcoholLicenseController()
        {
            repository = new DbCustomerAlcoholLicenseRepository();
        }

        // GET: CustomerAlcoholLicense
        public ActionResult Index()
        {
            var model = repository.GetItems();
            return View(model);
        }

        //public ActionResult Create()
        //{

        //    List<SelectListItem> addresses = repository.GetAddress().
        //        Select(a => new SelectListItem
        //        {
        //            Value = a.Id.ToString(),
        //            Text = a.AddressString
        //        }).ToList();

        //    List<SelectListItem> customers = repository.GetCustomers().
        //     Select(c => new SelectListItem
        //     {
        //         Value = c.Id.ToString(),
        //         Text = c.Name
        //     }).ToList();

        //    var model = new CustomerAddressVM { AddressList = addresses, CustomerList = customers };
        //    return View(model);
        //}

        // POST: CustomerAlcoholLicense/Create/{id}
        [HttpPost]
        public ActionResult Create(CustomerAlcoholLicense modelCustomerAlcoholLicense)
        {
            if (!ModelState.IsValid)
                return View(repository.GetItems());
            repository.CreateItem(modelCustomerAlcoholLicense);
            return RedirectToAction("Index");
        }

        //public ActionResult Edit(int id)
        //{
        //    var item = repository.GetItem(id);
        //    List<SelectListItem> addresses = repository.GetAddress().
        //        Select(a => new SelectListItem
        //        {
        //            Value = a.Id.ToString(),
        //            Text = a.AddressString
        //        }).ToList();

        //    List<SelectListItem> customers = repository.GetCustomers().
        //        Select(c => new SelectListItem
        //        {
        //            Value = c.Id.ToString(),
        //            Text = c.Name
        //        }).ToList();

        //    var model = new CustomerAddressVM
        //    {
        //        Id = item.Id,
        //        AddressList = addresses,
        //        CustomerList = customers
        //    };
        //    return View(model);
        //}

        // GET: CustomerAlcoholLicense/Details/{id}
        public ActionResult Details(int id)
        {
            var model = repository.GetItem(id);
            return View(model);
        }

        // GET: CustomerAlcoholLicense/Delete/{id}
        public ActionResult Delete(int id)
        {
            var model = repository.GetItem(id);
            return View(model);
        }

        // POST: Products/Delete{id}
        [HttpPost]
        public ActionResult Delete(int id, CustomerAlcoholLicense model)
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