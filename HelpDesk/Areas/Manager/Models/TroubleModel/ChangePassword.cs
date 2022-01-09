using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HelpDesk.Areas.Manager.Models.TroubleModel
{
    //Mẫu dành cho đổi mật khẩu
    [Serializable]
    public class ChangePassword
    {
        //Lấy vào id
        public int ID { get; set; }

        //Mật khẩu cũ
        [Required(ErrorMessage ="A password is required")]
        [Display(Name = "oldpwd", ResourceType = typeof(StaticResource.Resource))]
        public String CurrentPassword { get; set; }

        //Mật khẩu mới
        [Required(ErrorMessage = "A password is required")]
        [Display(Name = "newpwd", ResourceType = typeof(StaticResource.Resource))]
        public String NewPassword { get; set; }

        //Nhập lại mật khẩu mới
        [Required(ErrorMessage = "A password is required")]
        [Display(Name = "renewpwd", ResourceType = typeof(StaticResource.Resource))]
        [Compare("NewPassword", ErrorMessage = "The Confirm is not match. Please type again")]
        public String RetypeNewPassword { get; set; }

    }
}