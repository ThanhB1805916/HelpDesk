namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Device")]
    public partial class Device
    {
        [Key]
        public int id_Device { get; set; }

        public int? id_Category { get; set; }

        [StringLength(255)]
        public string nameDevice { get; set; }

        public int? id_User { get; set; }
    }
}
