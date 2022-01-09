using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HelpDesk.Areas.Manager.Models
{
    public class UserModelView
    {
        //[Key]
        //public int id_user { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(50)]
        [Display(Name = "Username")]
        public string username { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(100)]
        [Display(Name = "Password")]
        public string pwd { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Required")]
        [Display(Name = "Name")]
        public string name { get; set; }

        [Display(Name = "Right")]
        public int rightt { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Confirm Password")]
        [Compare("pwd", ErrorMessage = "Confirm password doesn't match")]
        public string rePwd { get; set; }
    }
}