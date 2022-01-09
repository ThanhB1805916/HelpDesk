using HelpDesk.Areas.Manager.Models.TroubleModel;
using HelpDesk.Common;
using HelpDesk.Models;
using Model;
using Model.DAO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using HelpDesk.Areas.Manager.Models.DAO;
using HelpDesk.Areas.Manager.Models;

namespace HelpDesk.Areas.Manager.Controllers
{
    public class TroubleController : BaseController
    {
        // GET: Tech/Trouble

        //Tham số lấy trực tiếp từ Request
        //index là vị trí của trang.
        public ActionResult Index(int? index, string search = null)
        {
            
            TroubleDAO troubleDAO = new TroubleDAO();
            DetailTechDAO deteDAQ = new DetailTechDAO();
            
            //Chuyển đổi trạng thái từ số -> chữ
            ViewData["status-0"] = StaticResource.Resource.statusManagerSend; 
            ViewData["status-1"] = StaticResource.Resource.statusManagerReceived;
            ViewData["status-2"] = StaticResource.Resource.statusManagerProcess; 
            ViewData["status-3"] = StaticResource.Resource.statusManagerFinish; 
            ViewData["status-4"] = StaticResource.Resource.statusManagerFinish;


            //Chuyển đổi level từ số -> chữ
            ViewData["level-0"] = StaticResource.Resource.levelLess; 
            ViewData["level-1"] = StaticResource.Resource.levelSerious;
            ViewData["level-2"] = StaticResource.Resource.levelSerious; 
            ViewData["level--1"] = StaticResource.Resource.levelNo;

            //
            ViewData["status-icon-0"] = "fas fa-envelope";
            ViewData["status-icon-1"] = "fas fa-clipboard-list";
            ViewData["status-icon-2"] = "fas fa-spinner";
            ViewData["status-icon-3"] = "fas fa-check";
            ViewData["status-icon-4"] = "fas fa-check";

            //Thêm thứ tự sắp xếp, cách sắp xếp
            string sortOrder = Request["sortOrder"];
            //Mặc định sắp xếp theo thời gian đến sớm nhất
            ViewBag.ReportDate_SortParm = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            //Sắp xếp theo level
            ViewBag.Level_SortParm = sortOrder == "level" ? "level_desc" : "level";

            //Sắp xếp theo ID
            ViewBag.ID_SortParm = sortOrder == "id" ? "id_desc" : "id";

            //Lưu thứ tự sắp xếp hiện tại
            ViewBag.CurrentSort = sortOrder;

            //Tạo chuỗi tìm kiếm hiện tại định dạng là: dd-mm-yyyy_level (không lấy đối số năm)

            //string searchString = Request["searchString_Date"] + '-' + Request["searchString_Month"] + '-'
            //                    + '_' + Request["searchString_Level"];
            string searchString = "";
            ViewData["selected-"] = false;
            ViewData["selected--1"] = false;
            ViewData["selected-0"] = false;
            ViewData["selected-1"] = false;
            ViewData["selected-2"] = false;
            if (search != null)
            {
                var listSearch = search.Split('_');
                searchString = search;
                if (!String.IsNullOrEmpty(listSearch[0])&& !String.IsNullOrEmpty(listSearch[1]))
                {
                    ViewBag.dateStart = String.Format("{0:yyyy-MM-dd}", DateTime.Parse(listSearch[0]));
                    ViewBag.dateEnd = String.Format("{0:yyyy-MM-dd}", DateTime.Parse(listSearch[1]));
                }
                if (listSearch[2] != null)
                {
                    var level = "selected-" + listSearch[2];
                    ViewData[level] = true;
                }
                    
            }
                
            
            //Tạo danh sách lưu các troubles
            List<Trouble> troubles = troubleDAO.ListAll();
            setRowWarning(troubles);
            
            foreach(var item in troubles)
            {
                if(deteDAQ.CountTask(item.id_Trouble) != 0)
                    ViewData["stage"+item.id_Trouble] = "(" + deteDAQ.CountTaskFinish(item.id_Trouble) + "/" + deteDAQ.CountTask(item.id_Trouble) + ")";
            }
            
            //Sắp xếp danh sách theo chuỗi sortOrder
            troubles = troubleDAO.Ordered_Troubles(troubles, sortOrder);

            //Tìm kiếm theo chuỗi searchString
            troubles = troubleDAO.Search_Troubles(troubles, searchString);

            //Page size số lượng hàng(trouble) trong 1 trang
            int pageSize = 10;
            //Page index vị trí của trang mặc định là 1
            int pageIndex = (index ?? 1);
            
            return View(troubles.ToPagedList(pageIndex, pageSize));
        }

