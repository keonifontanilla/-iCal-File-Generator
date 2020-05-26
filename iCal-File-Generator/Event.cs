using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public Dictionary<string, string> GetInputs()
        {
            inputs.Add("SUMMARY", summary);
            inputs.Add("DESCRIPTION", description);
            inputs.Add("DTSTART", startTime);
            inputs.Add("DTEND", endTime);
            inputs.Add("DTSTAMP", dtstamp);
            inputs.Add("UID:", uniqueIdentifier);
            return inputs;
        }
    }
}
