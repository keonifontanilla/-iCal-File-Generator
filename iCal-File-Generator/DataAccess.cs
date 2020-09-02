using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace iCal_File_Generator
{
    public class DataAccess
    {
        static string connStr = ConfigurationManager.ConnectionStrings["EventsDB"].ConnectionString;

        List<Event> records;

        public void InsertEvent(string summary, string description, string startTime, string endTime, string dtstamp, string uid, TimeZoneInfo timezone, string classification, string organizer, List<string> attendees)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string timeZoneStandardName = timezone.StandardName;
                Event newEvent = new Event
                {
                    summary = summary, description = description, startTime = startTime, endTime = endTime,
                    dtstamp = dtstamp, uniqueIdentifier = uid, timeZone = timezone.DisplayName,
                    timeZoneStandardName = timeZoneStandardName, classification = classification,
                    organizer = organizer, attendees = attendees 
                };
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
                    cmd.Parameters.Add("@organizer", SqlDbType.NVarChar).Value = newEvent.organizer;
                    cmd.ExecuteNonQuery();

                    // Inserting multiple recoreds of attendees to the same eventID in attendees table
                    using (SqlCommand cmd2 = new SqlCommand("spAttendees_Insert", conn))
                    {
                        cmd2.CommandType = CommandType.StoredProcedure;

                        cmd2.Parameters.Add("@email", SqlDbType.NVarChar);
                        foreach (string attendee in attendees)
                        {
                            cmd2.Parameters["@email"].Value = attendee;
                            cmd2.ExecuteNonQuery();
                        }
                    }
                }   
            }
        }

        public void UpdateEvent(string summary, string description, string startTime, string endTime, TimeZoneInfo timezone, string classification, int eventID)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                
                using (SqlCommand cmd = new SqlCommand("spEvent_UpdateEvent", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@eventID", SqlDbType.NVarChar).Value = eventID;
                    cmd.Parameters.Add("@summary", SqlDbType.NVarChar).Value = summary;
                    cmd.Parameters.Add("@description", SqlDbType.NVarChar).Value = description;
                    cmd.Parameters.Add("@startTime", SqlDbType.DateTime).Value = startTime;
                    cmd.Parameters.Add("@endTime", SqlDbType.DateTime).Value = endTime;
                    cmd.Parameters.Add("@timezone", SqlDbType.NVarChar).Value = timezone.ToString();
                    cmd.Parameters.Add("@classification", SqlDbType.NVarChar).Value = classification;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteEvent(int eventID)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand("spEvent_DeleteEvent", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@eventID", SqlDbType.NVarChar).Value = eventID;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<string> ListEvents()
        {
            List<string> data = new List<string>();
            records = new List<Event>();
            
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

        private string ReadSingleRow(IDataRecord dataReader, List<Event> events)
        {
            int eventID = (int) dataReader["eventID"];
            string title = dataReader["summary"].ToString().Trim();
            string description = dataReader["description"].ToString().Trim();
            string startTime = dataReader["startTime"].ToString().Trim();
            string endTime = dataReader["endTime"].ToString().Trim();
            string timezone = dataReader["timezone"].ToString().Trim();
            string classification = dataReader["classification"].ToString().Trim();
            string dtstamp = dataReader["dtstamp"].ToString().Trim();
            string uniqueID = dataReader["uniqueIdentifier"].ToString();
            string newLine = Environment.NewLine;
            string formatedStr = "Title: " + TrimString(title, 16) + newLine + "Description: " + TrimString(description, 20) + newLine + "Created: " + dtstamp + newLine;

            Event oneEvent = new Event { eventID = eventID, summary = title, description = description, 
                                         startTime = startTime, endTime = endTime, dtstamp = dtstamp, uniqueIdentifier = uniqueID, timeZone = timezone, classification = classification };
            events.Add(oneEvent);

            return formatedStr;
        }

        public List<Event> GetEvents()
        {
            return records;
        }
      
        private string TrimString(string str, int maxChars)
        {
            return str.Length <= maxChars ? str : str.Substring(0, maxChars) + "...";
        }
    }
}
