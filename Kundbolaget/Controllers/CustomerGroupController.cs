﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kundbolaget.EntityFramework.Context;
using Kundbolaget.EntityFramework.Repositories;
using Kundbolaget.Models.EntityModels;

namespace Kundbolaget.Controllers
{
    public class CustomerGroupController : Controller
    {
        DbCustomerGroupRepository repository;
        private DbCustomerRepository customerRepository;

        public CustomerGroupController()
        {
            customerRepository = new DbCustomerRepository();
            repository = new DbCustomerGroupRepository();
        }

        public CustomerGroupController(DbCustomerGroupRepository dbCustomerGroupRepository)
        {
            repository = dbCustomerGroupRepository;
        }

        // GET: CustomerGroup
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
        public ActionResult Create(CustomerGroup model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            repository.CreateItem(model);
            return RedirectToAction("Index");
        }
        //Todo: I Edit visas inte vilken kundgrupp som tidigare varit vald.
        //Get: CustomerGroup/Edit/{Id}
        public ActionResult Edit(int id)
        {
            var model = repository.GetItem(id);
            return View(model);
        }
        //POST: CustomerGroups/Edit/{Id}
        [HttpPost]
        public ActionResult Edit(CustomerGroup model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            repository.UpdateItem(model);
            return RedirectToAction("Index");
        }
        //GET: CustomerGroups/Details/{Id}
        public ActionResult Details(int id)
        {
            var model = repository.GetItem(id);
            return View(model);
        }
        //GET: CustomerGroups/Delete/{id}
        public ActionResult Delete(int id)
        {
            var model = repository.GetItem(id);
            return View(model);
        }
        // POST: Address/Delete{id}
        [HttpPost]
        public ActionResult Delete(int id, CustomerGroup model)
        {
            if (id != model.Id)
            {
                ModelState.AddModelError("Name", "Bad request");
            }
            repository.DeleteItem(id);
            return RedirectToAction("Index");
        }


    }
}
