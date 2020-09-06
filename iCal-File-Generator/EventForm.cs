using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace iCal_File_Generator
{
    public partial class EventForm : Form
    {
        List<string> dbEvents = new List<string>();
        List<TextBox> attendees = new List<TextBox>();
        List<ComboBox> attendeesRsvp = new List<ComboBox>();
        DataAccess db = new DataAccess();

        Panel attendeePanel;
        Button addAttendeeButton = new Button();

        private int eventID = 0;
        private bool updateClicked = false;
        private int numOfAttendees = 1;

        public EventForm()
        {
            InitializeComponent();
            InitializeDateTime();
            InitializeListbox();
            InitializeTimezone();
            InitializeClassification();

            attendeePanel = new Panel();
            attendeePanel.Visible = false;
        }

        private void submitButton_Click(object sender, EventArgs e)
        {
            string startTime = startDatePicker.Value.ToString("yyyy/MM/dd") + " " + startTimePicker.Value.TimeOfDay.ToString();
            string endTime = endDatePicker.Value.ToString("yyyy/MM/dd") + " " + endTimePicker.Value.TimeOfDay.ToString();
            string dtstamp = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fffff");
            string uid = CreateUID();
            TimeZoneInfo timezone = (TimeZoneInfo)timezoneComboBox.SelectedItem;

            HandleErrors.HandleError(titleTextBox.Text);
            HandleErrors.HandleTimeError(startDatePicker, startTimePicker, endTimePicker, endDatePicker);
            if (string.IsNullOrWhiteSpace(HandleErrors.ErrorMsg) && !updateClicked) 
            {
                db.InsertEvent(titleTextBox.Text, descriptionTextBox.Text, startTime, endTime, dtstamp, uid, timezone, classificationComboBox.Text, organizerTextBox.Text, GetAttendeesInput(), GetAttendeesRsvp());
                GetData();
                ClearInputs();
            }
            else if (string.IsNullOrWhiteSpace(HandleErrors.ErrorMsg) && updateClicked)
            {
                db.UpdateEvent(titleTextBox.Text, descriptionTextBox.Text, startTime, endTime, timezone, classificationComboBox.Text, eventID);
                GetData();
                ClearInputs();
                updateClicked = false;
                MessageBox.Show("Update Successful!");
            }
            else
            {
                HandleErrors.DisplayErrorMsg();
            }
        }

        private void ClearInputs()
        {
            EventForm newEventForm = new EventForm();
            newEventForm.Show();
            this.Dispose(false);

            /*
            InitializeDateTime();
            InitializeTimezone();
            InitializeClassification();

            titleTextBox.Text = "";
            descriptionTextBox.Text = "";
            startDatePicker.Value = DateTime.Now;
            startTimePicker.Value = DateTime.Now;
            endDatePicker.Value = DateTime.Now;
            endTimePicker.Value = DateTime.Now;
            */
        }

        private void InitializeDateTime()
        {
            GetData();

            startDatePicker.MinDate = DateTime.Now.AddSeconds(-DateTime.Now.Second);
            startTimePicker.Format = DateTimePickerFormat.Time;
            startTimePicker.ShowUpDown = true;

            endDatePicker.MinDate = startDatePicker.Value;
            endTimePicker.Format = DateTimePickerFormat.Time;
            endTimePicker.ShowUpDown = true;
        }

        private void startDatePicker_ValueChanged(object sender, EventArgs e)
        {
            endDatePicker.MinDate = startDatePicker.Value;
        }

        private void GetData()
        {
            dbEvents = db.ListEvents();
            eventsListBox.DataSource = dbEvents;
        }

        private void InitializeListbox()
        {
            eventsListBox.DrawMode = DrawMode.OwnerDrawVariable;
            eventsListBox.MeasureItem += new MeasureItemEventHandler(eventsListBox_MeasureItem);
            eventsListBox.DrawItem += new DrawItemEventHandler(eventsListBox_DrawItem);
            this.Controls.Add(this.eventsListBox);
        }

        private void eventsListBox_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            foreach (string item in eventsListBox.Items)
            {
                //Set the Height of the item at index 2 to 50
                if (e.Index == eventsListBox.Items.IndexOf(item)) { e.ItemHeight = 100; }
            }
        }

        private void eventsListBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            if (e.Index != -1) { e.Graphics.DrawString(eventsListBox.Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds); }
        }

        private string CreateUID()
        {
            string randomNum = "";
            Random random = new Random();
            randomNum = random.Next().ToString() + random.Next().ToString();

            return randomNum + "@kmf";
        }

        private void InitializeTimezone()
        {
            List<TimeZoneInfo> zone = new List<TimeZoneInfo>
            {
                TimeZoneInfo.FindSystemTimeZoneById("Hawaiian Standard Time"),
                TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time"),
                TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"),
                TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time"),
                TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"),
                TimeZoneInfo.FindSystemTimeZoneById("Alaskan Standard Time"),
            };

            timezoneComboBox.DataSource = zone;
        }

        private TimeZoneInfo GetTimeZone(string tzDayName)
        {
            switch(tzDayName)
            {
                case string tz when tz.Contains("Hawaii"):
                    return TimeZoneInfo.FindSystemTimeZoneById("Hawaiian Standard Time");
                case string tz when tz.Contains("Pacific"):
                    return TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
                case string tz when tz.Contains("Mountain"):
                    return TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time");
                case string tz when tz.Contains("Central"):
                    return TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
                case string tz when tz.Contains("Eastern"):
                    return TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            }
            return TimeZoneInfo.FindSystemTimeZoneById("Alaskan Standard Time");
        }   

        private void InitializeClassification()
        {
            List<string> classification = new List<string>()
            {
                "PUBLIC", "PRIVATE", "CONFIDENTIAL"
            };
            classificationComboBox.DataSource = classification;
        }

        private void viewButton_Click(object sender, EventArgs e)
        {
            int index = eventsListBox.SelectedIndex;
            string newLine = Environment.NewLine;
           
            if (index != -1)
            {
                string expandedRowStr = "Title: " + db.GetEvents()[index].summary + newLine
                                      + "Description: " + db.GetEvents()[index].description + newLine
                                      + "Start time: " + db.GetEvents()[index].startTime + newLine
                                      + "End time: " + db.GetEvents()[index].endTime + newLine
                                      + "Timezone: " + db.GetEvents()[index].timeZone + newLine
                                      + "Classification: " + db.GetEvents()[index].classification + newLine
                                      + "Created: " + db.GetEvents()[index].dtstamp + newLine;
                
                viewPanel.Visible = true;

                foreach(string record in db.GetEvents()[index].attendees)
                {
                    expandedRowStr += "Attendee: " + record + newLine;
                }

                eventInfoTextBox.Text = expandedRowStr;                
            }
        }

        private void panelCloseButton_Click(object sender, EventArgs e)
        {
            viewPanel.Visible = false;
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            int index = eventsListBox.SelectedIndex;

            if (index != -1)
            {
                updateClicked = true;

                titleTextBox.Text = db.GetEvents()[index].summary;
                descriptionTextBox.Text = db.GetEvents()[index].description;

                SetDateTime(index);

                timezoneComboBox.SelectedItem = GetTimeZone(db.GetEvents()[index].timeZone);
                classificationComboBox.SelectedItem = db.GetEvents()[index].classification;
                eventID = db.GetEvents()[index].eventID;
            }
        }

        private void clearInputsButton_Click(object sender, EventArgs e)
        {
            ClearInputs();
            updateClicked = false;
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            int index = eventsListBox.SelectedIndex;
            if (index != -1)
            {
                db.DeleteEvent(db.GetEvents()[index].eventID);
            }
            
            GetData();
            ClearInputs();
        }

        private void generateButton_Click(object sender, EventArgs e)
        {
            int index = eventsListBox.SelectedIndex;

            if (index != -1)
            {
                FileGenerator fg = new FileGenerator();
                Event getEvent = db.GetEvents()[index];
                var dtstamp = DateTime.Parse(db.GetEvents()[index].dtstamp);

                SetDateTime(index);

                getEvent.startTime = startDatePicker.Value.ToString("yyyy/MM/dd") + " " + startTimePicker.Value.TimeOfDay.ToString();
                getEvent.endTime = endDatePicker.Value.ToString("yyyy/MM/dd") + " " + endTimePicker.Value.TimeOfDay.ToString();
                getEvent.dtstamp = dtstamp.ToString("yyyy/MM/dd HH:mm:ss.fffff");

                db.GetEvents()[index].timeZoneStandardName = GetTimeZone(db.GetEvents()[index].timeZone).StandardName;
                fg.FormatInput(db.GetEvents()[index]);

                ClearInputs();
            }
        }

        private void SetDateTime(int index)
        {
            var startTime = DateTime.Parse(db.GetEvents()[index].startTime);
            var endTime = DateTime.Parse(db.GetEvents()[index].endTime);

            startDatePicker.MinDate = startTime;
            startDatePicker.Value = startTime;
            startTimePicker.Value = startTime;
            endDatePicker.Value = endTime;
            endTimePicker.Value = endTime;
        }

        private void attendeesButton_Click(object sender, EventArgs e)
        {
            TextBox attendeeEmailTextbox = new TextBox();
            
            // Dynamically added button click event handler
            addAttendeeButton.Click += new EventHandler(addAttendeeButton_Click);

            if (attendeePanel.Visible == false) 
            { 
                attendeePanel.Visible = true;
                eventsListBox.Visible = false;
            } 
            else
            {
                attendeePanel.Visible = false;
                eventsListBox.Visible = true;
            }

            attendeePanel.Size = new Size(402, 364);
            attendeePanel.Location = new Point(527, 50);
            attendeePanel.BackColor = Color.White;
            this.Controls.Add(attendeePanel);

            addAttendeeButton.Text = "Add Attendee";
            addAttendeeButton.Name = "addAttendeeButton";
            addAttendeeButton.AutoSize = true;

            attendeePanel.Controls.Add(addAttendeeButton);
        }

        // Click handler for dynamically generated button in attendee panel
        private void addAttendeeButton_Click(object send, EventArgs e)
        {
            TextBox attendeeEmailTextBox = new TextBox();
            Label attendeeLabel = new Label();
            ComboBox rsvpComboBox = new ComboBox();

            attendeeLabel.Location = new Point(25, numOfAttendees * 25);
            attendeeLabel.Text = "Email: ";
            attendeeLabel.AutoSize = true;

            attendeeEmailTextBox.Location = new Point(100, numOfAttendees * 25);
            attendeeEmailTextBox.Name = "attendeeEmailTextbox" + numOfAttendees.ToString();

            List<string> rsvp = new List<string>()
            {
                "False", "True"
            };
            rsvpComboBox.DataSource = rsvp;
            rsvpComboBox.Location = new Point(attendeeEmailTextBox.Location.X + 105, numOfAttendees * 25);
            rsvpComboBox.Name = "rsvpComboBox" + numOfAttendees.ToString();
            rsvpComboBox.Size = new Size(60, 21);

            attendees.Add(attendeeEmailTextBox);
            attendeesRsvp.Add(rsvpComboBox);

            numOfAttendees++;

            attendeePanel.Controls.Add(attendeeLabel);
            attendeePanel.Controls.Add(attendeeEmailTextBox);
            attendeePanel.Controls.Add(rsvpComboBox);
        }

        private List<string> GetAttendeesInput()
        {
            List<string> attendees = new List<string>();

            foreach(TextBox attendee in this.attendees)
            {
                attendees.Add(attendee.Text);
            }

            return attendees; 
        }

        private List<string> GetAttendeesRsvp()
        {
            List<string> attendeesRsvp = new List<string>();

            foreach (ComboBox rsvp in this.attendeesRsvp)
            {
                attendeesRsvp.Add(rsvp.SelectedItem.ToString());
            }

            return attendeesRsvp;
        }
    }
}
