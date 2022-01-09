using HelpDesk.Common;
using Model.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using PagedList;
using PagedList.Mvc;
using HelpDesk.Areas.Tech.Models;
using HelpDesk.Areas.Tech.Models.DAO;
using HelpDesk.Hubs;

namespace HelpDesk.Areas.Tech.Controllers
{
    public class TroubleController : BaseController
    {
       
        // GET: Tech/Trouble

        //Tham số lấy trực tiếp từ Request
        //index là vị trí của trang.
        public ActionResult Index(int? index, string search = null)
        {
            TroubleDAO troubleDAO = new TroubleDAO();
            //Chuyển status sang chữ
            ViewData["status-0"] = StaticResource.Resource.statusManagerSend;
            ViewData["status-1"] = StaticResource.Resource.statusManagerReceived;
            ViewData["status-2"] = StaticResource.Resource.statusManagerProcess;
            ViewData["status-3"] = StaticResource.Resource.statusManagerFinish;
            ViewData["status-4"] = StaticResource.Resource.statusManagerFinish;
            //Chuyển đổi level từ số -> chữ
            ViewData["level-0"] = StaticResource.Resource.levelLess;
            ViewData["level-1"] = StaticResource.Resource.levelSerious;
            ViewData["level-2"] = StaticResource.Resource.levelVery;
            ViewData["level--1"] = StaticResource.Resource.levelNo;
            //
            ViewData["status-icon-0"] = "fas fa-envelope";
            ViewData["status-icon-1"] = "fas fa-clipboard-list";
            ViewData["status-icon-2"] = "fas fa-spinner";
            ViewData["status-icon-3"] = "fas fa-check";
            ViewData["status-icon-4"] = "fas fa-check";

            //Lấy ra người dùng đang đăng nhập
            UserLogin userLogin = (UserLogin)Session["User_Session"];

            //Lọc danh sách sự cố của người dùng đăng nhập
            var deteDAO = new DetailTechMV_DAO();
            var troubles = deteDAO.ListDeTeMV(userLogin.UserID);
   
            //Thêm thứ tự sắp xếp, cách sắp xếp
            string sortOrder = Request["sortOrder"];
            //Mặc định sắp xếp theo thời gian đến sớm nhất
            ViewBag.ReportDate_SortParm = sortOrder == "dateReport" ? "dateReport_Desc" : "dateReport";
            //Sắp xếp theo level
            ViewBag.Level_SortParm = sortOrder == "level" ? "level_Desc" : "level";

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
                if (!String.IsNullOrEmpty(listSearch[0]))
                    ViewBag.dateStart = String.Format("{0:yyyy-MM-dd}", DateTime.Parse(listSearch[0]));
                if (!String.IsNullOrEmpty(listSearch[1]))
                    ViewBag.dateEnd = String.Format("{0:yyyy-MM-dd}", DateTime.Parse(listSearch[1]));
                if (listSearch[2] != null)
                {
                    var level = "selected-" + listSearch[2];
                    ViewData[level] = true;
                }

            }


            ////Kiểm tra nếu có giá trị tìm kiếm
            //if (searchString != "--_")
            //{
            //    index = 1;
            //}
            //else
            //{
            //    //Nếu chuỗi không có giá trị tìm kiếm
            //    //Gán chuỗi tìm kiếm là chuỗi tìm kiếm hiện tại lấy từ View
            //    searchString = Request["currentFilter"];
            //}
            List<Trouble> trouble = troubleDAO.ListAll();
            setRowWarning(trouble);

            ////Lưu lại lọc theo chuỗi tìm kiếm hiện tại.
            //ViewBag.CurrentFilter = searchString;

            //Sắp xếp danh sách theo chuỗi sortOrder
            troubles = deteDAO.Ordered_Troubles(troubles, sortOrder);

