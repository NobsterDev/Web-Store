using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_Store.Models;

namespace Web_Store.Controllers
{
    [RoutePrefix("User")]
    public class UserController : Controller
    {
        private webshopEntities db = new webshopEntities();
        // GET: User
        [Route]
        public ActionResult Index()
        {
            try { auth(); } catch { return Redirect("/"); }
            List<news> nlist = new List<news>();
            nlist = db.news.ToList();
            return View(nlist);
        }

        // GET: User/Details/5
        [Route("Details/{id}")]
        public ActionResult Details(int id)
        {
            try { auth(); } catch { return Redirect("/"); }
            news news = new news();
            news = db.news.Where(x => x.idNews == id).FirstOrDefault();
            return View(news);
        }
        // GET: Product/Edit/5
        [Route("Profile")]
        public ActionResult ProfileDetails()
        {
            try { auth(); } catch { return Redirect("/"); }
            user tmpuser = Session["user"] as user;
            user user = db.user.Where(x => x.idUsers == tmpuser.idUsers).FirstOrDefault();
            return View(user);
        }
        // POST: Product/Edit/5
        [Route("Profile")]
        [HttpPost]
        public ActionResult ProfileDetails( user user)
        {
            try { auth(); } catch { return Redirect("/"); }


            return RedirectToAction("Index");
        }

        private void auth()
        {
            if (Session["user"] != null)
            {
                user userDetail = Session["user"] as user;
                string remember = Session["remember"] as string;
                if (userDetail.Status != 1)
                {
                    TempData["AlertMessage"] = "Your ip and session has been logged.";
                }
            }
            else
            { 
                TempData["AlertMessage"] = "You are not logged in";
                throw new InvalidStateException();
            }
        }
        public class InvalidStateException : Exception { }
    }
}
