namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Trouble")]
    public partial class Trouble
    {
        [Key]
        [Display(Name = "id_trouble", ResourceType = typeof(StaticResource.Resource))]
        public int id_Trouble { get; set; }

        [Display(Name = "id_report", ResourceType = typeof(StaticResource.Resource))]
        [Required(ErrorMessage = "Required")]
        public int id_Report { get; set; }

        [Display(Name = "id_fill", ResourceType = typeof(StaticResource.Resource))]
        public int id_Fill { get; set; }

        [Display(Name = "id_manage", ResourceType = typeof(StaticResource.Resource))]
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

    }
}
