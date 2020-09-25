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
        List<int> attendeesID = new List<int>();
        DataAccess db = new DataAccess();

        Panel attendeePanel, recurrencePanel;
        Button addAttendeeButton = new Button();
        GroupBox attendeeGroupBox;
        ComboBox frequencyComboBox;
        RadioButton neverRecurRadioButton;
        RadioButton untilRecurRadioButton;
        DateTimePicker untilDate;
        DateTimePicker untilTime;
        DateTime dateNow = DateTime.Now;

        private int eventID = 0;
        private bool updateClicked = false;
        private bool deleteAttendee = false;
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

            recurrencePanel = new Panel();
            recurrencePanel.Visible = false;

            // Dynamically added button click event handler
            addAttendeeButton.Click += new EventHandler(addAttendeeButton_Click);
        }

        private void submitButton_Click(object sender, EventArgs e)
        {
            string startTime = startDatePicker.Value.ToString("yyyy/MM/dd") + " " + startTimePicker.Value.TimeOfDay.ToString();
            string endTime = endDatePicker.Value.ToString("yyyy/MM/dd") + " " + endTimePicker.Value.TimeOfDay.ToString();
            string dtstamp = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fffff");
            string uid = CreateUID();
            string recurFrequency = frequencyComboBox != null ? frequencyComboBox.SelectedItem.ToString() : "";
            string recurUntil = (untilRecurRadioButton != null && untilRecurRadioButton.Checked) ? untilDate.Value.ToString("yyyy/MM/dd") + " " + untilTime.Value.TimeOfDay.ToString() : "";
            List<int> attendeesID;
            TimeZoneInfo timezone = (TimeZoneInfo)timezoneComboBox.SelectedItem;

            HandleErrors.HandleTitleError(titleTextBox.Text);
            HandleErrors.HandleTimeError(startDatePicker, startTimePicker, endTimePicker, endDatePicker, dateNow);
            // Disabled for testing purposes HandleErrors.HandleEmailError(GetAttendeesInput(), organizerTextBox.Text); 
            if (string.IsNullOrWhiteSpace(HandleErrors.ErrorMsg) && !updateClicked) 
            {
                db.InsertEvent(titleTextBox.Text, descriptionTextBox.Text, startTime, endTime, dtstamp, uid, timezone, classificationComboBox.Text, organizerTextBox.Text, GetAttendeesInput(), GetAttendeesRsvp(), recurFrequency, recurUntil, locationTextBox.Text);
                GetData();
                ClearInputs();
                MessageBox.Show("Insert Successful!");
            }
            else if (string.IsNullOrWhiteSpace(HandleErrors.ErrorMsg) && updateClicked)
            {
                attendeesID = db.GetEvents()[eventsListBox.SelectedIndex].attendeesId;
                
                // fix deleting a newly added attendee with text in the input box
                db.UpdateEvent(titleTextBox.Text, descriptionTextBox.Text, startTime, endTime, timezone, classificationComboBox.Text, organizerTextBox.Text, eventID, GetAttendeesInput(), GetAttendeesRsvp(), attendeesID, recurFrequency, recurUntil, locationTextBox.Text);

                if (deleteAttendee)
                {
                    foreach (int id in this.attendeesID)
                    {
                        db.DeleteAttendee(id);
                    }
                }
                
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
                                      + "Location: " + db.GetEvents()[index].location + newLine
                                      + "Start time: " + db.GetEvents()[index].startTime + newLine
                                      + "End time: " + db.GetEvents()[index].endTime + newLine
                                      + "Timezone: " + db.GetEvents()[index].timeZone + newLine
                                      + "Classification: " + db.GetEvents()[index].classification + newLine
                                      + "Organizer: " + db.GetEvents()[index].organizer + newLine
                                      + "Created: " + db.GetEvents()[index].dtstamp + newLine
                                      + "Repeat: " + db.GetEvents()[index].recurFrequency + newLine
                                      + "Repeat end: " + db.GetEvents()[index].recurUntil + newLine;
                
                viewPanel.Visible = true;

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

        private void panelCloseButton_Click(object sender, EventArgs e)
        {
            viewPanel.Visible = false;
        }

        // fix updateClicked boolean to open and close update
        private void updateButton_Click(object sender, EventArgs e)
        {
            LinkLabelLinkClickedEventArgs ex;
            int index = eventsListBox.SelectedIndex;

            if (index != -1 && !updateClicked)
            {
                updateClicked = true;
                updateButton.Text = "Cancel";

                titleTextBox.Text = db.GetEvents()[index].summary;
                descriptionTextBox.Text = db.GetEvents()[index].description;
                locationTextBox.Text = db.GetEvents()[index].location;
                organizerTextBox.Text = db.GetEvents()[index].organizer;

                SetDateTime(index);

                timezoneComboBox.SelectedItem = GetTimeZone(db.GetEvents()[index].timeZone);
                classificationComboBox.SelectedItem = db.GetEvents()[index].classification;
                eventID = db.GetEvents()[index].eventID;

                // attendees panel update
                attendeesButton_Click(sender, e);
                if (attendeePanel.Visible && (db.GetEvents()[index].attendees != null))
                {
                    for (int i = 0; i < db.GetEvents()[index].attendees.Count; i++)
                    {
                        addAttendeeButton_Click(sender, e);
                        attendees[i].Text = db.GetEvents()[index].attendees[i];
                        attendeesRsvp[i].SelectedItem = db.GetEvents()[index].attendeesRsvp[i];
                    }
                }

                // repeat panel update
                ex = new LinkLabelLinkClickedEventArgs(repeatsLinkLabel.Links[0]);
                repeatsLinkLabel_LinkClicked(sender, ex);
                frequencyComboBox.SelectedItem = db.GetEvents()[index].recurFrequency;
                if (db.GetEvents()[index].recurUntil != "")
                {
                    untilRecurRadioButton.Checked = true;
                    untilDate.Value = DateTime.Parse(db.GetEvents()[index].recurUntil);
                    untilTime.Value = DateTime.Parse(db.GetEvents()[index].recurUntil);
                }
            }
            else
            {
                ClearInputs();
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

            if (attendeePanel.Visible == false) 
            { 
                attendeePanel.Visible = true;
                eventsListBox.Visible = false;
            } 
            else
            {
                attendeePanel.Controls.Clear();
                attendeePanel.Visible = false;
                eventsListBox.Visible = true;
                attendees.Clear();
                attendeesRsvp.Clear();
                numOfAttendees = 1;
                // updateClicked = false;
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
            Button deleteAttendeeButton = new Button();
            attendeeGroupBox = new GroupBox();
            
            attendeeGroupBox.Location = new Point(0, numOfAttendees * 50);
            attendeeGroupBox.Size = new Size(attendeePanel.Location.X, 50);
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

            attendees.Add(attendeeEmailTextBox);
            attendeesRsvp.Add(rsvpComboBox);

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
            int dbIndex = eventsListBox.SelectedIndex;
            deleteAttendee = true;

            // delete input from panel
            foreach (Control item in attendeePanel.Controls)
            {
                if (item.Name == "attendeeGroupBox" + index)
                {
                    attendeePanel.Controls.Remove(item);
                }
            }

            /*
            // delete unwanted inputs for insert
            if (!updateClicked)
            {
                // delete attendeeEmailTextbox control from attendees list
                foreach (TextBox textbox in attendees)
                {
                    if (textbox.Name == "attendeeEmailTextbox" + index)
                    {
                        attendees.Remove(textbox);
                        break;
                    }
                }
                // delete rsvpComboBox control from attendeesRsvp list
                foreach (ComboBox combobox in attendeesRsvp)
                {
                    if (combobox.Name == "rsvpComboBox" + index)
                    {
                        attendeesRsvp.Remove(combobox);
                        break;
                    }
                }
            }
            */

            // get attendee IDs to delete from database
            int attIndex = attendees.FindIndex(att => att.Name == "attendeeEmailTextbox" + index);
            int rsvpIndex = attendeesRsvp.FindIndex(rsvp => rsvp.Name == "rsvpComboBox" + index);
            List<int> dbAttendeesId = db.GetEvents()[dbIndex].attendeesId;
            
            if (updateClicked && (attIndex != -1) && (dbAttendeesId != null) && !(attIndex >= dbAttendeesId.Count))
            {
                attendeesID.Add(dbAttendeesId[attIndex]);
            }
            else
            {
                attendees.RemoveAt(attIndex);
                attendeesRsvp.RemoveAt(attIndex);
            }
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

        private void repeatsLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Label frequencyLabel = new Label();
            frequencyComboBox = new ComboBox();
            neverRecurRadioButton = new RadioButton();
            untilRecurRadioButton = new RadioButton();
            untilDate = new DateTimePicker();
            untilTime = new DateTimePicker();

            List<string> frequencies = new List<string>()
            {
                "Once", "Daily", "Weekly", "Monthly", "Yearly"
            };

            if (recurrencePanel.Visible == false)
            {
                recurrencePanel.Visible = true;
                recurrencePanel.Size = new Size(318, 139);
                recurrencePanel.Location = new Point(122, 427);
                recurrencePanel.BackColor = Color.White;
                submitButton.Location = new Point(submitButton.Location.X, recurrencePanel.Bottom + 5);
                clearInputsButton.Location = new Point(clearInputsButton.Location.X, recurrencePanel.Bottom + 5);

                frequencyLabel.Text = "Frequency";
                frequencyLabel.Name = "frequencyLabel";
                frequencyComboBox.Name = "frequencyComboBox";
                recurrencePanel.Controls.Add(frequencyLabel);
                frequencyComboBox.DataSource = frequencies;
                frequencyComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                frequencyComboBox.Location = new Point(frequencyLabel.Right, frequencyLabel.Location.Y);
                recurrencePanel.Controls.Add(frequencyComboBox);

                frequencyComboBox.SelectedIndexChanged += new EventHandler(frequencyComboBox_OnChange);

            }
            else
            {
                submitButton.Location = new Point(submitButton.Location.X, 447);
                clearInputsButton.Location = new Point(clearInputsButton.Location.X, 447);
                recurrencePanel.Controls.Clear();                
                recurrencePanel.Visible = false;
            }

            this.Controls.Add(recurrencePanel);
        }

        private void frequencyComboBox_OnChange(object send, EventArgs e)
        {
            Label endLabel = new Label();

            if (frequencyComboBox.SelectedItem.ToString() != "Once")
            {
                endLabel.Text = "End";
                endLabel.Location = new Point(0, frequencyComboBox.Bottom + 10);
                endLabel.Visible = true;
                recurrencePanel.Controls.Add(endLabel);

                neverRecurRadioButton.Text = "Never";
                neverRecurRadioButton.Checked = true;
                neverRecurRadioButton.Visible = true;
                neverRecurRadioButton.Location = new Point(endLabel.Right, endLabel.Location.Y);
                recurrencePanel.Controls.Add(neverRecurRadioButton);

                untilRecurRadioButton.Text = "Until";
                untilRecurRadioButton.Visible = true;
                untilRecurRadioButton.Location = new Point(endLabel.Right, endLabel.Bottom);
                recurrencePanel.Controls.Add(untilRecurRadioButton);

                untilDate.Visible = true;
                untilDate.Location = new Point(untilRecurRadioButton.Location.X, untilRecurRadioButton.Bottom);
                recurrencePanel.Controls.Add(untilDate);

                untilTime.Visible = true;
                untilTime.Location = new Point(untilRecurRadioButton.Location.X, untilDate.Bottom);
                untilTime.Size = new Size(98, 20);
                untilTime.ShowUpDown = true;
                untilTime.Format = DateTimePickerFormat.Time;
                recurrencePanel.Controls.Add(untilTime);
            }
            else
            {
                foreach(Control item in recurrencePanel.Controls)
                {
                    if (item.Name != "frequencyComboBox" && item.Name != "frequencyLabel") { item.Visible = false; }
                }
            }
        }
    }
}
