using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.DAO;
using Model;
using HelpDesk.Common;
using PagedList;

namespace HelpDesk.Areas.Tech.Controllers
{
    public class FAQController : BaseController
    {
        private FAQ_DAO FAQs = new FAQ_DAO(); 


        //Hàm trả về danh sách FAQ
        // GET: Tech/FAQ
        public ActionResult Index(int? index)
        {
            var user = (UserLogin)Session[CommonConstants.User_Session];
            var model = FAQs.GetLisAll();
            foreach (var item in model)
            {
                if (item.answer.Length > 50)
                {
                    item.answer = item.answer.Substring(0, 50);
                    item.answer += " . . .";
                }

            }
            ViewData["0"] = "Private";
            ViewData["1"] = "Public";
            //
            ViewData["role-icon-0"] = "fas fa-lock";
            ViewData["role-icon-1"] = "fas fa-globe-asia";
            int pageSize = 10;
            int pageIndex = (index ?? 1);
            return View(model.ToPagedList(pageIndex,pageSize));
        }

        //Xem thông tin chi tiết của FAQ
        // GET: Tech/FAQ/Details/5
        public ActionResult Details(int? id)
        {
            if (id != null)
            {
                FAQ  FAQ = FAQs.GetFAQByID((int)id);
                if(FAQ == null)
                {
                    ModelState.AddModelError("", "Không tìm thấy");
                    return View();
                }
                else
                {
                    return View(FAQ);
                }
            }

            return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
        }

        //Hàm tạo một FAQ
        // GET: Tech/FAQ/Create
        public ActionResult SuggestionFAQ()
        {
            return View();
        }

        // POST: Tech/FAQ/Create
        [HttpPost]
        public ActionResult SuggestionFAQ(FAQ faq)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var session = (UserLogin)Session[CommonConstants.User_Session];
                    faq.id_User = session.UserID;
                    //Thêm vào CSDL
                    bool check = FAQs.SuggestionFAQ(faq);
                    if(check)
                    {
                        ViewBag.Success = "Suggestion Success";
                        return View(new FAQ());
                    }
                }

                ModelState.AddModelError("", "FAQ đã tồn tại");

                return View();
            }
            catch
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
        }
    }
}
