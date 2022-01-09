namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DetailTech")]
    public partial class DetailTech
    {
        [Key]
        public int id_DetailTech { get; set; }

        public int id_Trouble { get; set; }

        public int id_Tech { get; set; }

        public int status { get; set; }

        [Column(TypeName = "date")]
        public DateTime? dateFinish { get; set; }

        [Column(TypeName = "date")]
        public DateTime? deadline { get; set; }

        [StringLength(254)]
        public string describe { get; set; }

        public int? fixMethod { get; set; }

        [StringLength(254)]
        public string id_Bill { get; set; }

        [StringLength(254)]
        public string contentBill { get; set; }

        public int id_FAQ { get; set; }

        [StringLength(254)]
        public string describeFAQ { get; set; }

        public int? finishTime { get; set; }
    }
}
