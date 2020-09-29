using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography.X509Certificates;

namespace iCal_File_Generator.Controls
{
    public partial class FullEventView : UserControl
    {
        DataAccess db;
        int index;

        public FullEventView(DataAccess db, int index)
        {
            this.db = db;
            this.index = index;

            InitializeComponent();
            DisplayEventInfo();
        }

        private void DisplayEventInfo()
        {
            string newLine = Environment.NewLine;

            if (index != -1)
            {
                string expandedRowStr = "Title: " + db.GetEvents()[index].summary + newLine
                                      + "Description: " + db.GetEvents()[index].description + newLine
                                      + "Location: " + db.GetEvents()[index].location + newLine
                                      + "Start time: " + db.GetEvents()[index].startTime + newLine
                                      + "End time: " + db.GetEvents()[index].endTime + newLine
                                      + "Timezone: " + db.GetEvents()[index].timeZone + newLine
                                      + "Classification: " + db.GetEvents()[index].classification + newLine
                                      + "Organizer: " + db.GetEvents()[index].organizer + newLine
                                      + "Created: " + db.GetEvents()[index].dtstamp + newLine
                                      + "Repeat: " + db.GetEvents()[index].recurFrequency + newLine
                                      + "Repeat end: " + db.GetEvents()[index].recurUntil + newLine;

                if (db.GetEvents()[index].attendees != null)
                {
                    for (int i = 0; i < db.GetEvents()[index].attendees.Count; i++)
                    {
                        if (db.GetEvents()[index].attendees[i] == "") { continue; }
                        expandedRowStr += "Attendee: " + db.GetEvents()[index].attendees[i] + ", " + "Rsvp: " + db.GetEvents()[index].attendeesRsvp[i] + newLine;
                    }
                }

                eventInfoTextBox.Text = expandedRowStr;
            }
        }
    }
}
