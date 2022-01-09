using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class UserDAO
    {
        private readonly Model db;

        public UserDAO()
        {
            db = new Model();
        }


        /*
           0: username ko ton tai,
           1: hop le,
          -1: sai pwd 
        */
        public int checkLogin(string username, string pwd)
        {
            User user = db.Users.SingleOrDefault(x => x.username == username);
            
            if (user == null) return 0;
            else
            {
                if (user.pwd == pwd) return 1;
                else return -1;
            }
            
        }

        public User getUserByUsername(string username)
        {
            return db.Users.SingleOrDefault(x => x.username == username);
        }

        /*1: thanh cong
         0: that bai
         -1 tai khoan ton tai*/
        public int insert(User user)
        {
            if (user.username != this.getUserByUsername(user.username).username)
            {
                var newUser = db.Users.Add(user);
                db.SaveChanges();
                if (newUser != null) return 1;
                else return 0;
            }
            else return -1;
        }

        /*1: thanh cong
         0: that bai*/
        public int delete(User user)
        {
            var data = db.Users.Remove(user);
            db.SaveChanges();
            if (data != null) return 1;
            else return 0;
        }

        /*1: thanh cong
         0: that bai
        -1: sai username*/
        public int update(User newUser)
        {
            var user = this.getUserByUsername(newUser.username);
            if (user == null) return -1;
            else
            {
                if (newUser.name == null) return 0;
                user.name = newUser.name;
                user.rightt = newUser.rightt;
                int kt = db.SaveChanges();
                if(kt!=0) return 1;
                else return 0;
            }
            
        }


        /*1: thanh cong
         0: sai username*/
        public int changePassword(string username, string newPwd)
        {
            var user = this.getUserByUsername(username);
            if (user == null) return 0;
            else
            {
                user.pwd = newPwd;
                db.SaveChanges();
                return 1;
            }
            
        }


        //Thanh Add
        //Get user by id
        public User getUserById(int id)
        {
            return db.Users.Find(id);
        }
        //List user by id 
        //if id == null list all
        public List<User> ListUser(int ?id)
        {
            if(id == null)
            {
                return db.Users.ToList();
            }

            return db.Users.Where(x => x.rightt == (int)id).ToList();
        }

        public List<User> ListAllUser()
        {
            return db.Users.ToList();
        }

        public List<User> ListByRight(int right)
        {
            return db.Users.Where(x => x.rightt == right).ToList();
        }

        //Thêm môt người dùng mới vào CSDL
        public bool AddUser(User user)
        {  
            if(getUserByUsername(user.username) == null)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return true;
            }
            return false;
        }

        //Xóa một người dùng ra khỏi CSDL
        public bool DeleteUser(User user)
        {
            try
            {
                db.Users.Remove(user);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        //Cập nhật thông tin của người dùng
        public bool UpdateUserInfo(User user)
        {
            try
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        
    }
}
