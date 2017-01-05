using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.Mvc;
using Kundbolaget.EntityFramework.Context;
using Kundbolaget.EntityFramework.Repositories;
using Kundbolaget.Models.EntityModels;
using Kundbolaget.Models.ViewModels;

namespace Kundbolaget.Controllers
{
    public class PriceListController : Controller
    {
        DbPriceListRepository repository;
        DbCustomerGroupRepository customerGroupRepository;
        private DbProductRepository productRepository;
        

        public PriceListController()
        {
            StoreContext db = new StoreContext();
            repository = new DbPriceListRepository(db);
            customerGroupRepository = new DbCustomerGroupRepository(db);
            productRepository = new DbProductRepository(db);
        }

        public PriceListController(DbPriceListRepository dbPriceListRepository)
        {
            repository = dbPriceListRepository;
        }

        // GET: PriceList
        public ActionResult Index()
        {
            var model = repository.GetItems();
            return View(model);
        }
        //GET: PriceList/Create
        public ActionResult Create()
        {
            ViewBag.ProductId = new SelectList(productRepository.GetItems(), "Id", "Name");
            ViewBag.CustomerGroupId = new SelectList(customerGroupRepository.GetItems(), "Id", "Name");
            return View();
        }
        // POST: Products/Create
        [HttpPost]
        public ActionResult Create(PriceList model)
        {
            if (!ModelState.IsValid)
                return View(model);
            repository.CreateItem(model);
            return RedirectToAction("Index");
        }

        //GET: PriceList/Edit{id}
        public ActionResult Edit(int id)
        {
            var model = repository.GetItem(id);
            ViewBag.ProductId = new SelectList(productRepository.GetItems(), "Id", "Name");
            ViewBag.CustomerGroupId = new SelectList(customerGroupRepository.GetItems(), "Id", "Name");
            return View(model);
        }
        //Post: PriceList/Edit{id}
        [HttpPost]
        public ActionResult Edit(PriceList model)
        {
            if (!ModelState.IsValid)
                return View(model);
            repository.UpdateItem(model);
            return RedirectToAction("Index");
        }

        // GET: PriceList/Details/{id}
        public ActionResult Details(int id)
        {
            var model = repository.GetItem(id);
            return View(model);
        }
        // GET: PriceList/Delete/{id}
        public ActionResult Delete(int id)
        {
            var model = repository.GetItem(id);
            return View(model);
        }
        // POST: PriceList/Delete{id}
        [HttpPost]
        public ActionResult Delete(int id, PriceList model)
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