using System;
using System.Collections.Generic;
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
        public EventForm()
        {
            InitializeComponent();
            InitializeDateTime();
        }

        private void submitButton_Click(object sender, EventArgs e)
        {
            DataAccess db = new DataAccess();
            string startTime = startDatePicker.Value.ToString("yyyy/MM/dd") + " " + startTimePicker.Value.TimeOfDay.ToString();
            string endTime = endDatePicker.Value.ToString("yyyy/MM/dd") + " " + endTimePicker.Value.TimeOfDay.ToString();

            db.InsertEvent(titleTextBox.Text, descriptionTextBox.Text, startTime, endTime);

            titleTextBox.Text = "";
            descriptionTextBox.Text = "";
        }

        private void InitializeDateTime()
        {
            startDatePicker.MinDate = DateTime.Today;
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

        private void startTimePicker_ValueChanged(object sender, EventArgs e)
        {
            if (startTimePicker.Value.TimeOfDay < DateTime.Now.TimeOfDay)
            {
                MessageBox.Show("ILLEGAL");
                startTimePicker.MinDate = DateTime.Now;
            }
        }
    }
}
