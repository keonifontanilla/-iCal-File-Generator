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
        public TimeZoneInfo timezone { get; set; }
        public string classification { get; set; }

        public string GetTZID()
        {
            string tzid = "";
            int indexOfNextSpace = 0;
            int indexOfSpace = timezone.DisplayName.IndexOf(" ") + 1;
            if (timezone.DisplayName.Substring(indexOfSpace).IndexOf(" ") >= 0)
            {
                indexOfNextSpace = timezone.DisplayName.Substring(indexOfSpace).IndexOf(" ");
                tzid = "US-" + timezone.DisplayName.Substring(indexOfSpace, indexOfNextSpace);
                return tzid;
            }
            tzid = "US-" + timezone.DisplayName.Substring(indexOfSpace);
            return tzid;
        }

        public string tzOffSetFrom { get; private set; }
        public string tzOffSetTo { get; private set; }

        public void GetTimezoneOffset()
        {
            switch (GetTZID())
            {
                case "US-Hawaii":
                    tzOffSetFrom = "-1000";
                    tzOffSetTo = "-1000";
                    break;
                case "US-Mountain":
                    tzOffSetFrom = "-0600";
                    tzOffSetTo = "-0700";
                    break;
                case "US-Central":
                    tzOffSetFrom = "-0500";
                    tzOffSetTo = "-0600";
                    break;
                case "US-Eastern":
                    tzOffSetFrom = "-0400";
                    tzOffSetTo = "-0500";
                    break;
                case "US-Alaskan":
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
            return inputs;
        }
    }
}
