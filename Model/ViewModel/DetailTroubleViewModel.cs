using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class DetailTroubleViewModel
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
        [Required(ErrorMessage = "Require")]
        public DateTime? dateManage { get; set; }

        [Column(TypeName = "date")]
        [Display(Name = "Finish Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? dateTech { get; set; }


        //detailTech

        public List<int> id_DetailTech { get; set; }

        [Display(Name = "Technician ID")]
        [Required(ErrorMessage = "Required")]
        public List<int> id_Tech { get; set; }

        [Display(Name = "Status")]
        public List<int> statusTech { get; set; }

        [Column(TypeName = "date")]
        [Display(Name = "Finish Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public List<DateTime?> dateFinish { get; set; }


        [Display(Name = "Describe of Manager")]
        public List<String> describeTech { get; set; }


        //FAQ
        [Display(Name = "ID FAQ")]
        public List<int> id_FAQ { get; set; }

        [Display(Name = "Describe Technician")]
        public List<string> describeFAQ { get; set; }

        [Display(Name = "Finish Time (Day)")]
        public List<int> finishTime { get; set; }


        public DetailTroubleViewModel()
        {
            id_Tech = new List<int>();
            //describeTech = new List<String>();
            dateFinish = new List<DateTime?>();
            statusTech = new List<int>();
            id_FAQ = new List<int>();
            finishTime = new List<int>();
            id_DetailTech = new List<int>();

        }
    }
}

