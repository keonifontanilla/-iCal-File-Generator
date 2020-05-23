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

        public Dictionary<string, string> getInputs
        {
            get
            {
                inputs.Add("TITLE", summary);
                inputs.Add("DESCRIPTION", description);
                return inputs;
            }
            private set { 
            }
        }
    }
}
