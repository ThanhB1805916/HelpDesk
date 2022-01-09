using Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class DeviceDAO
    {
        private Model db;

        public DeviceDAO()
        {
            db = new Model();
        }

        //Hàm trả về thiết bị theo id
        public Device getDeviceByID(int id)
        {
            return this.db.Devices.Find(id);
        }

        public List<Device> ListByUserID(int id)
        {
            return db.Devices.Where(x => x.id_User == id).ToList();
        }

        //Lấy danh sách lịch sử Device bằng ID Device
        public List<HistoryDeviceViewModel> ListDeviceHistoryByDeviceID(int id)
        {
            var model = from a in db.Troubles
                        join b in db.Devices
                        on a.id_Device equals b.id_Device
                        where a.id_Device == id
                        select new HistoryDeviceViewModel()
                        {
                            DeviceID = a.id_Device,
                            Name = b.nameDevice,
                            TroubleID = a.id_Trouble,
                            ReportID = a.id_Report,
                            FillID = a.id_Fill,
                            ManagerID = a.id_Manage,
                            DateStaff = a.dateStaff,
                            Status = a.status
                        };
            return model.ToList();
        }

    }
}
