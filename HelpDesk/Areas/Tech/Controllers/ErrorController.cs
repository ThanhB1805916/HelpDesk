using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HelpDesk.Areas.Tech.Controllers
{
    public class ErrorController : Controller
    {
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