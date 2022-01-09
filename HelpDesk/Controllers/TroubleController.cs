using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HelpDesk.Areas.Manager.Models.DAO;
using HelpDesk.Areas.Manager.Models.TroubleModel;
using HelpDesk.Areas.Tech.Models.DAO;
using HelpDesk.Common;
using HelpDesk.Hubs;
using HelpDesk.Models.UserModel;
using Model;
using Model.DAO;
using Model.ViewModel;
using PagedList;


namespace HelpDesk.Controllers
{
    //public class TroubleController : Controller
    public class TroubleController : BaseController
    {
       
        // GET: Trouble
        public ActionResult Index(int? index, string search = null)
        {
            TroubleDAO troubleDAO = new TroubleDAO();
            //Chuyển đổi trạng thái từ số -> chữ
            // ViewData["status-0"] = "Sent"; ViewData["status-1"] = "Received"; ViewData["status-2"] = "Processing"; ViewData["status-3"] = "Finished"; //ViewData["status-4"] = "Finished";
            ViewData["status-0"] = StaticResource.Resource.statusSend;
            ViewData["status-1"] = StaticResource.Resource.statusRecevied;
            ViewData["status-2"] = StaticResource.Resource.statusProcess;
            ViewData["status-3"] = StaticResource.Resource.statusFinish;
            ViewData["status-4"] = StaticResource.Resource.statusFinish;
            //
            ViewData["status-icon-0"] = "fas fa-paper-plane";
            ViewData["status-icon-1"] = "fas fa-envelope-open-text";
            ViewData["status-icon-2"] = "fas fa-spinner";
            ViewData["status-icon-3"] = "fas fa-check";
            ViewData["status-icon-4"] = "fas fa-check";
            //Thêm thứ tự sắp xếp, cách sắp xếp
            string sortOrder = Request["sortOrder"];
            if (sortOrder == null) sortOrder = "default_staff";
            //Mặc định sắp xếp theo thời gian đến sớm nhất
            ViewBag.ReportDate_SortParm = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";

            //Sắp xếp theo ID
            ViewBag.ID_SortParm = sortOrder == "id" ? "id_desc" : "id";

            //Lưu thứ tự sắp xếp hiện tại
            ViewBag.CurrentSort = sortOrder;

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
                if (!String.IsNullOrEmpty(listSearch[0]) && !String.IsNullOrEmpty(listSearch[1]))
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


            var user = (UserLogin)Session[CommonConstants.User_Session];
            ViewBag.ListTrouble = new TroubleDAO().ListByIdReport(user.UserID);
            ViewData["idUser"] = user.UserID;
            switch (user.rightt)
            {
                case 0: ViewBag.Layout = "~/Areas/Manager/Views/Shared/_LayoutTrouble.cshtml"; break;
                case 1: ViewBag.Layout = "~/Areas/Tech/Views/Shared/_LayoutTrouble.cshtml"; break;
                default: ViewBag.Layout = "~/Views/Shared/_LayoutTrouble.cshtml"; break;
            }
            
            //return View();
            List<Trouble> troubles = troubleDAO.ListByIdReport(user.UserID);
            setRowWarning(troubles);
            foreach(var item in troubles)
            {
                ViewData["date-staff-"+item.id_Trouble] = String.Format("{0:dd-MM-yyyy}", item.dateStaff);
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
            foreach (var item in trouble)
            {
                if (item.status == 0)
                    ViewData[item.id_Trouble.ToString()] = "bg-info";
                else if (item.status == 2)
                    ViewData[item.id_Trouble.ToString()] = "bg-warning";
                else if (item.status == 3)
                    ViewData[item.id_Trouble.ToString()] = "bg-success";
            }
        }

        public ActionResult Insert()
        {
            SetLayoutTrouble();
            return View(new Trouble());
        }

        [HttpPost]
        public ActionResult Insert(Trouble trouble)
        {
            SetLayoutTrouble();
            if (ModelState.IsValid)
            {
                
                var session = (UserLogin)Session[Common.CommonConstants.User_Session];
                trouble.id_Fill = session.UserID;
                var deviceDAO = new DeviceDAO();
                var userDAO = new UserDAO();
                if(userDAO.getUserById(trouble.id_Report) == null)
                {
                    ModelState.AddModelError("", "ID Report is not valid");
                }
                else if(deviceDAO.getDeviceByID(trouble.id_Device) == null)
                {
                    ModelState.AddModelError("", "ID Device is not valid");
                }
                else
                {
                    var res = new TroubleDAO().insertStaff(trouble);
                    if (res)
                    {
                        TroubleHub.SentTrouble();
                        ViewBag.Success = "Insert Success";
                        ModelState.Clear();
                        SetLayoutTrouble();
                        return View();
                    }
                    else
                    {
                        ModelState.AddModelError("", "Insert Fail");
                    }
                }
            }
            return View(trouble);
        }

        public ActionResult Detail(int? id)
        {
            if (id != null)
            {
                var dao = new TroubleFAQ_DAO(); 
                var model = dao.GetTrouble((int)id);
                
                var session = (UserLogin)Session[CommonConstants.User_Session];
                switch (session.rightt)
                {
                    case 0: ViewBag.Layout = "~/Areas/Manager/Views/Shared/_LayoutTrouble.cshtml"; break;
                    case 1: ViewBag.Layout = "~/Areas/Tech/Views/Shared/_LayoutTrouble.cshtml"; break;
                    default: ViewBag.Layout = "~/Views/Shared/_LayoutTrouble.cshtml"; break;
                }
                SetViewData(model);
                return View(model);
            }
            else
            {
                return RedirectToAction("Error", "Error");
            }
        }

        void SetViewData(TroubleFAQ_MV model)
        {
            UserDAO user = new UserDAO();

            if (user.getUserById(model.id_Report) != null)
                ViewData["Report"] = user.getUserById(model.id_Report).name;
            else ViewData["Report"] = "none";

            if (user.getUserById(model.id_Fill) != null)
                ViewData["Fill"] = user.getUserById(model.id_Fill).name;
            else ViewData["Fill"] = "none";

            if (user.getUserById(model.id_Manage) != null)
                ViewData["Manage"] = user.getUserById(model.id_Manage).name;
            else ViewData["Manage"] = "none";

            if (user.getUserById(model.id_Tech) != null)
                ViewData["Tech"] = user.getUserById(model.id_Tech).name;
            else ViewData["Tech"] = "Tenchnician";

            if (model.images == null) model.images = "";
        }

        public ActionResult Details(int? id)
        {
            if (id != null)
            {
                TroubleShareDAO dao = new TroubleShareDAO();
                var model = dao.ConvertTrouble((int)id);

                SetLayoutTrouble();
                SetViewData(model);

                return View(model);
            }
            else return RedirectToAction("Index", "Er");
            
        }

        void SetViewData(TroubleShareModelView model)
        {
            UserDAO user = new UserDAO();

            if (user.getUserById(model.id_Report) != null)
                ViewData["Report"] = user.getUserById(model.id_Report).name;
            else ViewData["Report"] = "none";

            if (user.getUserById(model.id_Fill) != null)
                ViewData["Fill"] = user.getUserById(model.id_Fill).name;
            else ViewData["Fill"] = "none";

            if (user.getUserById(model.id_Manage) != null)
                ViewData["Manage"] = user.getUserById(model.id_Manage).name;
            else ViewData["Manage"] = "none";

            var listStatus = new List<string> {
                    "Sent",
                    "Troubleshooting",
                    "Send Back",
                    "Finished"
                };
            ViewData["StatusTech"] = listStatus;
        }

        public ActionResult Edit(int id)
        {
            
            var trouble = new TroubleDAO().GetTrouble(id);
            SetLayoutTrouble();
            return View(trouble);
        }

        [HttpPost]
        public ActionResult Edit(Trouble trouble)
        {
            SetLayoutTrouble();
            if (ModelState.IsValid)
            {
                var session = (UserLogin)Session[Common.CommonConstants.User_Session];
                trouble.id_Fill = session.UserID;
                var deviceDAO = new DeviceDAO();
                var userDAO = new UserDAO();
                if (userDAO.getUserById(trouble.id_Report) == null)
                {
                    ModelState.AddModelError("", "ID Report is not valid");
                }
                else if (deviceDAO.getDeviceByID(trouble.id_Device) == null)
                {
                    ModelState.AddModelError("", "ID Device is not valid");
                }
                else
                {
                    var res = new TroubleDAO().EditTrouble(trouble);
                    if (res)
                    {
                        TroubleHub.SentTrouble();
                        ViewBag.Success = "Update Success";
                        ModelState.Clear();
                        return View(trouble);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Update Fail");
                    }
                }
            }
            return View();
        }

        public ActionResult DetailHis(int id)
        {
            SetLayoutTrouble();
            var dao = new TroubleDetailDAO();
            var model = dao.ConvertTrouble(id);
            SetViewData(model);

            return View(model);
        }

        void SetViewData(TroubleDetailViewModel model)
        {
            UserDAO user = new UserDAO();

            if (user.getUserById(model.ReportID) != null)
                ViewData["Report"] = user.getUserById(model.ReportID).name;
            else ViewData["Report"] = "none";

            if (user.getUserById(model.FillID) != null)
                ViewData["Fill"] = user.getUserById(model.FillID).name;
            else ViewData["Fill"] = "none";

            if (user.getUserById(model.ManagerID) != null)
                ViewData["Manage"] = user.getUserById(model.ManagerID).name;
            else ViewData["Manage"] = "none";
        }

        public ActionResult Delete(int id)
        {
            new TroubleDAO().DeleteTrouble(id);
            return RedirectToAction("ViewTrouble");
        }

        [HttpDelete]
        public ActionResult Delete(int? id)
        {
            if (id != null)
            {
                var dao = new TroubleDAO().Delete((int)id);
                if (dao) TroubleHub.SentTrouble();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult DeleteP(int? id)
        {
            try
            {
                if (id != null)
                {
                    var dao = new TroubleDAO().Delete((int)id);
                    if (dao)
                    {
                        TroubleHub.SentTrouble();
                        return new JsonResult { Data = true, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                    }
                }

                return new JsonResult { Data = false, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch(Exception)
            {
                return new JsonResult { Data = false, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            
        }

        [HttpPost]
        public JsonResult FinishViewed(int? id)
        {
            try
            {
                if (id != null)
                {
                    var dao = new TroubleDAO().FinishTrobuleViewed((int)id);
                    if (dao)
                    {
                        return new JsonResult { Data = true, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                    }
                }

                return new JsonResult { Data = false, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception)
            {
                return new JsonResult { Data = false, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }
        
        public JsonResult GetTrouble(int id)
        {
            TroubleDAO dao = new TroubleDAO();
            var trouble = dao.GetTrouble(id);
            if(trouble!=null)
                return new JsonResult { Data = trouble, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            return new JsonResult { Data = false, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult DetailFAQ(int? id)
        {
            //idDetailTech
            if (id != null)
            {
                SetLayoutTrouble();
                var deTeMV_DAO = new DetailTechMV_DAO();
                var model = deTeMV_DAO.GetDetailTechMV((int)id);

                UserDAO user = new UserDAO();

                if (user.getUserById(model.id_Report) != null)
                    ViewData["Report"] = user.getUserById(model.id_Report).name;
                else ViewData["Report"] = "none";

                if (user.getUserById(model.id_Fill) != null)
                    ViewData["Fill"] = user.getUserById(model.id_Fill).name;
                else ViewData["Fill"] = "none";

                if (user.getUserById(model.id_Manage) != null)
                    ViewData["Manage"] = user.getUserById(model.id_Manage).name;
                else ViewData["Manage"] = "none";

                if (user.getUserById(model.id_Tech) != null)
                    ViewData["Tech"] = user.getUserById(model.id_Tech).name;
                else ViewData["Tech"] = "Tenchnician";

                if (model.images == null) model.images = "";

                //Add thêm getter vào DeviceDAO
                DeviceDAO device = new DeviceDAO();
                //Thêm tên thiết bị
                ViewData["Device" + model.id_Device.ToString()] =
                (device.getDeviceByID(model.id_Device) == null) ?
                "none" : device.getDeviceByID(model.id_Device).nameDevice;

                
                //set button disabled
                if (model.statusTech >= 1)
                    ViewData["process"] = "disabled";

                ViewData["finish"] = "disabled";
                if (model.statusTech == 1)
                    ViewData["finish"] = "";

                //ViewData["SendBack"] = "disabled";
                if (model.statusTech >= 2)
                    ViewData["SendBack"] = "disabled";

                return View(model);
            }
            else return View();
        }

        void SetLayoutTrouble()
        {
            var user = (UserLogin)Session[CommonConstants.User_Session];
            switch (user.rightt)
            {
                case 0: ViewBag.Layout = "~/Areas/Manager/Views/Shared/_LayoutTrouble.cshtml"; break;
                case 1: ViewBag.Layout = "~/Areas/Tech/Views/Shared/_LayoutTrouble.cshtml"; break;
                default: ViewBag.Layout = "~/Views/Shared/_LayoutTrouble.cshtml"; break;
            }
        }

        
    }
}
