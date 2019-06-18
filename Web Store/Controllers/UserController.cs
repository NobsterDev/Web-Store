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
        public ActionResult ProfileDetails(user user)
        {
            try { auth(); } catch { return Redirect("/"); }
            try
            {
                db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                TempData["AlertMessage"] = "Changes Saved, Wait for approval";
                return Redirect("/");
            }
            catch (Exception)
            {
                TempData["AlertMessage"] = "Failed";
                return RedirectToAction("Profile");
            }
        }

        // GET: Product/Edit/5
        [Route("Cart")]
        public ActionResult Cart()
        {
            try { auth(); } catch { return Redirect("/"); }
            user tmpuser = Session["user"] as user;  
            List<cart> cl = db.cart.Where(x => x.User_idUsers == tmpuser.idUsers).ToList();
            List<product> pl = db.product.ToList();
            List<product> result = new List<product>();
            List<string> result1 = new List<string>();
            foreach (var c in cl)
            {
                foreach (var p in pl)
                {
                    if (c.Product_idItems == p.idItems)
                    {
                        result.Add(p);
                        result1.Add(c.Quantity.ToString());
                    }
                }
            }
            TempData["qu"] = result1;
            return View(result);
        }
        // GET: Product/Edit/5
        [Route("Buy")]
        public ActionResult Buy(int id)
        {
            try { auth(); } catch { return Redirect("/"); }
            try
            {
                user tmpuser = Session["user"] as user;
                List<cart> cart = db.cart.Where(x=> x.User_idUsers == tmpuser.idUsers).ToList();
                foreach (var item in cart)
                {
                    string str = "Delete From `webshop`.`cart` WHERE (`Product_idItems` = '" + id + "');";
                    db.Database.ExecuteSqlCommand(str);
                }
                TempData["AlertMessage"] = "Successfully Bought. Please Check your Email For Payment And Your Full Address.";
                return RedirectToAction("Cart");
            }
            catch
            {
                TempData["AlertMessage"] = "Failed";
                return RedirectToAction("Cart");
            }
        }
        // GET: Product/Edit/5
        [Route("DeleteCart/{id}")]
        public ActionResult DeleteFromCart(int id)
        {
            try { auth(); } catch { return Redirect("/"); }
            try
            {
                user tmpuser = Session["user"] as user;
                cart cart = db.cart.Where(x => x.Product_idItems == id && x.User_idUsers == tmpuser.idUsers).First();
                string str = "UPDATE `webshop`.`cart` SET `Quantity` = '0' WHERE (`Product_idItems` = '"+id+"');";
                db.Database.ExecuteSqlCommand(str);
                str= "Delete From `webshop`.`cart` WHERE (`Product_idItems` = '" + id + "');";
                db.Database.ExecuteSqlCommand(str);
                TempData["AlertMessage"] = "Success";
                return RedirectToAction("Cart");
            }
            catch
            {
                TempData["AlertMessage"] = "Failed";
                return RedirectToAction("Cart");
            }
        }
        // GET: Product/Edit/5
        [Route("ChangeCart/{id}")]
        [HttpPost]
        public ActionResult ChangeCart(int id,string quantity)
        {
            try { auth(); } catch { return Redirect("/"); }
            try
            {
                user tmpuser = Session["user"] as user;
                string str = "UPDATE `webshop`.`cart` SET `Quantity` = '" + quantity + "' WHERE (`Product_idItems` = '" + id + "');";
                db.Database.ExecuteSqlCommand(str);
                TempData["AlertMessage"] = "Success";
                return RedirectToAction("Cart");
            }
            catch
            {
                TempData["AlertMessage"] = "Failed";
                return RedirectToAction("Cart");
            }
        }





        // GET: Product
        [Route("Products")]
        public ActionResult Products()
        {
            try { auth(); } catch { return Redirect("/"); }
            List<product> plist = new List<product>();
            user tmpuser = Session["user"] as user;
            List<cart> clist = new List<cart>();
            clist = db.cart.Where(x=> x.User_idUsers == tmpuser.idUsers).ToList();
            plist = db.product.ToList();
            List<product> tmpplist = new List<product>();
            foreach (var c in clist)
            {
                foreach (var p in plist)
                {
                    if (p.idItems == c.Product_idItems)
                    {
                        tmpplist.Add(p);
                    }      
                }
            }
            foreach (var item in tmpplist)
            {
                plist.Remove(item);
            }
            return View(plist);
        }

        // GET: addcart
        [Route("AddCart/{id}")]
        public ActionResult Addtocart(int id)
        {
            try { auth(); } catch { return Redirect("/"); }
            user user = Session["user"] as user;
            try
            {
                db.Database.ExecuteSqlCommand("INSERT INTO `webshop`.`cart` VALUES ("+id+","+user.idUsers+",0);");
                TempData["AlertMessage"] = "Success";
                return RedirectToAction("Products");
            }
            catch
            {
                TempData["AlertMessage"] = "Failed";
                return RedirectToAction("Products");
            }
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
