using HelpDesk.Areas.Manager.Models.TroubleModel;
using HelpDesk.Common;
using Model;
using Model.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HelpDesk.Controllers
{
    public class AccountController : BaseController
    {
        public ActionResult ChangeUserPassword()
        {
            //Session lưu thông tin của người dùng hiện tại
            //var session = (UserLogin)Session[CommonConstants.User_Session];
            //if (session == null)
            //{
            //    return RedirectToAction("Login", "Authentication");
            //}
            SetLayout();
            return View();
        }
        [HttpPost]
        public ActionResult ChangeUserPassword(ChangePassword model)
        {
            SetLayout();
            try
            {
                //Kiểm tra nếu mẫu hợp lệ
                if (ModelState.IsValid)
                {
                    //Thêm hàm dao để truy xuất dữ liệu
                    UserDAO dao = new UserDAO();

                    //Session lưu thông tin của người dùng hiện tại
                    var session = (UserLogin)Session[CommonConstants.User_Session];

                    //Lấy người dùng theo tài khoản đăng nhập
                    User user = dao.getUserByUsername(session.UserAccount);

                    //Kiểm tra nếu người dùng tồn tại
                    if (user == null)
                    {
                        ModelState.AddModelError("", "User not found");
                        return View();
                    }
                    else
                    {
                        
                        if (user.pwd == model.CurrentPassword)
                        {
                            user.pwd = model.NewPassword;
                            dao.UpdateUserInfo(user);
                            ViewBag.success = StaticResource.Resource.updateSuccess;
                            return View();
                        }
                        //Nếu không trùng thông báo lỗi
                        else
                        {
                            ModelState.AddModelError("", StaticResource.Resource.currentPwdFalse);
                            return View();
                        }
                    }
                }

                ModelState.AddModelError("", "Error please try again");
                return View();
            }
            catch
            {
                // Lỗi sẽ xảy ra khi truy vấn CSDL gặp vấn đề
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

        }

        public ActionResult AccountInfo()
        {
            var session = (UserLogin)Session[CommonConstants.User_Session];
            //if (session == null)
            //{
            //    return RedirectToAction("Login", "Authentication");
            //}
            SetLayout();
            ViewData["0"] = StaticResource.Resource.manager;
            ViewData["1"] = StaticResource.Resource.technician;
            ViewData["2"] = StaticResource.Resource.nameStaff;
            
            
            var model = new UserDAO().getUserByUsername(session.UserAccount);
            return View(model);
        }

        void SetLayout()
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