using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_Store.Models;

namespace Web_Store.Controllers
{
    public class HomeController : Controller
    {
        private webshopEntities db = new webshopEntities();
        // GET: Home
        [Route]
        public ActionResult Index()
        {
            return RedirectToAction("Login");
        }
        // GET: register
        [Route("Register")]
        public ActionResult Register()
        {
            return View();
        }
        // GET: register
        [Route("Login")]
        public ActionResult Login()
        {
            return View();
        }


        [Route("Register")]
        [HttpPost]
        public ActionResult Register(user user)
        {
            try
            {
                // TODO: Add insert logic here
                var userDetail = db.user.Where(x => x.Mail == user.Mail && x.Password == user.Password).FirstOrDefault();
                if (true)
                {
                    TempData["AlertMessage"] = "registerpost";
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        [Route("Login")]
        [HttpPost]
        public ActionResult Login(user user, string password)
        {
            try
                {
                var userDetail = db.user.Where(x => x.Mail == user.Mail && x.Password == user.Password).FirstOrDefault();
                // TODO: Add insert logic here
                if (true)
                {
                    TempData["AlertMessage"] = "loginpost";
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}