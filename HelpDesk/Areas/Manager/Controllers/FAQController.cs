using HelpDesk.Models.DAO;
using HelpDesk.Models.UserModel;
using Model;
using Model.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace HelpDesk.Areas.Manager.Controllers
{
    public class FAQController : BaseController
    {
        // GET: Manager/FAQ
        public ActionResult Index(int? index)
        {
            var dao = new FAQ_DAO();
            var model = dao.GetLisAll();
            foreach(var item in model)
            {
                if(item.answer.Length > 50)
                {
                    item.answer = item.answer.Substring(0, 50);
                    item.answer += " . . .";
                }
                
            }
            ViewData["0"] = StaticResource.Resource._private; 
            ViewData["1"] = StaticResource.Resource._public;

            int pageSize = 10;
            int pageIndex = (index ?? 1);

            return View(model.ToPagedList(pageIndex,pageSize));
        }

        public ActionResult SuggestionFAQ()
        {
            var dao = new FAQ_DAO();
            var model = dao.GetListSuggestion();
            foreach (var item in model)
            {
                if (item.answer.Length > 50)
                {
                    item.answer = item.answer.Substring(0, 50);
                    item.answer += " . . .";
                }

            }
            ViewData["0"] = StaticResource.Resource._private;
            ViewData["1"] = StaticResource.Resource._public;
            return View(model);
        }

        public ActionResult Edit(int? id)
        {
            if (id != null)
            {
                var dao = new FAQ_DAO();
                var faq = dao.GetFAQByID((int)id);
               

                ViewData["right"] = faq.rightt == 0 ? StaticResource.Resource._private : StaticResource.Resource._public;
                SetViewBagEdit((int)id);
                

                return View(faq);
            }
            else
            {
                return RedirectToAction("Error", "Error");
            }
        }

        void SetViewBagEdit(int idFAQ)
        {
            var techDAO = new UserDAO();
            var dao = new DetailTechDAO();
            var detail = dao.GetDetailFAQ(idFAQ);
            for (int i = 0; i < detail.Count; i++)
            {
                ViewData["TechName_" + i] = techDAO.getUserById(detail[i].id_Tech).name;
            }
            ViewBag.detail = detail;
        }

        public ActionResult Insert()
        {
            var model = new FAQ();
            return View(model);
        }

        [HttpPost]
        public ActionResult Insert(FAQ model)
        {
            if (ModelState.IsValid)
            {
                var dao = new FAQ_DAO();
                var kt = dao.Insert_FAQ(model);
                if (kt)
                {
                    ViewBag.sucess = StaticResource.Resource.success;
                    ModelState.Clear();
                    return View("Insert", new FAQ());
                }
            }
            ModelState.AddModelError("",StaticResource.Resource.fail);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(FAQ model)
        {
            if (ModelState.IsValid)
            {
                var daoMV = new FAQ_MV_DAO();
                var dao = new FAQ_DAO();
                var kt = dao.UpdateFAQ(model);
                if (kt)
                {
                    ViewBag.Sucess = StaticResource.Resource.success;
                    SetViewBagEdit(model.id_FAQ);
                    return View(model);
                }
            }
            ModelState.AddModelError("", StaticResource.Resource.fail);
            return View("Edit", model);
        }

        [HttpPost]
        public ActionResult ChangeRight(int id)
        {
            FAQ_DAO dao = new FAQ_DAO();
            ViewData["0"] = StaticResource.Resource._private;
            ViewData["1"] = StaticResource.Resource._public;
            dao.ChangeRight(id);
            return RedirectToAction("Index");
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            FAQ_DAO dao = new FAQ_DAO();
            dao.Delete(id);
            return RedirectToAction("Index");
        }

        public ActionResult Remove(int id)
        {
            DetailTechDAO dao = new DetailTechDAO();
            if(dao.RemoveFAQ(id))
                ViewBag.sucess = StaticResource.Resource.removeSuccess;
            else ViewBag.sucess = StaticResource.Resource.updateFail;
            return RedirectToAction("Edit");
        }

        public ActionResult DetailSuggestion(int? id)
        {
            if(id != null)
            {
                FAQ_DAO dao = new FAQ_DAO();
                var faq = dao.GetFAQByID((int)id);
                ViewBag.NameSug = new UserDAO().getUserById(faq.id_User).name;
                return View(faq);
            }
            else
            return RedirectToAction("SuggestionFAQ");
        }

        [HttpPost]
        public ActionResult DetailSuggestion(FAQ faq)
        {
            var kt = new FAQ_DAO().AcceptFAQ(faq);
            ViewBag.NameSug = new UserDAO().getUserById(faq.id_User).name;
            if (kt)
            {
                ViewBag.Success = StaticResource.Resource.success;
                return View(faq);
            }
            ViewBag.Fail = StaticResource.Resource.fail;
            return View(faq);
        }

        public JsonResult RejectFAQ(int id)
        {
            var kt = new FAQ_DAO().RejectFAQ(id);
            return new JsonResult { Data = kt, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}

