using System;
using System.Collections.Generic;

namespace iCal_File_Generator
{
    public class Event
    {
        private Dictionary<string, string> inputs = new Dictionary<string, string>();

        public int eventID { get; set; }
        public string summary { get; set; }
        public string description { get; set; }
        public string startTime { get; set; }
        public string endTime { get; set; }
        public string dtstamp { get; set; }
        public string uniqueIdentifier { get; set; }
        public string classification { get; set; }
        public string timeZoneStandardName { get; set; }
        public string timeZone { get; set; }
        public string tzid { get; private set; }
        public string tzOffSetFrom { get; private set; }
        public string tzOffSetTo { get; private set; }
        public string organizer { get; set; }
        public List<string> attendees { get; set; }
        public List<string> attendeesRsvp { get; set; }
        public List<int> attendeesId { get; set; }
        public string recurFrequency { get; set; }
        public string recurUntil { get; set; }

        public void GetTimeZoneOffset()
        {
            switch (timeZoneStandardName)
            {
                case "Hawaiian Standard Time":
                    tzid = "Pacific/Honolulu";
                    tzOffSetFrom = "-1000";
                    tzOffSetTo = "-1000";
                    break;
                case "Pacific Standard Time":
                    tzid = "America/Los_Angeles";
                    tzOffSetFrom = "-0700";
                    tzOffSetTo = "-0800";
                    break;
                case "Mountain Standard Time":
                    tzid = "America/Phoenix";
                    tzOffSetFrom = "-0600";
                    tzOffSetTo = "-0700";
                    break;
                case "Central Standard Time":
                    tzid = "America/Chicago";
                    tzOffSetFrom = "-0500";
                    tzOffSetTo = "-0600";
                    break;
                case "Eastern Standard Time":
                    tzid = "America/New_York";
                    tzOffSetFrom = "-0400";
                    tzOffSetTo = "-0500";
                    break;
                case "Alaskan Standard Time":
                    tzid = "America/Anchorage";
                    tzOffSetFrom = "-0800";
                    tzOffSetTo = "-0900";
                    break;
            }
        }

        public Dictionary<string, string> GetInputs()
        {
            inputs.Add("SUMMARY", summary);
            inputs.Add("DESCRIPTION", description);
            inputs.Add("DTSTART", startTime);
            inputs.Add("DTEND", endTime);
            inputs.Add("DTSTAMP", dtstamp);
            inputs.Add("UID", uniqueIdentifier);
            inputs.Add("CLASS", classification);
            inputs.Add("ORGANIZER", organizer);
            inputs.Add("RRULE", recurFrequency);

            return inputs;
        }
    }
}
