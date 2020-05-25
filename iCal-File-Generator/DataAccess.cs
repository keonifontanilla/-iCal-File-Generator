using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace iCal_File_Generator
{
    public class DataAccess
    {
        static string connStr = ConfigurationManager.ConnectionStrings["EventsDB"].ConnectionString;
        
        public void InsertEvent(string summary, string description, string startTime, string endTime, string dtstamp)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                Event newEvent = new Event { summary = summary, description = description, startTime = startTime, endTime = endTime, dtstamp = dtstamp };
                FileGenerator fg= new FileGenerator();
                fg.FormatInput(newEvent);

                using (SqlCommand cmd = new SqlCommand("spEvent_InsertEvent", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@summary", SqlDbType.NVarChar).Value = newEvent.summary;
                    cmd.Parameters.Add("@description", SqlDbType.NVarChar).Value = newEvent.description;
                    cmd.Parameters.Add("@startTime", SqlDbType.DateTime).Value = newEvent.startTime;
                    cmd.Parameters.Add("@endTime", SqlDbType.DateTime).Value = newEvent.endTime;
                    cmd.Parameters.Add("@dtstamp", SqlDbType.DateTime).Value = newEvent.dtstamp;
                    cmd.ExecuteNonQuery();
                }   
            }
        }

        public List<string> ListEvents()
        {
            List<string> data = new List<string>();
            

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                using(SqlCommand cmd = new SqlCommand("spEvent_SelectEvent", conn))
                {
                    SqlDataReader dataReader = cmd.ExecuteReader();
                    while(dataReader.Read())
                    {
                        data.Add(ReadSingleRow((IDataRecord)dataReader));
                    }
                }
            }
            return data;
        }

        private string ReadSingleRow(IDataRecord dataReader)
        {
            string title = dataReader["summary"].ToString();
            string description = dataReader["description"].ToString();
            //string formatedStr = $@"{TrimString(title, 6)} {TrimString(description, 10)}";
            //string formatedStr = String.Format("{0, 0}{1, 20}", TrimString(title, 6) + "\n", TrimString(description, 10)); //turned on multi column for listBox
            string formatedStr = title + "\n" + description;
            return formatedStr;
        }

        private string TrimString(string str, int maxChars)
        {
            return str.Length <= maxChars ? str : str.Substring(0, maxChars) + "...";
        }
    }
}