        void setRowWarning(List<Trouble> trouble)
        {
            foreach(var item in trouble)
            {
                if (item.status == 0)
                    ViewData[item.id_Trouble.ToString()] = "bg-info";
                else if (item.status == 2)
                    ViewData[item.id_Trouble.ToString()] = "bg-warning";
            }
        }

        public ActionResult Share(int? id)
        {
            if (id != null)
            {
                TroubleShareDAO dao = new TroubleShareDAO();
                var model = dao.ConvertTrouble((int)id);

                SetViewData(model);


                if (model.dateTech != null)
                {
                    ViewBag.done = "disabled";
                }

                return View(model);
            }
            else return RedirectToAction("Error", "Error", "Manager");

        }

        public void SetViewBag(int? id = null)
        {
            UserDAO dao = new UserDAO();
            ViewBag.id_Tech = new SelectList(dao.ListUser(1), "id_user", "name", id);
        }

        [HttpPost]
        public ActionResult Share(TroubleShareModelView model)
        {
            TroubleShareDAO dao = new TroubleShareDAO();
            //if (ModelState.IsValid)
            //{
            var user = (UserLogin)Session[CommonConstants.User_Session];
            model.id_Manage = user.UserID;

            int kt = dao.InsertManage(model);
            if (kt >= 1)
            {
                model = dao.ConvertTrouble(model.id_Trouble);
                SetViewData(model);
                ViewBag.Success = StaticResource.Resource.insertSuccess;
                return View("Share", model);
            }
            else
            {
                switch (kt)
                {
                    case -2: ModelState.AddModelError("", "Insert FAQ Fail"); break;
                    case -3: ModelState.AddModelError("", "Id FAQ Not Found"); break;
                    case -4: ModelState.AddModelError("", "Insert Technician Fail"); break;
                    case -5: ModelState.AddModelError("", "Not Technician Id"); break;
                    case -6: ModelState.AddModelError("", "Id Technician Not Found"); break;
                    case -7: ModelState.AddModelError("", StaticResource.Resource.deadlineRequire); break;
                    default: ModelState.AddModelError("", StaticResource.Resource.insertFail); break;
                }
                ModelState.AddModelError("", StaticResource.Resource.insertFail);
                //for (int i = model.dateFinish.Count; i < model.id_Tech.Count; i++)
                //{
                //    model.id_Tech.RemoveAt(i);
                //    model.describeTech.RemoveAt(i);
                //}
            }

            SetViewData(model);
            return View("Share", model);
            
        }

        void SetViewData(TroubleShareModelView model)
        {
            UserDAO user = new UserDAO();

            if (user.getUserById(model.id_Report) != null)
                ViewData["Report"] = user.getUserById(model.id_Report).name;
            else ViewData["Report"] = StaticResource.Resource.none;

            if (user.getUserById(model.id_Fill) != null)
                ViewData["Fill"] = user.getUserById(model.id_Fill).name;
            else ViewData["Fill"] = StaticResource.Resource.none;

            if (user.getUserById(model.id_Manage) != null)
                ViewData["Manage"] = user.getUserById(model.id_Manage).name;
            else ViewData["Manage"] = StaticResource.Resource.none;

            var listStatus = new List<string> {
                    StaticResource.Resource.statusSend,
                    StaticResource.Resource.statusTroubleshooting,
                    StaticResource.Resource.statusSendBack,
                    StaticResource.Resource.statusFinish
                };
            ViewData["StatusTech"] = listStatus;
            for(int i = 0; i < model.id_Tech.Count; i++)
            {
                ViewData["tech_" + model.id_Tech[i]] = user.getUserById(model.id_Tech[i]).name;
                ViewData["faq_" + model.id_FAQ[i]] = new FAQ_DAO().GetFAQByID(model.id_FAQ[i]);
            }

            var listFAQ = new FAQ_DAO().GetLisAll();
            listFAQ.Insert(0, new FAQ(0, "none"));
            
            for(int i = 0; i < model.id_FAQ.Count; i++)
            {
                ViewData["faq_" + i] = new SelectList(listFAQ, "id_FAQ", "question", model.id_FAQ[i]);
            }
            
        }

