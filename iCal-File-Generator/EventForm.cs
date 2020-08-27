using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace iCal_File_Generator
{
    public partial class EventForm : Form
    {
        List<string> dbEvents = new List<string>();
        DataAccess db = new DataAccess();
        public EventForm()
        {
            InitializeComponent();
            InitializeDateTime();
            InitializeListbox();
            InitializeTimezone();
            InitializeClassification();
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
            if (string.IsNullOrWhiteSpace(HandleErrors.ErrorMsg)) 
            {
                db.InsertEvent(titleTextBox.Text, descriptionTextBox.Text, startTime, endTime, dtstamp, uid, timezone, classificationComboBox.Text);
                GetData();
                titleTextBox.Text = "";
                descriptionTextBox.Text = "";
            }
            else
            {
                HandleErrors.DisplayErrorMsg();
            }
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
                                      + "Created: " + db.GetEvents()[index].dtstamp;

                var startTime = DateTime.Parse(db.GetEvents()[index].startTime);
                var endTime = DateTime.Parse(db.GetEvents()[index].endTime);

                eventInfoTextBox.Text = expandedRowStr;

                titleTextBox.Text = db.GetEvents()[index].summary;
                descriptionTextBox.Text = db.GetEvents()[index].description;

                startDatePicker.MinDate = startTime;
                startDatePicker.Value = startTime;
                startTimePicker.Value = startTime;

                endDatePicker.Value = endTime;
                endTimePicker.Value = endTime;

                timezoneComboBox.SelectedItem = GetTimeZone(db.GetEvents()[index].timeZone);

                classificationComboBox.SelectedItem = db.GetEvents()[index].classification;

            }
        }
    }
}
