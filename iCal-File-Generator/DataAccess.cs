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
        
        public void InsertEvent(string summary, string description, string startTime, string endTime)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                Event newEvent = new Event { summary = summary, description = description, startTime = startTime, endTime = endTime };
                FileGenerator fg= new FileGenerator();
                fg.FormatInput(newEvent);

                using (SqlCommand cmd = new SqlCommand("spEvent_InsertEvent", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@summary", SqlDbType.NVarChar).Value = newEvent.summary;
                    cmd.Parameters.Add("@description", SqlDbType.NVarChar).Value = newEvent.description;
                    cmd.Parameters.Add("@startTime", SqlDbType.DateTime).Value = newEvent.startTime;
                    cmd.Parameters.Add("@endTime", SqlDbType.DateTime).Value = newEvent.endTime;
                    cmd.ExecuteNonQuery();
                }   
            }
        }
    }
}
