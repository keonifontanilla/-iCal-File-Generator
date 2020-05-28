using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        }

        private void submitButton_Click(object sender, EventArgs e)
        {
            string startTime = startDatePicker.Value.ToString("yyyy/MM/dd") + " " + startTimePicker.Value.TimeOfDay.ToString();
            string endTime = endDatePicker.Value.ToString("yyyy/MM/dd") + " " + endTimePicker.Value.TimeOfDay.ToString();
            string dtstamp = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fffff");
            string uid = CreateUID();
            // string timezone = timezoneComboBox.Text;
            TimeZoneInfo timezone = (TimeZoneInfo)timezoneComboBox.SelectedItem;
            //string tzOffset = tz.BaseUtcOffset.ToString();

            HandleErrors.HandleError(titleTextBox.Text);
            HandleErrors.HandleTimeError(startDatePicker, startTimePicker, endTimePicker, endDatePicker);
            if (string.IsNullOrWhiteSpace(HandleErrors.ErrorMsg)) 
            {
                db.InsertEvent(titleTextBox.Text, descriptionTextBox.Text, startTime, endTime, dtstamp, uid, timezone);
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
    }
}
