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

        List<string> records;
        public void InsertEvent(string summary, string description, string startTime, string endTime, string dtstamp, string uid, TimeZoneInfo timezone, string classification)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                Event newEvent = new Event { summary = summary, description = description, startTime = startTime, endTime = endTime, dtstamp = dtstamp, uniqueIdentifier = uid, timeZone = timezone, classification = classification };
                FileGenerator fg = new FileGenerator();
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
                    cmd.Parameters.Add("@timezone", SqlDbType.NVarChar).Value = newEvent.timeZone.ToString();
                    cmd.Parameters.Add("@classification", SqlDbType.NVarChar).Value = newEvent.classification;
                    cmd.ExecuteNonQuery();
                }   
            }
        }

        public List<string> ListEvents()
        {
            List<string> data = new List<string>();
            records = new List<string>();
            
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                using(SqlCommand cmd = new SqlCommand("spEvent_SelectEvent", conn))
                {
                    SqlDataReader dataReader = cmd.ExecuteReader();
                    while(dataReader.Read())
                    {
                        data.Add(ReadSingleRow((IDataRecord)dataReader, records));
                    }
                    dataReader.Close();
                }
            }
            return data;
        }

        private string ReadSingleRow(IDataRecord dataReader, List<string> records)
        {
            string title = dataReader["summary"].ToString().Trim();
            string description = dataReader["description"].ToString().Trim();
            string startTime = dataReader["startTime"].ToString().Trim();
            string endTime = dataReader["endTime"].ToString().Trim();
            string timezone = dataReader["timezone"].ToString().Trim();
            string classification = dataReader["classification"].ToString().Trim();
            string dtstamp = dataReader["dtstamp"].ToString().Trim();
            string newLine = Environment.NewLine;
            string formatedStr = "Title: " + TrimString(title, 16) + newLine + "Description: " + TrimString(description, 20) + newLine + "Created: " + dtstamp;
            string expandedRowStr = "Title: " + title + newLine 
                                  + "Description: " + description + newLine 
                                  + "Start time: " + startTime + newLine 
                                  + "End time: " + endTime + newLine
                                  + "Timezone: " + timezone + newLine
                                  + "Classification: " + classification + newLine
                                  + "Created: " + dtstamp;
            records.Add(expandedRowStr);
            return formatedStr;
        }

        public List<string> GetEvents()
        {
            return records;
        }

        private string TrimString(string str, int maxChars)
        {
            return str.Length <= maxChars ? str : str.Substring(0, maxChars) + "...";
        }
    }
}
