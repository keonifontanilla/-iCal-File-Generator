using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace iCal_File_Generator.Controls
{
    public partial class AttendeesListView : UserControl
    {
        public List<TextBox> Attendees { get; private set; } = new List<TextBox>();
        public List<ComboBox> AttendeesRsvp { get; private set; } = new List<ComboBox>();
        public List<int> AttendeesID { get; private set; } = new List<int>();
        public bool DeleteAttendee { get; private set; }

        DataAccess db;

        private int numOfAttendees = 1;
        private int dbIndex;
        private bool updateClicked;

        public AttendeesListView(DataAccess db)
        {
            this.db = db;

            InitializeComponent();
        }

        public AttendeesListView(DataAccess db, bool updateClicked, int dbIndex)
        {
            this.db = db;
            this.updateClicked = updateClicked;
            this.dbIndex = dbIndex;

            InitializeComponent();

            attendeePanel.BackColor = Color.White;
        }

        private void addAttendeeButton_Click(object sender, EventArgs e)
        {
            TextBox attendeeEmailTextBox = new TextBox();
            Label attendeeLabel = new Label();
            ComboBox rsvpComboBox = new ComboBox();
            Button deleteAttendeeButton = new Button();
            GroupBox attendeeGroupBox = new GroupBox();

            attendeeGroupBox.Location = new Point(0, numOfAttendees * 50);
            attendeeGroupBox.Size = new Size(402, 50);
            attendeeGroupBox.Name = "attendeeGroupBox" + numOfAttendees.ToString();

            attendeeLabel.Location = new Point(25, attendeeGroupBox.Size.Height / 2);
            attendeeLabel.Text = "Email: ";
            attendeeLabel.Name = "attendeeLabel" + numOfAttendees.ToString();
            attendeeLabel.Size = new Size(50, 21);

            attendeeEmailTextBox.Location = new Point(attendeeLabel.Right, attendeeGroupBox.Size.Height / 2);
            attendeeEmailTextBox.Name = "attendeeEmailTextbox" + numOfAttendees.ToString();

            List<string> rsvp = new List<string>()
            {
                "False", "True"
            };
            rsvpComboBox.DataSource = rsvp;
            rsvpComboBox.Location = new Point(attendeeEmailTextBox.Location.X + 105, attendeeGroupBox.Size.Height / 2);
            rsvpComboBox.Name = "rsvpComboBox" + numOfAttendees.ToString();
            rsvpComboBox.Size = new Size(60, 21);

            deleteAttendeeButton.Text = "Delete";
            deleteAttendeeButton.Location = new Point(rsvpComboBox.Right + 4, attendeeGroupBox.Size.Height / 2);
            deleteAttendeeButton.Name = "deleteAttendeeButton" + numOfAttendees.ToString();
            deleteAttendeeButton.Size = new Size(46, 23);

            Attendees.Add(attendeeEmailTextBox);
            AttendeesRsvp.Add(rsvpComboBox);

            numOfAttendees++;

            attendeeGroupBox.Controls.Add(attendeeLabel);
            attendeeGroupBox.Controls.Add(attendeeEmailTextBox);
            attendeeGroupBox.Controls.Add(rsvpComboBox);
            attendeeGroupBox.Controls.Add(deleteAttendeeButton);

            attendeePanel.Controls.Add(attendeeGroupBox);

            deleteAttendeeButton.Click += new EventHandler(deleteAttendeeButton_Click);
        }

        private void deleteAttendeeButton_Click(object send, EventArgs e)
        {
            // get # in the name of attendeeGroupBox# where # starts at 1
            Control btn = (Control)send;
            string index = btn.Name.Substring(btn.Name.Length - 1, 1);
            
            DeleteAttendee = true;

            // delete input from panel
            foreach (Control item in attendeePanel.Controls)
            {
                if (item.Name == "attendeeGroupBox" + index)
                {
                    attendeePanel.Controls.Remove(item);
                }
            }

            // get attendee IDs to delete from database
            int attIndex = Attendees.FindIndex(att => att.Name == "attendeeEmailTextbox" + index);
            int rsvpIndex = AttendeesRsvp.FindIndex(rsvp => rsvp.Name == "rsvpComboBox" + index);
            List<int> dbAttendeesId = db.GetEvents()[dbIndex].attendeesId;

            if (updateClicked && (attIndex != -1) && (dbAttendeesId != null) && !(attIndex >= dbAttendeesId.Count))
            {
                AttendeesID.Add(dbAttendeesId[attIndex]);
            }
            else
            {
                Attendees.RemoveAt(attIndex);
                AttendeesRsvp.RemoveAt(attIndex);
            }
        }

        public void UpdateAttendees(object sender, EventArgs e)
        {
            if (attendeePanel.Visible && (db.GetEvents()[dbIndex].attendees != null))
            {
                for (int i = 0; i < db.GetEvents()[dbIndex].attendees.Count; i++)
                {
                    addAttendeeButton_Click(sender, e);
                    Attendees[i].Text = db.GetEvents()[dbIndex].attendees[i];
                    AttendeesRsvp[i].SelectedItem = db.GetEvents()[dbIndex].attendeesRsvp[i];
                }
            }
        }
    }
}
