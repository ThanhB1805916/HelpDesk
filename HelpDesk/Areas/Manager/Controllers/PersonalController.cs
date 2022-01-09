using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HelpDesk.Areas.Manager.Models;
using HelpDesk.Areas.Manager.Models.TroubleModel;
using HelpDesk.Common;
using Model;
using Model.DAO;
using PagedList;

namespace HelpDesk.Areas.Manager.Controllers
{
    public class PersonalController : BaseController
    {
        UserDAO dao = new UserDAO();
        // GET: Manager/Personal
        public ActionResult Index(int? index)
        {
            ViewData["right-0"] = StaticResource.Resource.manager;
            ViewData["right-1"] = StaticResource.Resource.technician;
            ViewData["right-2"] = StaticResource.Resource.nameStaff;

            ViewData["icon-role-0"] = "fas fa-user-tie";
            ViewData["icon-role-1"] = "fas fa-user-cog";
            ViewData["icon-role-2"] = "fas fa-user";

            int pageIndex = (index ?? 1);
            int pageSize = 10;
            return View(dao.ListAllUser().ToPagedList(pageIndex, pageSize));
        }

        // GET: Manager/Personal/Details/5
        public ActionResult Details(int? id)
        {
            //Kiểm tra nếu id hợp lệ
            if(id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadGateway);
            }
            else
            {
                //Tìm kiếm người dùng theo id
                User user = dao.getUserById((int)id);
                if(user == null)
                {
                    ModelState.AddModelError("", "Error: User not found");
                    return View();
                }

                //Thêm tên cho các quyền theo rightt
                ViewData["0"] = StaticResource.Resource.nameManage;
                ViewData["1"] = StaticResource.Resource.nameTech;
                ViewData["2"] = StaticResource.Resource.nameStaff;
                return View(user);
            }
        }

        // GET: Manager/Personal/Create
        public ActionResult Create()
        {
            UserModelView model = new UserModelView();
            return View(model);
        }

        // POST: Manager/Personal/Create
        [HttpPost]
        public ActionResult Create(UserModelView model)
        {
            try
            {
                //Kiểm tra nếu mẫu user truyền vào là hợp lệ
                if (ModelState.IsValid)
                {
                    User user = new User();
                    user.username = model.username;
                    user.pwd = model.pwd;
                    user.rightt = model.rightt;
                    user.name = model.name;
                    //Hàm trả về true nếu thêm người dùng thành công vào CSDL
                    bool success = dao.AddUser(user);

                    //Nếu thành công trở về trang danh sách
                    if (success)
                    {
                        return RedirectToAction("Index");
                    }
                    //Không thành công hiện thông báo lỗi
                    else
                    {
                        ModelState.AddModelError("", StaticResource.Resource.accountExist);
                        return View(model);
                    }
                }

                //Mẫu không hợp lệ hiện thông báo lỗi
                ModelState.AddModelError("", StaticResource.Resource.createUserFail);
                return View();
            }
            catch
            {
                //Lỗi sẽ xảy ra khi truy vấn CSDL gặp vấn đề
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
        }

        // GET: Manager/Personal/Edit/5
        //Hàm chỉnh sửa thông tin của người dùng
        public ActionResult Edit(int? id)
        {
            //Kiểm tra nếu id hợp lệ
            if(id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadGateway);
            }

            //Trả về một người dùng lấy theo id
            return View(dao.getUserById((int)id));
        }

        // POST: Manager/Personal/Edit/5
        //Nhập vào một mẫu user
        [HttpPost]
        public ActionResult Edit(User user)
        {
            try
            {
                //Kiểm tra nếu mẫu hợp lệ
                if(ModelState.IsValid)
                {
                   //Hàm UpdateUser trả về true nếu update thành công                 
                   if (dao.update(user)==1)
                    {
                        //Nếu thành công chuyển qua trang chi tiết
                        return RedirectToAction("Details", new { id = user.id_user });
                    }

                   //Không thành công hiện thông báo lỗi
                    ModelState.AddModelError("", StaticResource.Resource.editUserFail);
                    return View();
                }

                //Mẫu không hợp hệ hiện thông báo lỗi
                ModelState.AddModelError("", StaticResource.Resource.invalid);
                return View();
            }
            catch
            {
                // Lỗi sẽ xảy ra khi truy vấn CSDL gặp vấn đề
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
        }

        

        // POST: Manager/Personal/Delete/5
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            try
            {
                //Kiểm tra nếu tài khoản trùng với tài khoản đang đăng nhập.
                var session = (UserLogin)Session[Common.CommonConstants.User_Session];
                User user = dao.getUserById(id);
                if(user.username == session.UserAccount)
                {
                    //Không xóa được tài khoản đang đăng nhập
                    return RedirectToAction("Delete");
                }

                //Nếu xóa xong quay về trang danh sách
                dao.DeleteUser(user);
                return RedirectToAction("Index");
            }
            catch
            {
                // Lỗi sẽ xảy ra khi truy vấn CSDL gặp vấn đề
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
        }

        [HttpPost]
        public ActionResult Details(User user)
        {
            UserDAO dao = new UserDAO();

            if (Request["Edit"] != null)
            {
                //if (ModelState.IsValid)
                //{
                    //Hàm UpdateUser trả về true nếu update thành công                 
                    if (dao.update(user)==1)
                    {
                        //Nếu thành công chuyển qua trang chi tiết
                        return RedirectToAction("Details", new { id = user.id_user });
                    }

                    else ModelState.AddModelError("", StaticResource.Resource.editUserFail);
                    return View();
                //}

                ////Mẫu không hợp hệ hiện thông báo lỗi
                //ModelState.AddModelError("", " Không Hợp lệ");
                //return View();
            }

            if (Request["Delete"] != null)
            {
                return RedirectToAction("Delete", new { id = user.id_user});
            }

            if (Request["ChangePassword"] != null)
            {
                return RedirectToAction("ChangeUserPassword", new { id = user.id_user});
                
            }


            return View("Index");
        }

        //Thanh Add
        //Thêm hàm đổi mật khẩu theo id người dùng
        public ActionResult ChangeUserPassword(int? id)
        {
            //Kiểm tra nếu id hợp lệ
            if(id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadGateway);
            }
            else
            {

                ChangePassword model = new ChangePassword() { ID = (int)id };
                
             return View(model);
            }
        }
        [HttpPost]
        public ActionResult ChangeUserPassword(ChangePassword model)
        {
            try
            {
                //Kiểm tra nếu mẫu hợp lệ
                if(ModelState.IsValid)
                {
                    User user = dao.getUserById(model.ID);
                    //Kiểm tra nếu người dùng tồn tại
                    if(user == null)
                    {
                        ModelState.AddModelError("", "User not found");
                        return View();
                    }
                    else
                    {
                        //Kiểm tra nếu mật khẩu trùng với mật khẩu hiện tại
                        if(user.pwd == model.CurrentPassword)
                        {
                            user.pwd = model.NewPassword;
                            dao.UpdateUserInfo(user);
                            ModelState.AddModelError("", StaticResource.Resource.updateSuccess);
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

                ModelState.AddModelError("", StaticResource.Resource.errorTryAgain);
                return View();
            }
            catch
            {
                // Lỗi sẽ xảy ra khi truy vấn CSDL gặp vấn đề
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
           
        }
    }
}
