using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HelpDesk.Areas.Manager.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Manager/Manager
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Error()
        {
            return Redirect("~/Er/Index");
        }

        public ActionResult Login()
        {
            return Redirect("~/Authentication/Login");
        }
    }
}