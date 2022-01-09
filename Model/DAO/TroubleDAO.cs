using Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class TroubleDAO
    {
        private Model db;

        public TroubleDAO()
        {
            db = new Model();
        }


        /*true: insert thanh cong
         false: insert that bai*/
        //sent
        public bool insertStaff(Trouble trouble)
        {
            string sql = "insert into Trouble (id_Report, id_Fill, id_Device, describe, images, status) values ( @idReport, @idFill , @idDevice , @describe , @images, @status)";
            if (trouble.describe == null) trouble.describe = "null";
            if (trouble.images == null) trouble.images = "null";
            trouble.status = 0;
            object[] para =
            {
                new SqlParameter("@idReport", trouble.id_Report),
                new SqlParameter("@idFill", trouble.id_Fill),
                new SqlParameter("@idDevice",trouble.id_Device),
                new SqlParameter("@describe", trouble.describe),
                new SqlParameter("@images",trouble.images),
                new SqlParameter("@status", trouble.status),

            };
            int kt = db.Database.ExecuteSqlCommand(sql, para);
            if (kt > 0) return true;
            return false;
        }

        //recvevied
        public bool insertManage(Trouble model)
        {
            // idManage, idTech, level, status, dateManage
            Trouble trouble = db.Troubles.SingleOrDefault(x => x.id_Trouble == model.id_Trouble);

            trouble.id_Manage = model.id_Manage;
            //trouble.id_Tech = model.id_Tech;
            trouble.level = model.level;
            trouble.status = 1;
            trouble.dateManage = model.dateManage;
            //trouble.id_FAQ = model.id_FAQ;

            int kt = db.SaveChanges();
            if (kt > 0) return true;
            return false;
        }

        //processing
        public bool ProcessingTrouble(int idTrouble)
        {
            Trouble trouble = db.Troubles.SingleOrDefault(x => x.id_Trouble == idTrouble);
            if (trouble != null)
            {
                trouble.status = 2;
                db.Entry(trouble).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            return false;
        }

        //finished
        public bool FinishTrouble(int idTrouble)
        {
            // dateTech
            Trouble trouble = db.Troubles.SingleOrDefault(x => x.id_Trouble == idTrouble);
            trouble.dateTech = DateTime.Now;
            trouble.status = 3;
            int kt = db.SaveChanges();
            if (kt > 0) return true;
            return false;
        }

        public bool FinishTrobuleViewed(int idTrouble)
        {
            Trouble trouble = db.Troubles.SingleOrDefault(x => x.id_Trouble == idTrouble);
            trouble.dateTech = DateTime.Now;
            trouble.status = 4;
            int kt = db.SaveChanges();
            if (kt > 0) return true;
            return false;
        }

        public List<Trouble> ListAll()
        {
            string sql = "select * from Trouble";
            var list = db.Database.SqlQuery<Trouble>(sql).ToList();
            return list;
        }

        public List<Trouble> ListByIdTech(int id)
        {
            var list = (from t in db.Troubles
                        join d in db.DetailTeches
                        on t.id_Trouble equals d.id_Trouble
                        where d.id_Tech == id
                        select t
                        ).ToList();

            return list;
        }

        public List<Trouble> ListByIdReport(int id)
        {
            return db.Troubles.Where(x => x.id_Report == id).ToList();
        }

        public List<Trouble> ListByIdDevice(int id)
        {
            return db.Troubles.Where(x => x.id_Device == id).ToList();
        }

        public List<Trouble> ListByIDFill(int id)
        {
            return db.Troubles.Where(x => x.id_Fill == id).ToList();
        }

        public Trouble GetTrouble(int idTrouble)
        {
            try
            {
                var trouble = db.Troubles.SingleOrDefault(x => x.id_Trouble == idTrouble);
                return trouble;
            }
            catch (Exception)
            {
                return null;
            }
            
        }

        public Trouble GetLastTrouble()
        {
            return db.Troubles.OrderByDescending(x => x.id_Trouble).First();
        }

        public bool UpdateStatus(int id)
        {

            Trouble trouble = db.Troubles.SingleOrDefault(x => x.id_Trouble == id);
            if (trouble != null)
            {
                trouble.status = 2;
                db.Entry(trouble).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }

            return false;
        }

        public bool SendBack(int idTrouble)
        {
            Trouble trouble = db.Troubles.SingleOrDefault(x => x.id_Trouble == idTrouble);
            if (trouble != null)
            {
                trouble.status = 1;
                //trouble.id_Tech = -1;
                db.Entry(trouble).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            return false;
        }

        public List<Trouble> ListByStatus(int status)
        {
            string sql = "select * from Trouble where status = @status";
            object[] para =
            {
                new SqlParameter("@status", status)
            };
            var list = db.Database.SqlQuery<Trouble>(sql, para).ToList();
            return list;
        }

        //Hàm trả về một danh sách, là danh sách truyền vào đã được sắp xếp theo chuỗi sortOrder 

        public List<Trouble> Ordered_Troubles(List<Trouble> troubles, string sortOrder)
        {
            // Trình tự sắp xếp
            switch (sortOrder)
            {
                //Sắp xếp theo ID
                //Tăng dần
                case "id":
                    troubles = troubles.OrderBy(x => x.id_Trouble).ToList();
                    break;
                //Giảm dần
                case "id_desc":
                    troubles = troubles.OrderByDescending(x => x.id_Trouble).ToList();
                    break;

                //Sắp xếp theo level
                //Tăng dần
                case "level":
                    troubles = troubles.OrderBy(x => x.level).ToList();
                    break;
                //Giảm dần
                case "level_desc":
                    troubles = troubles.OrderByDescending(x => x.level).ToList();
                    break;

                //Sắp xếp theo thời gian đến trễ nhất -> sớm nhất
                case "date_desc":
                    troubles = troubles.OrderByDescending(t => t.dateStaff).ToList();
                    break;
                case "default_staff":
                    var temp1 = troubles.Where(x => x.status == 3).OrderBy(x => x.status).ThenByDescending(x => x.id_Trouble).ToList();
                    var temp2 = troubles.Where(x => x.status != 3).OrderBy(x => x.status).ThenByDescending(x => x.id_Trouble).ToList();
                    temp1.AddRange(temp2);
                    troubles = temp1;
                    break;
                //Mặc định sớm nhất -> trễ nhất
                default:
                    troubles = troubles.OrderBy(x => x.status).ThenByDescending(x => x.id_Trouble).ToList();
                    break;
            }

            return troubles;
        }

        //Hàm trả về một danh sách, là danh sách truyền vào đã được lọc chuỗi searchString 
        public List<Trouble> Search_Troubles(List<Trouble> troubles, string searchString)
        {
            try
            {
                if (searchString != null)
                {
                    //Xử lý chuỗi tìm kiếm định dạng dd-mm-yyyy_level
                    //Tách level ra khỏi date
                    string[] search = searchString.Split('_');

                    //Tìm kiếm

                    //Tìm kiếm theo ngày
                    //Kiếm tra nếu định dạng ngày không rỗng hoặc null
                    if (!String.IsNullOrEmpty(search[0])&& !String.IsNullOrEmpty(search[1]))
                    {
                        
                        string[] searchDate = search[0].Split('-');
                        troubles = troubles.Where(x => x.dateStaff >= DateTime.Parse(search[0]) && x.dateStaff <= DateTime.Parse(search[1])).ToList();
                        
                    }

                    //Tìm kiếm theo level
                    if (!String.IsNullOrEmpty(search[2]))
                    {
                        troubles = troubles.Where(x => x.level == int.Parse(search[2])).ToList();
                    }
                }
                return troubles;
            }
            //Ngoại lệ người dùng nhập kí tự vào ô tìm kiếm ngày trả về danh sách không lọc
            catch
            {
                return troubles;
            }
        }

        //Update trouble
        public bool EditTrouble(Trouble entity)
        {
            try
            {
                string query = "update Trouble set id_Report = @ReportID, id_Device = @DeviceID" +
                    ", describe = @Describe, images = @Images, dateStaff = @Datestaff where id_Trouble = @TroubleID";
                var trouble = db.Troubles.Find(entity.id_Trouble);
                if(trouble != null)
                {
                    trouble.id_Report = entity.id_Report;
                    trouble.id_Device = entity.id_Device;
                    trouble.describe = entity.describe;
                    trouble.images = entity.images;
                    trouble.dateStaff = DateTime.Now;
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteTrouble(int id)
        {
            try
            {
                var trouble = db.Troubles.Find(id);
                db.Troubles.Remove(trouble);
                return db.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Delete(int idTrouble)
        {
            var trouble = db.Troubles.Find(idTrouble);
            if (trouble != null && trouble.status == 0) 
            {
                db.Troubles.Remove(trouble);
                db.SaveChanges();
                return true;
            }
                
            return false;
        }
    }
}
