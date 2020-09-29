using iCal_File_Generator.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace iCal_File_Generator
{
    public partial class EventForm : Form
    {
        DataAccess db = new DataAccess();
        List<string> dbEvents = new List<string>();
        List<int> attendeesID = new List<int>();
        EventListView eventListView;
        AttendeesListView attendeesListView;

        Panel recurrencePanel;
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
            InitializeTimezone();
            InitializeClassification();

            eventListView = new EventListView(db);
            attendeesListView = new AttendeesListView();

            recurrencePanel = new Panel();
            recurrencePanel.Visible = false;

            eventViewPanel.Controls.Add(eventListView);
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
                ClearInputs();
                MessageBox.Show("Insert Successful!");
            }
            else if (string.IsNullOrWhiteSpace(HandleErrors.ErrorMsg) && updateClicked)
            {
                attendeesID = db.GetEvents()[eventListView.Index()].attendeesId;
                
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
        }

        private void InitializeDateTime()
        {
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
            if (viewButton.Text == "View")
            {
                eventViewPanel.Controls[0].Visible = false;
                eventViewPanel.Controls.Add(new FullEventView(db, eventListView.Index()));
                viewButton.Text = "Close";
            }
            else
            {
                eventViewPanel.Controls[0].Visible = true;
                eventViewPanel.Controls.Remove(eventViewPanel.Controls[1]);
                viewButton.Text = "View";
            }
        }

        // fix updateClicked boolean to open and close update
        private void updateButton_Click(object sender, EventArgs e)
        {
            LinkLabelLinkClickedEventArgs ex;
            attendeesListView = new AttendeesListView(db, updateClicked, deleteAttendee, attendeesID, numOfAttendees, eventListView.Index());
            int index = eventListView.Index();

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
                attendeesListView.UpdateAttendees(sender, e);

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
            int index = eventListView.Index();
            if (index != -1)
            {
                db.DeleteEvent(db.GetEvents()[index].eventID);
            }
            
            ClearInputs();
        }

        private void generateButton_Click(object sender, EventArgs e)
        {
            int index = eventListView.Index();

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
            attendeesListView = new AttendeesListView(db, updateClicked, deleteAttendee, attendeesID, numOfAttendees, eventListView.Index());

            eventViewPanel.Controls[0].Visible = false;
            eventViewPanel.Controls.Add(attendeesListView);
        }


        private List<string> GetAttendeesInput()
        {
            List<string> attendees = new List<string>();

            foreach(TextBox attendee in attendeesListView.Attendees)
            {
                attendees.Add(attendee.Text);
            }

            return attendees; 
        }

        private List<string> GetAttendeesRsvp()
        {
            List<string> attendeesRsvp = new List<string>();

            foreach (ComboBox rsvp in attendeesListView.AttendeesRsvp)
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
