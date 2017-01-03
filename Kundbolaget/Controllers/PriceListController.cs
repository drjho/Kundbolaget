using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kundbolaget.EntityFramework.Repositories;
using Kundbolaget.Models.EntityModels;
using Kundbolaget.Models.ViewModels;

namespace Kundbolaget.Controllers
{
    public class PriceListController : Controller
    {
        DbPriceListRepository repository;
        DbCustomerGroupRepository customerGroupRepository;

        public PriceListController()
        {
            repository = new DbPriceListRepository();
            customerGroupRepository = new DbCustomerGroupRepository();
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

        public ActionResult AddToCustomerGroup(int id)
        {
            var priceList = repository.GetItem(id);

            List<SelectListItem> listItems = customerGroupRepository.GetItems().Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();

            CustomerGroupVM model = new CustomerGroupVM {PriceListId = priceList.Id, CustomerGroupList = listItems};

            return View(model);
        }
    }
}