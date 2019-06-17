using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_Store.Models;

namespace Web_Store.Controllers
{
    [RoutePrefix("Admin/Products")]
    public class ProductController : Controller
    {
        private webshopEntities db = new webshopEntities();
        // GET: Product
        [Route]
        public ActionResult Index()
        {
            try { auth(); } catch { return Redirect("/"); }
            List<product> plist = new List<product>();
            plist=db.product.ToList();
            return View(plist);
        }

        // GET: Product/Create
        [Route("Create")]
        public ActionResult Create()
        {
            try { auth(); } catch { return Redirect("/"); }
            return View(new product());
        }

        // POST: Product/Create
        [Route("Create")]
        [HttpPost]
        public ActionResult Create(product product)
        {
            try { auth(); } catch { return Redirect("/"); }
            try
            {
                // TODO: Add insert logic here
                db.product.Add(product);
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

        // GET: Product/Edit/5
        [Route("Edit/{id}")]
        public ActionResult Edit(int id)
        {
            try { auth(); } catch { return Redirect("/"); }
            product product = new product();
            product = db.product.Where(x => x.idItems == id).FirstOrDefault();
            return View(product);
        }

        // POST: Product/Edit/5
        [Route("Edit/{id}")]
        [HttpPost]
        public ActionResult Edit(int id,string ItemName,string ItemPrice)
        {
            try { auth(); } catch { return Redirect("/"); }
            try
            {
                ItemPrice = ItemPrice.Replace('.', ',');
                // TODO: Add update logic here
                product oldproduct = db.product.Where(x => x.idItems == id).FirstOrDefault();
                oldproduct.ItemName = ItemName;
                oldproduct.ItemPrice = decimal.Parse(ItemPrice);
                db.Entry(oldproduct).State = System.Data.Entity.EntityState.Modified;
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

        // GET: Product/Delete/5
        [Route("Delete/{id}")]
        public ActionResult Delete(int id)
        {
            try { auth(); } catch { return Redirect("/"); }
            product product = new product();
            product = db.product.Where(x => x.idItems == id).FirstOrDefault();
            return View(product);
        }

        // POST: Product/Delete/5
        [Route("Delete/{id}")]
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try { auth(); } catch { return Redirect("/"); }
            try
            {
                // TODO: Add update logic here
                product product = db.product.Where(x=> x.idItems == id).FirstOrDefault();
                db.product.Remove(product);
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
