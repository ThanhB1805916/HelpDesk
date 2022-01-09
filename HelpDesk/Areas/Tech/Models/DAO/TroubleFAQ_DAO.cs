using HelpDesk.Models.UserModel;
using Model.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpDesk.Areas.Tech.Models.DAO
{
    public class TroubleFAQ_DAO
    {
        public TroubleFAQ_MV GetTrouble(int idDetail)
        {
            TroubleFAQ_MV model = new TroubleFAQ_MV();
            var troubleDAO = new TroubleDAO();
            var detailDAO = new DetailTechDAO();
            var detail = detailDAO.GetDetailTech(idDetail);
            var trouble = troubleDAO.GetTrouble(detail.id_Trouble);

            //convert
            model.id_Trouble = trouble.id_Trouble;
            model.id_Report = trouble.id_Report;
            model.id_Fill = trouble.id_Fill;
            model.id_Device = trouble.id_Device;
            model.id_Manage = trouble.id_Manage;
            model.describe = trouble.describe;
            model.level = trouble.level;
            model.images = trouble.images;
            model.dateStaff = trouble.dateStaff;
            model.dateManage = trouble.dateManage;
            model.dateTech = trouble.dateTech;

            model.id_Tech = detail.id_Tech;
            model.id_FAQ = detail.id_FAQ;
            model.describeFAQ = detail.describeFAQ;
            model.describeTech = detail.describe;
            model.finishTime = (int)detail.finishTime;
            model.dateFinish = detail.dateFinish;
            model.deadline = detail.deadline;
            model.id_DetailTech = detail.id_DetailTech;
            
            return model;
        }
    }
}