using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HelpDesk.Areas.Manager.Models.TroubleModel
{
    public class TroubleShareModelView
    {
        [Key]
        public int id_Trouble { get; set; }

        [Display(Name = "Report ID")]
        [Required(ErrorMessage = "Required")]
        public int id_Report { get; set; }

        [Display(Name = "Fill ID")]
        public int id_Fill { get; set; }

        [Display(Name = "Manager ID")]
        public int id_Manage { get; set; }

        [Display(Name = "id_device", ResourceType = typeof(StaticResource.Resource))]
        [Required(ErrorMessage = "Required")]
        public int id_Device { get; set; }


        [StringLength(255)]
        [Display(Name = "describe", ResourceType = typeof(StaticResource.Resource))]
        [Required(ErrorMessage = "Required")]
        public string describe { get; set; }

        [StringLength(255)]
        [Display(Name = "image", ResourceType = typeof(StaticResource.Resource))]
        public string images { get; set; }

        [Display(Name = "status", ResourceType = typeof(StaticResource.Resource))]
        public int status { get; set; }

        [Display(Name = "level", ResourceType = typeof(StaticResource.Resource))]
        public int level { get; set; }

        [Column(TypeName = "date")]
        [Display(Name = "dateStaff", ResourceType = typeof(StaticResource.Resource))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime dateStaff { get; set; }

        [Column(TypeName = "date")]
        [Display(Name = "dateManage", ResourceType = typeof(StaticResource.Resource))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? dateManage { get; set; }

        [Column(TypeName = "date")]
        [Display(Name = "dateTech", ResourceType = typeof(StaticResource.Resource))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? dateTech { get; set; }


        //detailTech

        public List<int> id_DetailTech { get; set; }

        [Display(Name = "idTech", ResourceType = typeof(StaticResource.Resource))]
        [Required(ErrorMessage = "Required")]
        //[RegularExpression("([1-9][0-9]*)", ErrorMessage = "Must be a number")]
        public List<int> id_Tech { get; set; }

        [Display(Name = "status", ResourceType = typeof(StaticResource.Resource))]
        public List<int> statusTech { get; set; }

        [Column(TypeName = "date")]
        [Display(Name = "dateFinish", ResourceType = typeof(StaticResource.Resource))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public List<DateTime?> dateFinish { get; set; }

        [Column(TypeName = "date")]
        [Display(Name = "deadline", ResourceType = typeof(StaticResource.Resource))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public List<DateTime?> deadline { get; set; }


        [Display(Name = "taskDes", ResourceType = typeof(StaticResource.Resource))]
        public List<String> describeTech { get; set; }


        //FAQ
        [Display(Name = "idFAQ", ResourceType = typeof(StaticResource.Resource))]
        //[RegularExpression("([1-9][0-9]*)", ErrorMessage = "Must be a number")]
        public List<int> id_FAQ { get; set; }

        [Display(Name = "techDes", ResourceType = typeof(StaticResource.Resource))]
        public List<string> describeFAQ { get; set; }

        [Display(Name = "timeFinish", ResourceType = typeof(StaticResource.Resource))]
        public List<int> finishTime { get; set; }

        
        public TroubleShareModelView()
        {
            id_Tech = new List<int>();
            //describeTech = new List<String>();
            dateFinish = new List<DateTime?>();
            deadline = new List<DateTime?>();
            statusTech = new List<int>();
            id_FAQ = new List<int>();
            finishTime = new List<int>();
            id_DetailTech = new List<int>();
            
        }
    }
}