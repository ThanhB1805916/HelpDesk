using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HelpDesk.Areas.Manager.Controllers
{
    public class ManagerController : BaseController
    {
        // GET: Manager/Manager
        public ActionResult Index()
        {
            return View();
        }
    }
}