﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kundbolaget.EntityFramework.Repositories;
using Kundbolaget.Models.EntityModels;
using Kundbolaget.Models.ViewModels;

namespace Kundbolaget.Controllers
{
    public class WarehouseController : Controller
    {
        DbWarehouseRepository repository;

        public WarehouseController()
        {
            repository = new DbWarehouseRepository();

        }
        // GET: Warehouse
        public ActionResult Index()
        {
            var model = repository.GetItems();
            return View(model);
        }

        //Get: Warehouse/Create
        public ActionResult Create()
        {
            return View();
        }

        // Get: Warehouse/Create
        [HttpPost]
        public ActionResult Create(Warehouse modelWarehouse)
        {
            if (!ModelState.IsValid)
                return View(modelWarehouse);
            repository.CreateItem(modelWarehouse);
            return RedirectToAction("Index");

        }
        // Get: Warehouse/Edit{id}
        public ActionResult Edit(int id)
        {
            var model = repository.GetItem(id);
            return View(model);
        }

        //Post: Warehouse/Edit/{Id}
        [HttpPost]
        public ActionResult Edit(Warehouse modelWarehouse)
        {
            if (!ModelState.IsValid)
                return View(modelWarehouse);
            repository.UpdateItem(modelWarehouse);
            return RedirectToAction("Index");
        }

        //Get: Warehouse/Details/{id}
        public ActionResult Details(int id)
        {
            WarehouseVM model = new WarehouseVM
            {
                Id = id,
                Places = repository.GetOccupiedStoragePlaces(id).ToList(),
            };

            return View(model);
        }

        // GET: Warehouse/Delete/{id}
        public ActionResult Delete(int id)
        {
            var model = repository.GetItem(id);
            return View(model);
        }
        // Post: Customer/Delete{Id}
        [HttpPost]
        public ActionResult Delete(int id, Warehouse model)
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