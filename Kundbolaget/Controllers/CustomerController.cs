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
        IStoreRepository repository;

        public CustomerController()
        {
            repository = new DbStoreRepository();
        }

        // GET: Customer
        public ActionResult Index()
        {
            
            return View();
        }

        public ActionResult CreateCustomer()
        {
            return View();
        }

        //[HttpPost]
        //public ActionResult CreateCustomer(Customer model)
        //{
        //    if (!ModelState.IsValid)
        //        return View(model);
        //    repository.(model);
        //    return RedirectToAction("Index")
        //};
    }
}