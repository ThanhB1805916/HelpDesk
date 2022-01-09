using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HelpDesk.Models.UserModel
{
    public class FAQModelView
    {
        [Key]
        [Display(Name = "ID FAQ")]
        public int id_FAQ { get; set; }

        [StringLength(255)]
        [Display(Name = "Question")]
        [Required(AllowEmptyStrings = false)]
        public string question { get; set; }

        [StringLength(255)]
        [Display(Name = "Answer")]
        [Required(AllowEmptyStrings = false)]
        public string answer { get; set; }

        [Display(Name = "Right")]
        public int? rightt { get; set; }

        [Display(Name = "Count")]
        public int? countt { get; set; }

        //
        public List<int> id_DetailTech { get; set; }

        public List<int> id_Trouble { get; set; }

        [StringLength(254)]
        public List<string> describeFAQ { get; set; }

        public List<int> finishTime { get; set; }

        public List<int> id_Tech { get; set; }

        public FAQModelView()
        {
            id_DetailTech = new List<int>();
            id_Trouble = new List<int>();
            finishTime = new List<int>();
            id_Tech = new List<int>();
            describeFAQ = new List<string>();
        }
    }
}