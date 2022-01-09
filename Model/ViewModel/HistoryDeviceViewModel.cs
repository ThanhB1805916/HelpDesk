using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class HistoryDeviceViewModel
    {
        public int DeviceID { set; get; }

        public string Name { set; get; }

        public int TroubleID { set; get; }

        public int ReportID { set; get; }

        public int FillID { set; get; }

        public int ManagerID { set; get; }

        public DateTime DateStaff { set; get; }

        public int Status { set; get; }
    }
}
