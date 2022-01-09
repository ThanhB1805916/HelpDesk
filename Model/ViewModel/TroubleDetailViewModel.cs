using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class TroubleDetailViewModel
    {
        public int DeviceID { set; get; }

        public string Name { set; get; }

        public int TroubleID { set; get; }

        public int ReportID { set; get; }

        public int FillID { set; get; }

        public int ManagerID { set; get; }

        public DateTime DateStaff { set; get; }

        public DateTime? DateTech { set; get; }

        public DateTime? DateManager { set; get; }

        public int Status { set; get; }

        public string Images { set; get; }

        public string Describe { set; get; }

        public int Level { set; get; }
    }
}
