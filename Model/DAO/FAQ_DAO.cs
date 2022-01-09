using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class FAQ_DAO
    {
        private Model db;

        public FAQ_DAO()
        {
            db = new Model();
        }

 //Getter Setter

        //Hàm lấy FAQ theo id
        public FAQ GetFAQByID(int id)
        {
            return db.FAQs.Find(id);
        }

        //Hàm trả về danh sách FAQ
        public List<FAQ> GetListFAQ(int right)
        {
            return db.FAQs.Where(x => x.rightt == right).ToList();
        }

        //Hàm thêm FAQ vào danh sách trả về false nếu không thêm được
        public bool Insert_FAQ(FAQ FAQ)
        {
            if(db.FAQs.Find(FAQ.id_FAQ) == null)
            {
                FAQ.countt = 0;
                db.FAQs.Add(FAQ);
                db.SaveChanges();
                return true;
            }

            return false;
        }

        public List<FAQ> GetLisAll()
        {
            return db.FAQs.Where(x => x.rightt > -1).ToList();
        }

        public List<FAQ> GetListPublic()
        {
            return db.FAQs.Where(x => x.rightt == 1).ToList();
        }

        public List<FAQ> GetListSuggestion()
        {
            return db.FAQs.Where(x => x.rightt == -1).ToList();
        }

        public void IncCount(int idFAQ)
        {
            try
            {
                var faq = db.FAQs.SingleOrDefault(x => x.id_FAQ == idFAQ);
                faq.countt++;
                db.SaveChanges();
            }
            catch(Exception)
            {
                
            }
        }

        public bool UpdateFAQ(FAQ model)
        {
            var faq = db.FAQs.Find(model.id_FAQ);
            if(faq != null)
            {
                faq.question = model.question;
                faq.answer = model.answer;
                faq.rightt = model.rightt;
                db.SaveChanges();
                return true;
            }
            return false;
        }

        public bool ChangeRight(int idFAQ)
        {
            var faq = db.FAQs.Find(idFAQ);
            if (faq != null)
            {
                faq.rightt = faq.rightt == 0 ? 1 : 0;
                db.SaveChanges();
                return true;
            }
            else return false;
        }

        public bool Delete(int idFAQ)
        {
            var faq = db.FAQs.Find(idFAQ);
            if(faq != null)
            {
                db.FAQs.Remove(faq);
                db.SaveChanges();
                return true;
            }
            return false;
        }

        public bool SuggestionFAQ(FAQ faq)
        {
            //if (db.FAQs.Find(faq.id_FAQ) == null)
            //{
            //    faq.countt = 0;
            //    faq.rightt = -1;
            //    db.FAQs.Add(faq);
            //    db.SaveChanges();
            //    return true;
            //}

            //return false;
            string sql = "insert into FAQ (question, answer, rightt, id_User) values ( @question, @answer, @rightt, @idUser )";
            faq.countt = 0;
            faq.rightt = -1;
            object[] para =
            {
                new SqlParameter("@question", faq.question),
                new SqlParameter("@answer", faq.answer),
                new SqlParameter("@rightt",faq.rightt),
                new SqlParameter("@idUser",faq.id_User),
            };
            int kt = db.Database.ExecuteSqlCommand(sql, para);
            if (kt > 0) return true;
            return false;
        }

        public bool AcceptFAQ(FAQ model)
        {
            var faq = db.FAQs.Find(model.id_FAQ);
            if ( faq != null)
            {
                faq.rightt = model.rightt;
                db.SaveChanges();
                return true;
            }
            return false;
        }

        public bool RejectFAQ(int id)
        {
            var faq = db.FAQs.Find(id);
            if(faq != null)
            {
                db.FAQs.Remove(faq);
                db.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
