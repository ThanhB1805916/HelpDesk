namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class User
    {
        [Key]
        [Display(Name = "id_user", ResourceType = typeof(StaticResource.Resource))]
        public int id_user { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(50)]
        [Display(Name = "account", ResourceType = typeof(StaticResource.Resource))]
        public string username { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(100)]
        [Display(Name = "pwd", ResourceType = typeof(StaticResource.Resource))]
        public string pwd { get; set; }

        [StringLength(50)]
        [Display(Name = "fullName", ResourceType = typeof(StaticResource.Resource))]
        public string name { get; set; }

        [Display(Name = "right", ResourceType = typeof(StaticResource.Resource))]
        public int rightt { get; set; }
    }
}
