using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HelpDesk.Common
{
    [Serializable]
    public class UserLogin
    {

        [Display(Name = "ID")]
        public int UserID { get; set; }

        [Display(Name = "Tài khoản")]
        public String UserAccount { get; set; }

        public int rightt { get; set; }
    }
}