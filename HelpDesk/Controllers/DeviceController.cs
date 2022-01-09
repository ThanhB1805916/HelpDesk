using HelpDesk.Common;
using Model.DAO;
using Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HelpDesk.Controllers
{
    public class DeviceController : BaseController
    {
        // GET: Device
        public ActionResult Index()
        {
            var user = (UserLogin)Session[CommonConstants.User_Session];

            ViewData["deviceCategory-1"] = StaticResource.Resource.screen;
            ViewData["deviceCategory-2"] = StaticResource.Resource.printer;
            ViewData["deviceCategory-3"] = StaticResource.Resource.keyboard;
            ViewData["deviceCategory-4"] = StaticResource.Resource.camera;
            ViewData["deviceCategory-5"] = StaticResource.Resource.mouse;

            ViewBag.ListDevice = new DeviceDAO().ListByUserID(user.UserID);
            return View();
        }

        public ActionResult History(int id)
        {
            var device = new DeviceDAO().getDeviceByID(id);
            ViewBag.ListHistory = new DeviceDAO().ListDeviceHistoryByDeviceID(device.id_Device);
            ViewBag.DeviceID = device.id_Device;
            ViewBag.DeviceName = device.nameDevice;
            return View();
        }

        [HttpGet]
        public JsonResult GetDevice(int id)
        {
            UserDAO userDAO = new UserDAO();
            if (userDAO.getUserById(id) != null)
            {
                DeviceDAO dao = new DeviceDAO();
                var listDevice = dao.ListByUserID(id);
                return new JsonResult { Data = listDevice, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else return new JsonResult { Data = false, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }
    }
}