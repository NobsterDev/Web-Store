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
    [RoutePrefix("Admin")]
    public class AdminController : Controller
    {
        private webshopEntities db = new webshopEntities();
        // GET: Admin
        [Route]
        public ActionResult Index()
        {
            try { auth(); } catch { return Redirect("/"); }
            List<news> nlist = new List<news>();
            nlist = db.news.ToList();
            return View(nlist);
        }

        // GET: Admin/Details/5
        [Route("Details/{id}")]
        public ActionResult Details(int id)
        {
            try { auth(); } catch { return Redirect("/"); }
            news news = new news();
            news = db.news.Where(x => x.idNews == id).FirstOrDefault();
            return View(news);
        }

        // GET: Admin/Create
        [Route("Create")]
        public ActionResult Create()
        {
            try { auth(); } catch { return Redirect("/"); }
            return View(new news());
        }

        // POST: Admin/Create
        [Route("Create")]
        [HttpPost]
        public ActionResult Create(news news)
        {
            try { auth(); } catch { return Redirect("/"); }
            try
            {
                // TODO: Add insert logic here
                db.news.Add(news);
                db.SaveChanges();
                TempData["AlertMessage"] = "Success";
                return RedirectToAction("Index");
            }
            catch
            {
                TempData["AlertMessage"] = "Failed";
                return View();
            }
        }

        // GET: Admin/Edit/5
        [Route("Edit/{id}")]
        public ActionResult Edit(int id)
        {
            try { auth(); } catch { return Redirect("/"); }
            news news = new news();
            news = db.news.Where(x => x.idNews == id).FirstOrDefault();
            return View(news);
        }

        // POST: Admin/Edit/5
        [Route("Edit/{id}")]
        [HttpPost]
        public ActionResult Edit(int id, news news)
        {
            try { auth(); } catch { return Redirect("/"); }
            try
            {
                // TODO: Add update logic here
                news oldnews = db.news.Where(x => x.idNews == id).FirstOrDefault();
                oldnews.NewsHeader = news.NewsHeader;
                oldnews.NewsDesc = news.NewsDesc;
                db.Entry(oldnews).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                TempData["AlertMessage"] = "Success";
                return RedirectToAction("Index");
            }
            catch
            {
                TempData["AlertMessage"] = "Failed";
                return RedirectToAction("Index");
            }
        }

        // GET: Admin/Delete/5
        [Route("Delete")]
        public ActionResult Delete(int id)
        {
            try { auth(); } catch { return Redirect("/"); }
            try
            {
                news news = db.news.Where(x => x.idNews == id).FirstOrDefault();
                db.news.Remove(news);
                db.SaveChanges();
                TempData["AlertMessage"] = "Success";
                return RedirectToAction("Index");
            }
            catch
            {
                TempData["AlertMessage"] = "Failed";
                return RedirectToAction("Index");
            }
        }
        private void auth()
        {
            if (Session["user"] != null)
            {
                user userDetail = Session["user"] as user;
                string remember = Session["remember"] as string;
                if (userDetail.Status != 999)
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
