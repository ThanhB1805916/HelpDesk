using Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class TroubleDetailDAO
    {
        public TroubleDetailViewModel ConvertTrouble(int id)
        {
            TroubleDetailViewModel model = new TroubleDetailViewModel();

            var daoTrouble = new TroubleDAO();
            var trouble = daoTrouble.GetTrouble((int)id);

            //trouble
            model.TroubleID = trouble.id_Trouble;
            model.ReportID = trouble.id_Report;
            model.FillID = trouble.id_Fill;
            model.ManagerID = trouble.id_Manage;
            model.DeviceID = trouble.id_Device;
            model.Status = trouble.status;
            model.Level = trouble.level;
            model.Images = trouble.images;
            model.Describe = trouble.describe;
            model.DateStaff = trouble.dateStaff;
            model.DateManager = trouble.dateManage;
            model.DateTech = trouble.dateTech;

            return model;
        }
    }
}
