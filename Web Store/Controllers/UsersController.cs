using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_Store.Models;

namespace Web_Store.Controllers
{
    [RoutePrefix("Admin/Users")]
    public class UsersController : Controller
    {

        private webshopEntities db = new webshopEntities();
        // GET: Users
        [Route]
        public ActionResult Index()
        {
            try { auth(); } catch { return Redirect("/"); }
            List<user> ulist = new List<user>();
            ulist = db.user.ToList();
            return View(ulist);
        }

        // GET: enable
        [Route("Enable")]
        public ActionResult Enable(int id)
        {
            try { auth(); } catch { return Redirect("/"); }
            try
            {
                user user = new user();
                user = db.user.Where(x => x.idUsers == id).FirstOrDefault();
                user.Status = 1;
                db.Entry(user).State = System.Data.Entity.EntityState.Modified;
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

        // GET: disable
        [Route("Disable")]
        public ActionResult Disable(int id)
        {
            try { auth(); } catch { return Redirect("/"); }
            try
            {
                user user = new user();
                user = db.user.Where(x => x.idUsers == id).FirstOrDefault();
                user.Status = 0;
                db.Entry(user).State = System.Data.Entity.EntityState.Modified;
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

        // GET: Users/Delete/5
        [Route("Delete/{id}")]
        public ActionResult Delete(int id)
        {
            try { auth(); } catch { return Redirect("/"); }
            user user = new user();
            user = db.user.Where(x => x.idUsers == id).FirstOrDefault();
            return View(user);
        }

        // POST: Users/Delete/5
        [Route("Delete/{id}")]
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try { auth(); } catch { return Redirect("/"); }
            try
            {
                user user = db.user.Where(x => x.idUsers == id).FirstOrDefault();
                db.user.Remove(user);
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
                    throw new InvalidStateException();
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
