using HelpDesk.Areas.Manager.Models.TroubleModel;
using Model;
using Model.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpDesk.Areas.Manager.Models.DAO
{
    public class TroubleShareDAO
    {

        /*   -1: loi trouble
             -2 -3: loi FAQ
             -4 -5 -6: loi Tech
              > 1: ok
             */
        public int InsertManage(TroubleShareModelView model)
        {
            int kt = 0;
            
            Trouble trouble = new Trouble();
            DetailTech detailTech = new DetailTech();

            
            
            
            //insert DetailTech
            /*
                -4 insert fail
                -5 id not found
                -6 id not tech
                -7 dealine null
             */
            detailTech.id_Trouble = model.id_Trouble;
            
            DetailTechDAO detailTechDAO = new DetailTechDAO();
            UserDAO user = new UserDAO();
            var faq_dao = new FAQ_DAO();
            for (int i = 0; i < model.id_Tech.Count; i++)
            {
                
                detailTech.id_FAQ = 0;
                if (i >= model.dateFinish.Count)
                {
                    if (model.id_FAQ[i] > 0)
                    {
                        var faq = faq_dao.GetFAQByID((int)model.id_FAQ[i]);
                        if (faq != null)
                            detailTech.id_FAQ = model.id_FAQ[i];
                        else return -3;//id not found
                    }
                    var tech = user.getUserById((int)model.id_Tech[i]);
                    if (tech != null)
                    {
                        if (tech.rightt == 1)
                        {
                            detailTech.id_Tech = model.id_Tech[i];
                            detailTech.describe = model.describeTech[i];
                            detailTech.deadline = model.deadline[i];
                            if (detailTech.deadline == null) return -7;
                            if (detailTechDAO.insertManage(detailTech))
                                kt += 1;
                            else return -4;
                        }
                        else return -5;
                    }
                    else return -6;

                }
                else
                {
                    if (model.id_FAQ[i] > 0)
                    {
                        var faq = faq_dao.GetFAQByID((int)model.id_FAQ[i]);
                        if (faq != null)
                            detailTech.id_FAQ = model.id_FAQ[i];
                        else return -3;//id not found
                    }
                    detailTech.id_DetailTech = model.id_DetailTech[i];
                    detailTech.describe = model.describeTech[i];
                    detailTech.id_Tech = model.id_Tech[i];
                    detailTech.deadline = model.deadline[i];
                    if (detailTechDAO.UpdateTech(detailTech) > 0) kt += 1;
                }
            }

            //insert Trouble
            if (model.dateFinish.Count == 0)
            {
                trouble.id_Trouble = model.id_Trouble;
                trouble.id_Manage = model.id_Manage;
                trouble.level = model.level;
                trouble.dateManage = model.dateManage;
                TroubleDAO troubleDAO = new TroubleDAO();
                if (troubleDAO.insertManage(trouble)) kt += 1;
            }

            return kt;
        }

        

        public TroubleShareModelView ConvertTrouble(int id)
        {
            TroubleShareModelView model = new TroubleShareModelView();
            model.describeTech = new List<string>();
            model.describeFAQ = new List<string>();

            var daoTrouble = new TroubleDAO();
            var trouble = daoTrouble.GetTrouble((int)id);

            //var daoDetailFAQ = new DetailFAQ_DAO();
            //var detailFAQ = daoDetailFAQ.getByIdTrouble((int)id);

            var daoDetailTech = new DetailTechDAO();
            var detailTech = daoDetailTech.getListTechByIdTrouble((int)id);

            //trouble
            model.id_Trouble = trouble.id_Trouble;
            model.id_Report = trouble.id_Report;
            model.id_Fill = trouble.id_Fill;
            model.id_Manage = trouble.id_Manage;
            model.id_Device = trouble.id_Device;
            model.status = trouble.status;
            model.level = trouble.level;
            model.images = trouble.images;
            model.describe = trouble.describe;
            model.dateStaff = trouble.dateStaff;
            model.dateManage = trouble.dateManage;
            model.dateTech = trouble.dateTech;
            //detailFAQ
            //detailTech
            if (detailTech != null)
                for (int i = 0; i < detailTech.Count; i++)
                {
                    model.id_DetailTech.Add(detailTech[i].id_DetailTech);
                    model.id_Tech.Add(detailTech[i].id_Tech);
                    model.statusTech.Add(detailTech[i].status);
                    model.describeTech.Add(detailTech[i].describe);
                    model.dateFinish.Add(detailTech[i].dateFinish);
                    model.deadline.Add(detailTech[i].deadline);
                    model.id_FAQ.Add(detailTech[i].id_FAQ);
                    model.describeFAQ.Add(detailTech[i].describeFAQ);
                    
                    if (detailTech[i].finishTime != null)
                        model.finishTime.Add((int)detailTech[i].finishTime);
                }
            return model;
        }
    }
}