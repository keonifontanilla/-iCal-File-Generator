using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iCal_File_Generator
{
    public class DataAccess
    {
        static string connStr = ConfigurationManager.ConnectionStrings["EventsDB"].ConnectionString;
        
        public void InsertEvent(string summary, string description)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string sql = "INSERT INTO event (summary, description) VALUES (@summary, @description)";
                Event newEvent = new Event { summary = summary, description = description };

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add("@summary", SqlDbType.NVarChar).Value = newEvent.summary;
                cmd.Parameters.Add("@description", SqlDbType.NVarChar).Value = newEvent.description;
                cmd.ExecuteNonQuery();
            }
        }
    }
}
