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
            if(Session["user"]!=null && Session["remember"]!=null)
            {
                user userDetail = Session["user"] as user;
                string remember = Session["remember"] as string;
                if(remember=="true")
                {
                    if (userDetail.Status == 1)
                    {
                        Session["User"] = userDetail;
                        return Redirect("/User");
                    }
                    else if (userDetail.Status == 999)
                    {
                        Session["User"] = userDetail;
                        return Redirect("/Admin");
                    }
                }
            }
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

        [Route("Logout")]
        public ActionResult Logout()
        {
            Session["User"]= null;
            TempData["AlertMessage"] = "Logged Out";
            return RedirectToAction("Login");
        }

        [Route("Register")]
        [HttpPost]
        public ActionResult Register(user user)
        {
            try
            {
                // TODO: Add insert logic here
                user userDetail = db.user.Where(x => x.Mail == user.Mail).FirstOrDefault();
                if (userDetail != null)
                {
                    TempData["AlertMessage"] = "User Already Exists";
                }
                else
                {
                    db.user.Add(user);
                    db.SaveChanges();
                    TempData["AlertMessage"] = "Successfully Registered";
                }
                return RedirectToAction("Index");
            }
            catch
            {
                TempData["AlertMessage"] = "Failed";
                return RedirectToAction("Index");
            }
        }


        [Route("Login")]
        [HttpPost]
        public ActionResult Login(user user, string password,string remember)
        {
            try
                {
                user userDetail = db.user.Where(x => x.Mail == user.Mail && x.Password == user.Password).FirstOrDefault();
                // TODO: Add insert logic here
                if (userDetail!= null)
                {
                    userDetail.Password = "Your ip and session is recorded.";
                }
                if (userDetail == null)
                {
                    TempData["AlertMessage"] = "User not found";
                    return RedirectToAction("Index");
                }
                if (userDetail.Status ==1)
                {
                    count count = db.count.Where(x => x.iduser == userDetail.idUsers).FirstOrDefault();
                    if (count == null)
                    {
                        count newcount = new count();
                        string str = "INSERT INTO `webshop`.`count` (`iduser`, `count`) VALUES ('" + userDetail.idUsers + "', '1');";
                        db.Database.ExecuteSqlCommand(str);
                    }
                    else
                    {
                        count.count1++;
                        string str = "UPDATE `webshop`.`count` SET `count` = '"+count.count1+ "' WHERE (`iduser` = '" + userDetail.idUsers + "');";
                        db.Database.ExecuteSqlCommand(str);
                    }
                    int users = db.user.ToList().Count-1;
                    int visitedbyme = db.count.Where(x => x.iduser == userDetail.idUsers).First().count1;
                    int visit = 0;
                    List<count> lvisit = db.count.ToList();
                    foreach (var item in lvisit)
                    {
                        visit += item.count1;
                    }
                    DateTime now = DateTime.Now;

                    HttpCookie ck = new HttpCookie("counter");
                    ck.Expires.AddYears(30);
                    ck.Values.Add("visit", visit.ToString());
                    ck.Values.Add("visitedbyme", visitedbyme.ToString());
                    ck.Values.Add("users", users.ToString());
                    Response.Cookies.Add(ck);
                    TempData["AlertMessage"] = "Welcome "+user.Name;
                    Session["User"] = userDetail;
                    Remember(remember);
                    return Redirect("/User");
                }
                else if (userDetail.Status == 999)
                {
                    Session["User"] = userDetail;
                    Remember(remember);
                    return Redirect("/Admin");
                }
                else
                {
                    TempData["AlertMessage"] = "The User Is Disabled. Wait until Admins Enable Your Account.";
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                return View();
            }
        }
        private void Remember(string remember)
        {
            if(remember == "on")
            {
                Session["remember"] = "true";
            }
            else
            {
                Session["remember"] = "false";
            }
        }
    }
}