using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace iCal_File_Generator
{
    public class DataAccess
    {
        static string connStr = ConfigurationManager.ConnectionStrings["EventsDB"].ConnectionString;
        
        public void InsertEvent(string summary, string description, string startTime, string endTime, string dtstamp, string uid, TimeZoneInfo timezone, string classification)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                Event newEvent = new Event { summary = summary, description = description, startTime = startTime, endTime = endTime, dtstamp = dtstamp, uniqueIdentifier = uid, timezone = timezone, classification = classification };
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
                    cmd.Parameters.Add("@uniqueIdentifier", SqlDbType.NVarChar).Value = newEvent.uniqueIdentifier;
                    cmd.Parameters.Add("@timezone", SqlDbType.NVarChar).Value = newEvent.timezone.ToString();
                    cmd.Parameters.Add("@classification", SqlDbType.NVarChar).Value = newEvent.classification;
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
                    dataReader.Close();
                }
            }
            return data;
        }

        private string ReadSingleRow(IDataRecord dataReader)
        {
            string title = dataReader["summary"].ToString().Trim();
            string description = dataReader["description"].ToString().Trim();
            string dtstamp = dataReader["dtstamp"].ToString().Trim();
            string formatedStr = "Title: " + TrimString(title, 16) + "\n" + "Description: " + TrimString(description, 20) + "\n" + "Created: " + dtstamp;
            return formatedStr;
        }

        private string TrimString(string str, int maxChars)
        {
            return str.Length <= maxChars ? str : str.Substring(0, maxChars) + "...";
        }
    }
}
