using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace BikeDistributor.WebApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Home()
        {
            return PartialView("_Home");
        }

        public ActionResult Login()
        {
            return PartialView("_Login");
        }

        public ActionResult Register()
        {
            return PartialView("_Register");
        }

        public ActionResult Cart()
        {
            return PartialView("_Cart");
        }
    }
}
