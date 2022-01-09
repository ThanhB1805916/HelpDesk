using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HelpDesk.Areas.Tech.Models
{
    public class DetailTechMV
    {
        [Key]
        [Display(Name = "Trouble ID")]
        public int id_Trouble { get; set; }

        [Display(Name = "Report ID")]
        [Required(ErrorMessage = "Required")]
        public int id_Report { get; set; }

        [Display(Name = "Fill ID")]
        public int id_Fill { get; set; }

        [Display(Name = "Manager ID")]
        public int id_Manage { get; set; }

        [Display(Name = "Device ID")]
        [Required(ErrorMessage = "Required")]
        public int id_Device { get; set; }


        [StringLength(255)]
        [Display(Name = "Describe")]
        [Required(ErrorMessage = "Required")]
        public string describe { get; set; }

        [StringLength(255)]
        [Display(Name = "Image")]
        public string images { get; set; }

        [Display(Name = "Status")]
        public int status { get; set; }

        [Display(Name = "Level")]
        public int level { get; set; }

        [Column(TypeName = "date")]
        [Display(Name = "Report Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime dateStaff { get; set; }

        [Column(TypeName = "date")]
        [Display(Name = "Expected Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? dateManage { get; set; }

        [Column(TypeName = "date")]
        [Display(Name = "Finish Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? dateTech { get; set; }


        //detailTech
        [Display(Name = "Technician ID")]
        [Required(ErrorMessage = "Required")]
        public int id_Tech { get; set; }

        [Display(Name = "Status")]
        public int statusTech { get; set; }

        [Column(TypeName = "date")]
        [Display(Name = "Finish Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? dateFinish { get; set; }

        [Column(TypeName = "date")]
        [Display(Name = "Deadline")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? deadline { get; set; }


        [Display(Name = "Describe of Manager")]
        public string describeTech { get; set; }

        public int id_DetailTech { get; set; }

        [Required(ErrorMessage = "Require")]
        [Display(Name = "Fix Method")]
        public int? fixMethod { get; set; }

        //FAQ
        [Display(Name = "ID FAQ")]
        public int? id_FAQ { get; set; }

        //DetailDevice
        [Display(Name = "ID Bill")]
        [StringLength(254)]
        public string id_Bill { get; set; }

        [StringLength(254)]
        [Display(Name ="Content Bill")]
        public string contentBill { get; set; }

        [Display(Name = "Finish Time (Day)")]
        public int? finishTime { get; set; }

        [StringLength(254)]
        [Display(Name = "Describe Technician")]
        public string describeFAQ { get; set; }
    }
}