        public JsonResult Get()
        {
            TroubleDAO dao = new TroubleDAO();
            List<Trouble> list = dao.ListByStatus(0);
            list = list.OrderByDescending(x => x.dateStaff).ToList();
            return new JsonResult { Data = list, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        public JsonResult GetNewTrouble()
        {
            TroubleDAO dao = new TroubleDAO();
            var trouble = dao.GetLastTrouble();
            var count = dao.ListByStatus(0).Count;
            return new JsonResult { Data = new { trouble = trouble, count = count }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        //public ActionResult GetMessages()
        //{
        //    TroubleRes _messageRepository = new TroubleRes();
        //    return PartialView("_MessagesList", _messageRepository.GetTrouble());
        //}

        // Xem báo cáo sự cố
        public ActionResult Troubles_Statistic(string searchString_Date, string searchString_Month, string searchString_Year, string searchString_Level, string sortOrder, string currentFilter, int? index)
        {
            TroubleDAO troubleDAO = new TroubleDAO();
            //Thêm thứ tự sắp xếp, cách săp xếp
            //Mặc định sắp xếp theo thời gian đến sớm nhất
            ViewBag.ReportDate_SortParm = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            //Sắp xếp theo level
            ViewBag.Level_SortParm = sortOrder == "level" ? "level_desc" : "level";

            //Lưu thứ tự sắp xếp hiện tại
            ViewBag.CurrentSort = sortOrder;

            //Tạo chuỗi tìm kiến hiện tại định dạng dd-mm-yyyy_level
            string searchString = searchString_Date + '-' + searchString_Month + '-' + searchString_Year
                                + '_' + searchString_Level;

            //Kiểm tra nếu có chuỗi tìm kiếm
            if (searchString != "--_")
            {
                index = 1;
            }
            else
            {
                //Gán chuỗi tìm kiếm là chuỗi tìm kiếm hiện tại lấy từ View
                searchString = currentFilter;
            }

            //Lưu lại chuỗi tìm kiếm hiện tại truyền lên cho view
            ViewBag.CurrentFilter = searchString;


            List<Trouble> troubles = troubleDAO.ListAll();

            troubles = troubleDAO.Ordered_Troubles(troubles, sortOrder);
            troubles = troubleDAO.Search_Troubles(troubles, searchString);


            //Thống kê
            //Đếm số trouble được lọc theo ngày

            //Troubles_Statistic lưu trong HelpDesk.Areas.Manager.Models
            Troubles_Statistic troubles_Statistic = new Troubles_Statistic()
            {
                Troubles_Card = troubles.Count(),
                Troubles_Sent = troubles.Where(x => x.status == 0).Count(),
                Troubles_Received = troubles.Where(x => x.status == 1).Count(),
                Troubles_Processing = troubles.Where(x => x.status == 2).Count(),
                Troubles_Finished = troubles.Where(x => x.status == 3).Count()
            };

            ////Page size số lượng hàng(trouble) trong 1 trang
            //int pageSize = 3;
            ////Page index vị trí của trang mặc định là 1
            //int pageIndex = (index ?? 1);

            //return View(troubles_Statistic.ToPagedList(pageIndex, pageSize));
            return View(troubles_Statistic);
        }

        public ActionResult NewTrouble()
        {
            return View();
        }

        [HttpDelete]
        public ActionResult DeleteTech(int id)
        {
            var deteDAO = new DetailTechDAO();
            deteDAO.Delete(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ReSend(int id)
        {
            var deteDAO = new DetailTechDAO();
            deteDAO.ReSend(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public JsonResult GetListTech()
        {
            UserDAO dao = new UserDAO();
            var listTech = dao.ListByRight(1);
            return new JsonResult { Data = listTech, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        [HttpGet]
        public JsonResult GetListFAQ()
        {
            FAQ_DAO dao = new FAQ_DAO();
            var listFAQ = dao.GetLisAll();
            return new JsonResult { Data = listFAQ, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }
    }
}