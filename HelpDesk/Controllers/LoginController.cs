using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using HelpDesk.Models.UserModel;
using Model.DAO;
using HelpDesk.Common;

namespace HelpDesk.Controllers
{
    public class LoginController : Controller
    {
        // GET: Manager/Login
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(LoginModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UserDAO dao = new UserDAO();
                    int type = dao.checkLogin(model.UserAccount, model.PassWord);
                    if (type == 0)
                    {
                        ModelState.AddModelError("", "Username not exist");
                        return View();
                    }
                    else
                    {
                        if(type == -1)
                        {
                            ModelState.AddModelError("", "Wrong password");
                            return View();
                        }
                        else
                        {
                            var user= dao.getUserByUsername(model.UserAccount);
                            var UserSession = new UserLogin();
                            UserSession.UserID = user.id_user;
                            UserSession.UserAccount = user.username;
                            UserSession.rightt = user.rightt;

                            Session.Add(CommonConstants.User_Session, UserSession);
                            Session.Add("right", user.rightt);

                           if(user.rightt == 0)
                            {
                                return RedirectToAction("Index", "Manager");
                            }
                           else
                            {
                                if (user.rightt == 1)
                                {
                                    return RedirectToAction("Index", "Tech");
                                }
                                else
                                {
                                    return RedirectToAction("Index", "Home");
                                }
                            }
                        }
                    }
                }
                return View();
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           
        }
    }
}