using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using Model;
using HelpDesk.Hubs;

namespace HelpDesk.Models
{
    public class TroubleRes
    {

        private static TroubleRes instance;
        private TroubleRes() { }

        public static TroubleRes Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TroubleRes();
                }
                return instance;
            }
        }


        SqlDependency dependency;
        public List<Trouble> GetTrouble()
        {
            var listTrouble = new List<Trouble>();
            
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["HelpDesk"].ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(@"SELECT [id_Trouble],[id_Report],[id_Fill],[id_Tech],[id_Manage],[id_Device],[id_FAQ],[describe],[images],[status],[level],[dateStaff],[dateManage],[dateTech] FROM [HelpDesk].[dbo].[Trouble]", connection))
                {
                    // Make sure the command object does not already have
                    // a notification object associated with it.
                    command.Notification = null;

                    //SqlDependency
                    dependency = new SqlDependency(command);
                    dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);

                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        var trouble = new Trouble();
                        trouble.id_Trouble = (int)reader["id_Trouble"];
                        trouble.id_Fill = (int)reader["id_Fill"];
                        trouble.id_Report = (int)reader["id_Report"];
                        trouble.id_Manage = (int)reader["id_Manage"];
                        //trouble.id_Tech = (int)reader["id_Tech"];
                        trouble.id_Device = (int)reader["id_Device"];
                        //trouble.id_FAQ = (int)reader["id_FAQ"];
                        trouble.status = (int)reader["status"];
                        trouble.level = (int)reader["level"];
                        trouble.describe = (string)reader["describe"];
                        trouble.images = (string)reader["images"];
                        if (reader["dateStaff"] != DBNull.Value)
                            trouble.dateStaff = Convert.ToDateTime(reader["dateStaff"]);
                        if (reader["dateManage"] != DBNull.Value)
                            trouble.dateManage = Convert.ToDateTime(reader["dateManage"]);
                        if (reader["dateTech"] != DBNull.Value)
                            trouble.dateTech = Convert.ToDateTime(reader["dateTech"]);

                        listTrouble.Add(trouble);
                    }


                }
            }
            return listTrouble;
        }


        private void dependency_OnChange(object sender, SqlNotificationEventArgs e)
        {
            //if (dependency != null)
            //{
            //    dependency.OnChange -= dependency_OnChange;
            //    dependency = null;
            //}
            //if (e.Type == SqlNotificationType.Change)
            //{
            //    TroubleHub.Show();
            //}
            TroubleHub.SentTrouble();
        }
    }
}