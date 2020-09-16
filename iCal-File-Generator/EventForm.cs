﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

        Panel attendeePanel, recurrencePanel;
        Button addAttendeeButton = new Button();
        ComboBox frequencyComboBox;
        RadioButton neverRecurRadioButton;
        RadioButton untilRecurRadioButton;
        DateTimePicker untilDate;
        DateTimePicker untilTime;

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

            HandleErrors.HandleError(titleTextBox.Text);
            HandleErrors.HandleTimeError(startDatePicker, startTimePicker, endTimePicker, endDatePicker);
            HandleErrors.HandleEmailError(GetAttendeesInput(), organizerTextBox.Text);
            if (string.IsNullOrWhiteSpace(HandleErrors.ErrorMsg) && !updateClicked) 
            {
                db.InsertEvent(titleTextBox.Text, descriptionTextBox.Text, startTime, endTime, dtstamp, uid, timezone, classificationComboBox.Text, organizerTextBox.Text, GetAttendeesInput(), GetAttendeesRsvp(), recurFrequency, recurUntil);
                GetData();
                ClearInputs();
                MessageBox.Show("Insert Successful!");
            }
            else if (string.IsNullOrWhiteSpace(HandleErrors.ErrorMsg) && updateClicked)
            {
                attendeesID = db.GetEvents()[eventsListBox.SelectedIndex].attendeesId;
                db.UpdateEvent(titleTextBox.Text, descriptionTextBox.Text, startTime, endTime, timezone, classificationComboBox.Text, organizerTextBox.Text, eventID, GetAttendeesInput(), GetAttendeesRsvp(), attendeesID, recurFrequency, recurUntil);
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

        private void repeatsLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Label frequencyLabel = new Label();
            Label endLabel = new Label();
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
            }
            else
            {
                submitButton.Location = new Point(submitButton.Location.X, 447);
                clearInputsButton.Location = new Point(clearInputsButton.Location.X, 447);
                recurrencePanel.Controls.Clear();                
                recurrencePanel.Visible = false;
            }

            frequencyLabel.Text = "Frequency";
            recurrencePanel.Controls.Add(frequencyLabel);

            frequencyComboBox.DataSource = frequencies;
            frequencyComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            frequencyComboBox.Location = new Point(frequencyLabel.Right, frequencyLabel.Location.Y);
            recurrencePanel.Controls.Add(frequencyComboBox);

            endLabel.Text = "End";
            endLabel.Location = new Point(0, frequencyComboBox.Bottom + 10);
            recurrencePanel.Controls.Add(endLabel);

            neverRecurRadioButton.Text = "Never";
            neverRecurRadioButton.Checked = true;
            neverRecurRadioButton.Location = new Point(endLabel.Right, endLabel.Location.Y);
            recurrencePanel.Controls.Add(neverRecurRadioButton);

            untilRecurRadioButton.Text = "Until";
            untilRecurRadioButton.Location = new Point(endLabel.Right, endLabel.Bottom);
            recurrencePanel.Controls.Add(untilRecurRadioButton);

            untilDate.Location = new Point(untilRecurRadioButton.Location.X, untilRecurRadioButton.Bottom);
            recurrencePanel.Controls.Add(untilDate);

            untilTime.Location = new Point(untilRecurRadioButton.Location.X, untilDate.Bottom);
            untilTime.Size = new Size(98, 20);
            untilTime.ShowUpDown = true;
            untilTime.Format = DateTimePickerFormat.Time;
            recurrencePanel.Controls.Add(untilTime);

            this.Controls.Add(recurrencePanel);
        }
    }
}
