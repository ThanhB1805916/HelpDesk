using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class DetailTechDAO
    {
        private Model db;

        public DetailTechDAO()
        {
            db = new Model();
        }

        public bool insertManage(DetailTech model)
        {
            try
            {
                string sql = "insert into DetailTech (id_Trouble, id_Tech, status, describe, id_FAQ, deadline) values ( @idTrouble , @idTech , @status, @describe , @idFAQ, @deadline )";
                //if (model.describe == null) trouble.describe = "null";
                model.status = 0;
                object[] para =
                {
                new SqlParameter("@idTrouble", model.id_Trouble),
                new SqlParameter("@idTech", model.id_Tech),
                new SqlParameter("@status", model.status),
                new SqlParameter("@describe", model.describe),
                new SqlParameter("@idFAQ", model.id_FAQ), 
                new SqlParameter("@deadline", model.deadline)
                };
                int kt = db.Database.ExecuteSqlCommand(sql, para);
                if (kt > 0) return true;
                return false;
            }
            catch (Exception)
            {
                return false;
            }


        }

        public List<DetailTech> getListTechByIdTrouble(int idTrouble)
        {
            var list = db.DetailTeches.Where(x => x.id_Trouble == idTrouble).ToList();
            return list;
        }

        public List<int> getListIdTrouble(int idTech)
        {
            return db.DetailTeches.Where(x => x.id_Tech == idTech).Select(x => x.id_Trouble).ToList();
        }

        // -1 : false
        //  0 : finishTech true
        //  1 : finishTrouble true
        public int FinishTech(DetailTech model)
        {
            var detailTech = db.DetailTeches.SingleOrDefault(x => x.id_DetailTech == model.id_DetailTech);
            detailTech.dateFinish = DateTime.Now;
            detailTech.status = 3;
            detailTech.contentBill = model.contentBill;
            detailTech.id_Bill = model.id_Bill;
            detailTech.fixMethod = model.fixMethod;
            if (model.id_FAQ > 0)
            {
                new FAQ_DAO().IncCount(model.id_FAQ);
                detailTech.id_FAQ = model.id_FAQ;
                detailTech.describeFAQ = model.describeFAQ;
                detailTech.finishTime = model.finishTime;
            }
            int kt = db.SaveChanges();
            var fini = FinishTrouble(model.id_Trouble);
            if (kt > 0)
            {
                if (fini) return 1;
                return 0; ;
            }
            return -1;
        }

        public bool FinishTrouble(int idTrouble)
        {
            int kt = 0;
            var troubleLi = db.DetailTeches.Where(x => x.id_Trouble == idTrouble).ToList();
            foreach (var item in troubleLi)
            {
                if (item.status < 3)
                {
                    kt = -1;
                    return false;
                }
            }
            if (kt == 0)
            {
                var trouble = new TroubleDAO();
                trouble.FinishTrouble(idTrouble);
                return true;
            }
            return false;
        }


        //bug
        public bool TroubleShooting(int idTrouble, int idTech)
        {
            var troubleDAO = new TroubleDAO();
            var trouble = troubleDAO.GetTrouble(idTrouble);
            if (trouble.status == 1)
            {
                troubleDAO.ProcessingTrouble(idTrouble);
            }

            var detailTech = db.DetailTeches.SingleOrDefault(x => x.id_Tech == idTech && x.id_Trouble == idTrouble);
            detailTech.status = 1;
            var kt = db.SaveChanges();
            if (kt > 0) return true;
            return false;
        }

        public bool TroubleShooting(int idDetailTech)
        {
            int idTrouble = GetIdTrouble(idDetailTech);
            //int idTech = GetIdTech(idDetailTech);
            var troubleDAO = new TroubleDAO();
            var trouble = troubleDAO.GetTrouble(idTrouble);
            if (trouble.status == 1)
            {
                troubleDAO.ProcessingTrouble(idTrouble);
            }

            var detailTech = db.DetailTeches.SingleOrDefault(x => x.id_DetailTech == idDetailTech);
            detailTech.status = 1;
            var kt = db.SaveChanges();
            if (kt > 0) return true;
            return false;
        }

        //bug
        public bool SendBack(int idTrouble, int idTech)
        {
            var detailTech = db.DetailTeches.SingleOrDefault(x => x.id_Tech == idTech && x.id_Trouble == idTrouble);
            detailTech.status = 2;
            var kt = db.SaveChanges();
            if (kt > 0) return true;
            return false;
        }

        public bool SendBack(int idDetailTech)
        {
            var detailTech = db.DetailTeches.SingleOrDefault(x => x.id_DetailTech == idDetailTech);
            detailTech.status = 2;
            var kt = db.SaveChanges();
            if (kt > 0) return true;
            return false;
        }

        public int GetIdDetailTech(int idTrouble, int idTech)
        {
            return db.DetailTeches.SingleOrDefault(x => x.id_Trouble == idTrouble && x.id_Tech == idTech).id_DetailTech;
        }

        public int GetIdTrouble(int idDetailTech)
        {
            return db.DetailTeches.SingleOrDefault(x => x.id_DetailTech == idDetailTech).id_Trouble;
        }

        public int GetIdTech(int idDetailTech)
        {
            return db.DetailTeches.SingleOrDefault(x => x.id_DetailTech == idDetailTech).id_Tech;
        }

        public int CountTask(int idTrouble)
        {
            var list = db.DetailTeches.Where(x => x.id_Trouble == idTrouble).ToList();
            return list.Count;
        }

        public int CountTaskFinish(int idTrouble)
        {
            var list = db.DetailTeches.Where(x => x.id_Trouble == idTrouble).ToList();
            var count = 0;
            foreach( var item in list)
            {
                if(item.status == 3)
                {
                    count++;
                }
            }
            return count;
        }

        public DetailTech GetDetailTech(int idDetailTech)
        {
            return db.DetailTeches.SingleOrDefault(x => x.id_DetailTech == idDetailTech);
        }

        public DetailTech GetDetailTech(int idTrouble, int idTech)
        {
            return db.DetailTeches.SingleOrDefault(x => x.id_Trouble == idTrouble && x.id_Tech == idTech);
        }

        public List<DetailTech> GetDetailFAQ(int idFAQ)
        {
            return db.DetailTeches.Where(x => x.id_FAQ == idFAQ && x.finishTime != null).ToList();
        }

        public List<DetailTech> ListByIdTech(int idTech)
        {
            var list = (from t in db.Troubles
                        join d in db.DetailTeches
                        on t.id_Trouble equals d.id_Trouble
                        where d.id_Tech == idTech
                        select d
                        ).ToList();

            return list;
        }

        public int UpdateTech(DetailTech model)
        {
            try
            {
                var detailTech = db.DetailTeches.SingleOrDefault(x => x.id_DetailTech == model.id_DetailTech);
                if (detailTech != null)
                {
                    detailTech.describe = model.describe;
                    detailTech.deadline = model.deadline;
                    detailTech.id_FAQ = model.id_FAQ;
                    int kt = db.SaveChanges();
                    return kt;
                }
                else return -1;
                
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public bool Delete(int idDetailTech)
        {
            try
            {
                var dete = db.DetailTeches.Find(idDetailTech);
                db.DetailTeches.Remove(dete);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ReSend(int idDetailTech)
        {
            try
            {
                var dete = db.DetailTeches.Find(idDetailTech);
                dete.status = 0;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool RemoveFAQ(int idDetailTech)
        {
            var detail = db.DetailTeches.Find(idDetailTech);
            if(detail != null)
            {
                detail.id_FAQ = -1;
                detail.describeFAQ = "";
                detail.finishTime = -1;
                db.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
