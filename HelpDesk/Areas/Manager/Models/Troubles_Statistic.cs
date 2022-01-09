using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HelpDesk.Areas.Manager.Models
{
    public class Troubles_Statistic
    {
        //Lưu số lượng trouble 
        [Display(Name = "Troubles Cardinality")]
        public int Troubles_Card { get; set; }

        //Lưu số lượng Status
        [Display(Name = "Sent")]
        public int Troubles_Sent { get; set; }
       
        [Display(Name = "Recieved")]
        public int Troubles_Received { get; set; }
       
        [Display(Name = "Processing")]
        public int Troubles_Processing { get; set; }
       
        [Display(Name = "Finished")]
        public int Troubles_Finished { get; set; }

    }
}