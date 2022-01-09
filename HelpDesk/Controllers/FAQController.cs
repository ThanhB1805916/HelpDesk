using HelpDesk.Common;
using HelpDesk.Models.DAO;
using Model.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace HelpDesk.Controllers
{
    public class FAQController : BaseController
    {
        // GET: FAQ
        public ActionResult Index(int? index)
        {
            var dao = new FAQ_DAO();
            var model = dao.GetListPublic();
            foreach (var item in model)
            {
                if (item.answer.Length > 50)
                {
                    item.answer = item.answer.Substring(0, 50);
                    item.answer += " . . .";
                }

            }
            ViewData["0"] = StaticResource.Resource.pv;
            ViewData["1"] = StaticResource.Resource.pb;

            int pageSize = 10;
            int pageIndex = (index ?? 1);

            return View(model.ToPagedList(pageIndex, pageSize));
            
        }

        public  ActionResult Detail(int? id)
        {
            if (id != null)
            {
                var dao = new FAQ_MV_DAO();
                var faq = dao.GetFAQ_MV((int)id);
                var techDAO = new UserDAO();
                
                ViewData["right"] = faq.rightt == 0 ? StaticResource.Resource.pv : StaticResource.Resource.pb;
                var session = (UserLogin)Session[CommonConstants.User_Session];
                switch (session.rightt)
                {
                    case 0:
                        ViewBag.Layout = "~/Areas/Manager/Views/Shared/_LayoutTrouble.cshtml";
                        ViewBag.View = "Manager";
                        break;
                    case 1:
                        ViewBag.Layout = "~/Areas/Tech/Views/Shared/_LayoutTrouble.cshtml";
                        ViewBag.View = "Tech";
                        break;
                    default:
                        ViewBag.Layout = "~/Views/Shared/_LayoutTrouble.cshtml";
                        ViewBag.View = "";
                        break;
                }

                for(int i = 0; i < faq.finishTime.Count; i++)
                {
                    ViewData["TechName_"+i] = techDAO.getUserById(faq.id_Tech[i]).name;
                }
                
                return View(faq);
            }
            else
            {
                return RedirectToAction("Error", "Error");
            }
        }


    }
}