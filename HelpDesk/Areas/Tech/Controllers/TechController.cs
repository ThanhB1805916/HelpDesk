using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HelpDesk.Areas.Tech.Controllers
{
    public class TechController : BaseController
    {
        // GET: Tech/Tech
        public ActionResult Index()
        {
            return View();
        }
    }
}