            //Tìm kiếm theo chuỗi searchString
            troubles = deteDAO.Search_Troubles(troubles, searchString);

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
                if (item.status == 1)
                    ViewData[item.id_Trouble.ToString()] = "bg-info";
                else if (item.status == 2)
                    ViewData[item.id_Trouble.ToString()] = "bg-warning";
            }
        }

        public JsonResult Get(string searchString, string sortOrder, string currentFilter, int? index)
        {
            TroubleDAO troublesDAO = new TroubleDAO();
            //Chuyển trạng thái sang chữ
            ViewData["0"] = "Sent"; ViewData["1"] = "Received"; ViewData["2"] = "Processing"; ViewData["3"] = "Finished";
            UserLogin userLogin = (UserLogin)Session["User_Session"];

            //Tạo danh sách các trouble theo user id
            List<Trouble> troubles = troublesDAO.ListByIdTech(userLogin.UserID);

            //Thêm thứ tự sắp xếp, cách săp xếp

            //Mặc định sắp xếp theo thời gian đến sớm nhất
            ViewBag.ReportDate_SortParm = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            //Sắp xếp theo level
            ViewBag.Level_SortParm = sortOrder == "level" ? "level_desc" : "level";

            //Lưu thứ tự sắp xếp hiện tại
            ViewBag.CurrentSort = sortOrder;

            switch (sortOrder)
            {
                //Sắp xếp theo level
                //Tăng dần
                case "level":
                    troubles = troubles.OrderBy(x => x.level).ToList();
                    break;
                //Giảm dần
                case "level_desc":
                    troubles = troubles.OrderByDescending(x => x.level).ToList();
                    break;

                //Sắp xếp theo thời gian đến trễ nhất -> sớm nhất
                case "date_desc":
                    troubles = troubles.OrderByDescending(t => t.dateStaff).ToList();
                    break;
                //Mặc định sớm nhất -> trễ nhất
                default:
                    troubles = troubles.OrderBy(t => t.dateStaff).ToList();
                    break;
            }

            //Thêm ô tìm kiếm

            //Kiểm tra nếu có chuỗi tìm kiếm
            if (searchString != null)
            {
                index = 1;
            }
            else
            {
                //Gán chuỗi tìm kiếm là chuỗi tìm kiếm hiện tại
                searchString = currentFilter;
            }
            //Lưu lại chuỗi tìm kiếm hiện tại.
            ViewBag.CurrentFilter = searchString;

            //Tìm kiếm theo ngày
            if (!String.IsNullOrEmpty(searchString))
            {
                troubles = troubles.Where(x => x.dateStaff.ToString().Contains(searchString)).ToList();
            }


            //Page size số lượng hàng(trouble) trong 1 trang
            int pageSize = 10;
            //Page index vị trí của trang mặc định là 1
            int pageIndex = (index ?? 1);

            return new JsonResult { Data = troubles.ToPagedList(pageIndex, pageSize), JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }


        //public ActionResult UpdateStatus(int? id)
        //{
        //    //try
        //    //{
        //    if (id != null)
        //    {
        //        //bool check = troublesDAO.UpdateStatus((int)id);
        //        bool check = troublesDAO.insertTech((int)id);
        //        if (check)
        //        {
        //            return RedirectToAction("Index");
        //        }

        //        ModelState.AddModelError("", "Cập nhật không thành công");
        //        return View();

        //    }

        //    ModelState.AddModelError("", "Không hợp lệ");
        //    return View();
        //    //}
        //    //catch
        //    //{
        //    //    return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
        //    //}
        //}

        public ActionResult Details(int? id)
        {
            if (id != null)
            {
                
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

                //Add thêm getter vào FAQ_DAO
                //FAQ_DAO FAQs = new FAQ_DAO();

                // //Thêm câu hỏi
                // ViewData["Question" + model.id_FAQ.ToString()] =
                //(FAQs.getFAQByID(model.id_FAQ) == null) ?
                //"none" : FAQs.getFAQByID(model.id_FAQ).question;

                // //Thêm câu trả lời
                // ViewData["Answer" + model.id_FAQ.ToString()] =
                //(FAQs.getFAQByID(model.id_FAQ) == null) ?
                //"none" : FAQs.getFAQByID(model.id_FAQ).answer;

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

        [HttpPost]
        public ActionResult Details(DetailTechMV model)
        {
            var dtDAO = new DetailTechDAO();
            var dtMvDao = new DetailTechMV_DAO();
            UserLogin userLogin = (UserLogin)Session["User_Session"];

            if (Request["Finish"] != null)
            {
                var kt = dtMvDao.FinishTrouble(model);
                if (kt >= 0)
                {
                    if (kt == 1) 
                    {
                        TroubleHub.NotifyFinish(model.id_Report, model.id_Trouble);
                    }
                    return RedirectToAction("Index");
                }
                else ModelState.AddModelError("", "Finish Fail");
            }

            if (Request["Processing"] != null)
            {
                if (dtDAO.TroubleShooting(model.id_DetailTech))
                {
                    return RedirectToAction("Index");
                }
                else ModelState.AddModelError("", "Process Fail");
            }

            if (Request["SendBack"] != null)
            {
                if (dtDAO.SendBack(model.id_DetailTech))
                {
                    return RedirectToAction("Index");
                }
                else ModelState.AddModelError("", "Send Back Fail");
            }


            return View("Index");
        }


        //Hàm thêm báo cáo y như bên Staff
        public ActionResult Insert()
        {

            return View(new Trouble());
        }

        [HttpPost]
        public ActionResult Insert(Trouble trouble)
        {
            if (ModelState.IsValid)
            {
                //var session = (UserLogin)Session[Common.CommonConstants.User_Session];
                //trouble.id_Fill = session.UserID;
                //var res = new TroubleDAO().insertStaff(trouble);
                //if (res)
                //{
                //    ViewBag.Success = "OK";
                //    ModelState.Clear();
                //    return View("Insert", new Trouble());
                //}
                //else
                //{
                //    ModelState.AddModelError("", "Insert Fail");
                //}
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
                    var res = new TroubleDAO().insertStaff(trouble);
                    if (res)
                    {
                        ViewBag.Success = "OK";
                        ModelState.Clear();
                        return View("Insert", new Trouble());
                    }
                    else
                    {
                        ModelState.AddModelError("", "Insert Fail");
                    }
                }
            }
            return View("Insert");
        }
    }
}