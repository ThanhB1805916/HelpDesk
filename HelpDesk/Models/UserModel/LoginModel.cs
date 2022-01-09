using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HelpDesk.Models.UserModel
{
    public class LoginModel
    {
        [Key]
        [Required(ErrorMessage = "Phần này không được bỏ trống")]
        [Display(Name = "ID")]
        public int ID { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Username")]
        public String UserAccount { get; set; }
        [Required(ErrorMessage = "Required")]
        [Display(Name = "Password")]
        public String  PassWord { get; set; }
    }
}