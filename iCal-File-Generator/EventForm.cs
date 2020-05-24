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

            db.InsertEvent(titleTextBox.Text, descriptionTextBox.Text, startTime);

            titleTextBox.Text = "";
            descriptionTextBox.Text = "";
        }

        private void InitializeDateTime()
        {
            startDatePicker.MinDate = DateTime.Today;
            startTimePicker.Format = DateTimePickerFormat.Time;
            startTimePicker.ShowUpDown = true;
        }
    }
}
