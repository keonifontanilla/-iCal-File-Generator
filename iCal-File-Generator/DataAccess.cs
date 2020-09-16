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
        List<string> attendees, attendeesRsvp, formatedRecords;
        List<int> attendeesId;
        Event newEvent = new Event();

        public void InsertEvent(string summary, string description, string startTime, string endTime, string dtstamp, string uid, TimeZoneInfo timezone, string classification, string organizer, List<string> attendees, List<string> attendeesRsvp, string recurFrequency, string recurUntil)
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
                    organizer = organizer, attendees = attendees, attendeesRsvp = attendeesRsvp,
                    recurFrequency = recurFrequency, recurUntil = recurUntil
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
                    if (newEvent.organizer != "") { cmd.Parameters.Add("@organizer", SqlDbType.NVarChar).Value = newEvent.organizer; }
                    if (newEvent.recurFrequency != "") { cmd.Parameters.Add("@recurFrequency", SqlDbType.NVarChar).Value = newEvent.recurFrequency; }
                    if (newEvent.recurUntil != "" && newEvent.recurFrequency != "Once") { cmd.Parameters.Add("@recurDateTime", SqlDbType.DateTime).Value = newEvent.recurUntil; }
                    cmd.ExecuteNonQuery();

                    // Inserting multiple records of attendees to the same eventID in attendees table
                    using (SqlCommand cmd2 = new SqlCommand("spAttendees_Insert", conn))
                    {
                        int counter = 0;
                        cmd2.CommandType = CommandType.StoredProcedure;

                        cmd2.Parameters.Add("@email", SqlDbType.NVarChar);
                        cmd2.Parameters.Add("@rsvp", SqlDbType.NVarChar);
                        foreach (string attendee in attendees)
                        {
                            if (attendee != "")
                            {
                                cmd2.Parameters["@email"].Value = attendee;
                            }
                            cmd2.Parameters["@rsvp"].Value = attendeesRsvp[counter];
                            counter++;
                            cmd2.ExecuteNonQuery();
                        }
                    }
                }   
            }
        }

        public void UpdateEvent(string summary, string description, string startTime, string endTime, TimeZoneInfo timezone, string classification, string organizer, int eventID, List<string> attendees, List<string> attendeesRsvp, List<int> attendeesId, string recurFrequency, string recurUntil)
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
                    if (organizer != "") { cmd.Parameters.Add("@organizer", SqlDbType.NVarChar).Value = organizer; }
                    if (recurUntil != "") { cmd.Parameters.Add("@recurDateTime", SqlDbType.DateTime).Value = recurUntil; }
                    cmd.Parameters.Add("@recurFrequency", SqlDbType.NVarChar).Value = recurFrequency;
                                       
                    cmd.Parameters.Add("@attendeeID", SqlDbType.Int);
                    cmd.Parameters.Add("@email", SqlDbType.NVarChar);
                    cmd.Parameters.Add("@rsvp", SqlDbType.NVarChar);
                    for(int i = 0; i < attendees.Count; i++)
                    {
                        if (attendeesId == null)
                        {
                            attendeesId = new List<int>();
                        }
                        attendeesId.Add(0);

                        cmd.Parameters["@attendeeID"].Value = attendeesId[i];
                        if (attendees[i] != "")
                        {
                            cmd.Parameters["@email"].Value = attendees[i];
                        }
                        cmd.Parameters["@rsvp"].Value = attendeesRsvp[i];

                        cmd.ExecuteNonQuery();
                    }

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
            formatedRecords = new List<string>();
            records = new List<Event>();
            
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                using(SqlCommand cmd = new SqlCommand("spEvent_SelectEvent", conn))
                {
                    SqlDataReader dataReader = cmd.ExecuteReader();
                    while(dataReader.Read())
                    {
                        ReadSingleRow((IDataRecord) dataReader, records);
                    }
                    dataReader.Close();
                }
            }
            return formatedRecords;
        }

        private string ReadSingleRow(IDataRecord dataReader, List<Event> records)
        {
            int eventID = (int) dataReader["eventID"];
            int attendeeID = dataReader["attendeeID"] != DBNull.Value ? (int) dataReader["attendeeID"] : 0; 
            string title = dataReader["summary"].ToString().Trim();
            string description = dataReader["description"].ToString().Trim();
            string startTime = dataReader["startTime"].ToString().Trim();
            string endTime = dataReader["endTime"].ToString().Trim();
            string timezone = dataReader["timezone"].ToString().Trim();
            string classification = dataReader["classification"].ToString().Trim();
            string dtstamp = dataReader["dtstamp"].ToString().Trim();
            string uniqueID = dataReader["uniqueIdentifier"].ToString();
            string organizer = dataReader["organizer"].ToString();
            string recurFrequency = dataReader["recurFrequency"].ToString();
            string recurDateTime = dataReader["recurDateTime"].ToString();
            string newLine = Environment.NewLine;
            string formatedStr = "";

            if (newEvent.eventID != eventID)
            {
                attendees = new List<string>();
                attendeesRsvp = new List<string>();
                attendeesId = new List<int>();
                formatedStr = "Title: " + TrimString(title, 16) + newLine + "Description: " + TrimString(description, 20) + newLine + "Created: " + dtstamp;
                newEvent = new Event
                {
                    eventID = eventID,
                    summary = title,
                    description = description,
                    startTime = startTime,
                    endTime = endTime,
                    dtstamp = dtstamp,
                    uniqueIdentifier = uniqueID,
                    timeZone = timezone,
                    classification = classification,
                    organizer = organizer,
                    recurFrequency = recurFrequency,
                    recurUntil = recurDateTime
                };
                formatedRecords.Add(formatedStr);
                records.Add(newEvent);
            }

            if (dataReader["email"].ToString() != "")
            {
                attendees.Add(dataReader["email"].ToString());
                attendeesRsvp.Add(dataReader["rsvp"].ToString());
                attendeesId.Add(attendeeID);
                newEvent.attendees = attendees;
                newEvent.attendeesRsvp = attendeesRsvp;
                newEvent.attendeesId = attendeesId;
            }
            
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
