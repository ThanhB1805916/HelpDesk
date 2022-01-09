using HelpDesk.Areas.Manager.Models.TroubleModel;
using HelpDesk.Common;
using HelpDesk.Models.UserModel;
using Model;
using Model.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HelpDesk.Controllers
{
    //Authentication controller quản lý việc đăng nhập và đăng xuất của tài khoản trong vệ thống.
    public class AuthenticationController : Controller
    {
        //Hàm đăng nhập vào hệ thống
        // GET: Manager/Authentication/Login
        public ActionResult Login()
        {
            return View();
        }

        // Post: Manager/Authentication/Login
        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            //try
            //{
                //Kiểm tra nếu người dùng nhập đủ tài khoản và mật khẩu
                if (ModelState.IsValid)
                {

                    UserDAO dao = new UserDAO();

                    //checkLogin trả về kiểu int
                    // 0 là không tồn tại
                    // -1 là sai mật khẩu
                    // 1 là xác nhận thành công
                    int type = dao.checkLogin(model.UserAccount, model.PassWord);

                    if (type == 0)
                    {
                        ModelState.AddModelError("", "Invalid username and/ or password");
                        return View();
                    }
                    else
                    {
                        if (type == -1)
                        {
                            ModelState.AddModelError("", "The password is incorrect");
                            return View();
                        }
                        else
                        {
                            //Thành công lấy ra người dùng trùng với tài khoản cung cấp
                            var user = dao.getUserByUsername(model.UserAccount);
                            var UserSession = new UserLogin();
                            UserSession.UserID = user.id_user;
                            UserSession.UserAccount = user.username;
                            UserSession.rightt = user.rightt;
                            
                            //Thêm người dùng vào 1 session để có thể duy trì đăng nhập
                            Session.Add(CommonConstants.User_Session, UserSession);
                            Session.Add("right", user.rightt);

                            //Chuyển hướng trang người dùng theo Quyển(rightt)

                            //0 là Quản lý, 1 là Kỹ thuật viên, còn lại là nhân viên
                            if (user.rightt == 0)
                            {
                                return RedirectToAction("Index", "Trouble", new { area  = "Manager"});
                            }
                            else
                            {
                                if (user.rightt == 1)
                                {
                                    return RedirectToAction("Index", "Trouble", new { area = "Tech"});
                                }
                                else
                                {
                                    return RedirectToAction("Index", "Trouble");
                                }
                            }
                        }
                    }
                }

                ModelState.AddModelError("", "Try again later");
                return View();
            //}
            //catch
            //{
            //    //Lỗi xảy ra nếu truy xuất dữ liệu ở tần CSDL không thành công.
            //    return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            //}

        }

        //Hàm đăng xuất ra khỏi hệ thống
        //Gán cho session đăng nhập băng null
        public ActionResult Logout()
        {
            var session = (UserLogin)Session[CommonConstants.User_Session];
            if (session != null)
            {
                Session[CommonConstants.User_Session] = null;
            }
            return RedirectToAction("Login", "Authentication");
        }

        //Hàm đổi mật khẩu người dùng
        //Dựa theo session  
        //public ActionResult ChangeUserPassword()
        //{
        //    //Session lưu thông tin của người dùng hiện tại
        //    var session = (UserLogin)Session[CommonConstants.User_Session];
        //    if(session == null)
        //    {
        //        return RedirectToAction("Login", "Authentication");
        //    }
        //    SetLayout();
        //    return View();   
        //}
        //[HttpPost]
        //public ActionResult ChangeUserPassword(ChangePassword model)
        //{
        //    try
        //    {
        //        //Kiểm tra nếu mẫu hợp lệ
        //        if (ModelState.IsValid)
        //        {
        //            //Thêm hàm dao để truy xuất dữ liệu
        //            UserDAO dao = new UserDAO();

        //            //Session lưu thông tin của người dùng hiện tại
        //            var session = (UserLogin)Session[CommonConstants.User_Session];

        //            //Lấy người dùng theo tài khoản đăng nhập
        //            User user = dao.getUserByUsername(session.UserAccount);

        //            //Kiểm tra nếu người dùng tồn tại
        //            if (user == null)
        //            {
        //                ModelState.AddModelError("", "User not found");
        //                return View();
        //            }
        //            else
        //            {
        //                SetLayout();
        //                if (user.pwd == model.CurrentPassword)
        //                {
        //                    user.pwd = model.NewPassword;
        //                    dao.UpdateUserInfo(user);
        //                    ViewBag.success = "Update Successfully";
        //                    return View();
        //                }
        //                //Nếu không trùng thông báo lỗi
        //                else
        //                {
        //                    ModelState.AddModelError("", "Current password is incorrect");
        //                    return View();
        //                }
        //            }
        //        }

        //        ModelState.AddModelError("", "Error please try again");
        //        return View();
        //    }
        //    catch
        //    {
        //        // Lỗi sẽ xảy ra khi truy vấn CSDL gặp vấn đề
        //        return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
        //    }

        //}

        //public ActionResult AccountInfo()
        //{
        //    var session = (UserLogin)Session[CommonConstants.User_Session];
        //    if (session == null)
        //    {
        //        return RedirectToAction("Login", "Authentication");
        //    }
        //    SetLayout();
        //    ViewData["0"] = "Manager";
        //    ViewData["1"] = "Technical Support";
        //    ViewData["2"] = "Staff";
        //    var model = new UserDAO().getUserByUsername(session.UserAccount);
        //    return View(model);
        //}

        //void SetLayout()
        //{
        //    var user = (UserLogin)Session[CommonConstants.User_Session];
        //    switch (user.rightt)
        //    {
        //        case 0: ViewBag.Layout = "~/Areas/Manager/Views/Shared/_LayoutTrouble.cshtml"; break;
        //        case 1: ViewBag.Layout = "~/Areas/Tech/Views/Shared/_LayoutTrouble.cshtml"; break;
        //        default: ViewBag.Layout = "~/Views/Shared/_LayoutTrouble.cshtml"; break;
        //    }
        //}
    }
}