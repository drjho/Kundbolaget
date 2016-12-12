using Kundbolaget.EntityFramework.Repositories;
using Kundbolaget.Models.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kundbolaget.Controllers
{
    public class UserController : Controller
    {
        IGenericRepository<User> repository;
        public UserController()
        {
            repository = new DbUserRepository();
        }
         // GET: User
        public ActionResult Index()
        {
            var model = repository.GetItems();
            return View(model);
        }

        // GET: User/Create
        public ActionResult Create()
        {
            return View();
        }

        // Get: User/Create
        [HttpPost]
        public ActionResult Create(User model)
        {
            if (!ModelState.IsValid)
                return View(model);
            repository.CreateItem(model);
            return RedirectToAction("Index");
        }

        // GET: User/Edit/{id}
        public ActionResult Edit(int id)
        {
            var model = repository.GetItem(id);
            return View(model);
        }

        // POST: User/Edit/{id}
        [HttpPost]
        public ActionResult Edit(User model)
        {
            if (!ModelState.IsValid)
                return View(model);
            repository.UpdateItem(model);
            return RedirectToAction("Index");
        }

        // GET: User/Details/{id}
        //public ActionResult Details(int id)
        //{
        //    var model = repository.GetItem(id);
        //    return View(model);
        //}

        // GET: User/Delete/{id}
        public ActionResult Delete(int id)
        {
            var model = repository.GetItem(id);
            return View(model);
        }

        // POST: User/Delete{id}
        [HttpPost]
        public ActionResult Delete(int id, User model)
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