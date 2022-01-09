namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FAQ")]
    public partial class FAQ
    {
        [Key]
        [Display(Name = "ID FAQ")]
        public int id_FAQ { get; set; }

        [StringLength(255)]
        [Display(Name = "question",  ResourceType = typeof(StaticResource.Resource))]
        [Required(AllowEmptyStrings = false)]
        public string question { get; set; }

        [StringLength(255)]
        [Display(Name = "answer", ResourceType = typeof(StaticResource.Resource))]
        [Required(AllowEmptyStrings = false)]
        public string answer { get; set; }

        [Display(Name = "right", ResourceType = typeof(StaticResource.Resource))]
        public int? rightt { get; set; }

        [Display(Name = "count", ResourceType = typeof(StaticResource.Resource))]
        public int? countt { get; set; }

        [Display(Name = "idAddFAQ", ResourceType = typeof(StaticResource.Resource))]
        public int id_User { get; set; }

        public FAQ(int id_FAQ, string question)
        {
            this.id_FAQ = id_FAQ;
            this.question = question;
        }

        public FAQ() { }
    }
}
