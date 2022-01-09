using Model;
using Model.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpDesk.Areas.Tech.Models.DAO
{
    public class DetailTechMV_DAO
    {
        
        public DetailTechMV GetDetailTechMV(int idTrouble, int idTech)
        {
            var detailTechMV = new DetailTechMV();
            var troubleDAO = new TroubleDAO();
            var detailTechDAO = new DetailTechDAO();
            var trouble = troubleDAO.GetTrouble(idTrouble);
            var detail = detailTechDAO.GetDetailTech(idTrouble, idTech);
            detailTechMV = ConvertToMV(trouble, detail);
            return detailTechMV;
        }

        public DetailTechMV GetDetailTechMV(int idDetailTech)
        {
            var detailTechMV = new DetailTechMV();
            var troubleDAO = new TroubleDAO();
            var detailTechDAO = new DetailTechDAO();
            var trouble = troubleDAO.GetTrouble(detailTechDAO.GetIdTrouble(idDetailTech));
            var detail = detailTechDAO.GetDetailTech(idDetailTech);
            detailTechMV = ConvertToMV(trouble, detail);
            return detailTechMV;
        }

        public DetailTechMV ConvertToMV(Trouble trouble, DetailTech detail)
        {
            var detailTechMV = new DetailTechMV();
            //troube
            detailTechMV.id_Trouble = trouble.id_Trouble;
            detailTechMV.id_Fill = trouble.id_Fill;
            detailTechMV.id_Report = trouble.id_Report;
            detailTechMV.id_Manage = trouble.id_Manage;
            detailTechMV.id_Device = trouble.id_Device;
            detailTechMV.level = trouble.level;
            detailTechMV.status = trouble.status;
            detailTechMV.images = trouble.images;
            detailTechMV.describe = trouble.describe;
            detailTechMV.dateStaff = trouble.dateStaff;
            detailTechMV.dateManage = trouble.dateManage;
            detailTechMV.dateTech = trouble.dateTech;
            //detailtech
            detailTechMV.id_Tech = detail.id_Tech;
            detailTechMV.statusTech = detail.status;
            detailTechMV.describeTech = detail.describe;
            detailTechMV.dateFinish = detail.dateFinish;
            detailTechMV.deadline = detail.deadline;
            detailTechMV.id_DetailTech = detail.id_DetailTech;
            detailTechMV.fixMethod = detail.fixMethod;
            detailTechMV.id_Bill = detail.id_Bill;
            detailTechMV.contentBill = detail.contentBill;

            detailTechMV.id_FAQ = detail.id_FAQ;
            detailTechMV.describeFAQ = detail.describeFAQ;
            detailTechMV.finishTime = detail.finishTime;
                                    
            return detailTechMV;
        }

        public List<DetailTechMV> ListDeTeMV(int idTech)
        {
            
            var listDeTeMV = new List<DetailTechMV>();
            var troubleDAO = new TroubleDAO();
            var deteDAO = new DetailTechDAO();
            var listTrouble = troubleDAO.ListByIdTech(idTech);
            var listDeTe = deteDAO.ListByIdTech(idTech);

            for(int i = 0; i< listTrouble.Count; i++)
            {
                listDeTeMV.Add(ConvertToMV(listTrouble[i], listDeTe[i]));
            }
            return listDeTeMV;
        }

        // -1 : false
        //  0 : finishTech true
        //  1 : finishTrouble true
        public int FinishTrouble(DetailTechMV model)
        {
            var detailTech = new DetailTech();
            detailTech.id_DetailTech = model.id_DetailTech;
            detailTech.id_Trouble = model.id_Trouble;
            detailTech.id_Tech = model.id_Tech;
            detailTech.id_Bill = model.id_Bill;
            detailTech.contentBill = model.contentBill;
            detailTech.fixMethod = model.fixMethod;
            if(model.id_FAQ > 0)
            {
                detailTech.id_FAQ = (int)model.id_FAQ;
                detailTech.describeFAQ = model.describeFAQ;
                detailTech.finishTime = model.finishTime;
                detailTech.deadline = model.deadline;
            }
            var deteDAO = new DetailTechDAO();
            return deteDAO.FinishTech(detailTech);            
        }

        //Thanh ADD
        //Hàm sắp xếp
        public List<DetailTechMV> Ordered_Troubles(List<DetailTechMV> listDeTeMV, string sortOrder)
        {
            switch (sortOrder)
            {
                //sắp xếp theo level tăng dần - giảm dần 
                case "level":  return listDeTeMV.OrderBy(x => x.level).ToList();
                case "level_Desc": return listDeTeMV.OrderByDescending(x => x.level).ToList();

                //sắp xếp theo ngày báo cáo tăng dần - giảm dần
                case "dateReport": return listDeTeMV.OrderBy(x => x.dateStaff).ToList();
                case "dateReport_Desc": return listDeTeMV.OrderByDescending(x => x.dateStaff).ToList();
            }
            //Thứ tự sắp xếp mặc định status -> level -> expecteddate
            return listDeTeMV.OrderBy(x => x.status).ThenByDescending(x => x.id_Trouble).ToList();
        }
        //Tìm kiếm
        public List<DetailTechMV> Search_Troubles(List<DetailTechMV> listDeTeMV, string searchString)
        {
            try
            {
                if (searchString != null)
                {
                    
                    string[] search = searchString.Split('_');

                    if (!String.IsNullOrEmpty(search[0])&&!String.IsNullOrEmpty(search[1]))
                    {

                        listDeTeMV = listDeTeMV.Where(x => x.dateStaff >= DateTime.Parse(search[0]) && x.dateStaff <= DateTime.Parse(search[1])).ToList();
                        
                    }

                    //Tìm kiếm theo level
                    if (!String.IsNullOrEmpty(search[2]))
                    {
                        listDeTeMV = listDeTeMV.Where(x => x.level == int.Parse(search[2])).ToList();
                    }
                }
                return listDeTeMV;
            }
            //Ngoại lệ người dùng nhập kí tự vào ô tìm kiếm "ngày" trả về danh sách không lọc
            catch
            {
                return listDeTeMV;
            }
        }

    }
}