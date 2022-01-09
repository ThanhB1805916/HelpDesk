using HelpDesk.Models.UserModel;
using Model;
using Model.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpDesk.Models.DAO
{
    public class FAQ_MV_DAO
    {
        public FAQModelView GetFAQ_MV(int idFAQ)
        {
            var faqMV = new FAQModelView();
            faqMV.describeFAQ = new List<string>();
            var faqDAO = new FAQ_DAO();
            var faq = faqDAO.GetFAQByID(idFAQ);
            var detailDAQ = new DetailTechDAO();
            var detail = detailDAQ.GetDetailFAQ(idFAQ);

            //FAQ
            faqMV.id_FAQ = faq.id_FAQ;
            faqMV.question = faq.question;
            faqMV.answer = faq.answer;
            faqMV.countt = faq.countt;
            faqMV.rightt = faq.rightt;


            //Detail
            for(int i = 0; i < detail.Count; i++)
            {
                faqMV.id_DetailTech.Add(detail[i].id_DetailTech);
                faqMV.id_Trouble.Add(detail[i].id_Trouble);
                faqMV.finishTime.Add((int)detail[i].finishTime);
                faqMV.describeFAQ.Add(detail[i].describeFAQ);
                faqMV.id_Tech.Add(detail[i].id_Tech);
            }

            return faqMV;
        }

        public FAQ ConvertToFAQ(FAQModelView model)
        {
            var faq = new FAQ();
            faq.id_FAQ = model.id_FAQ;
            faq.question = model.question;
            faq.answer = model.answer;
            faq.countt = model.countt;
            faq.rightt = model.rightt;
            return faq;
        }
    }
}