﻿using System;
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
        List<string> dbEvents = new List<string>();
        DataAccess db = new DataAccess();
        public EventForm()
        {
            InitializeComponent();
            InitializeDateTime();
        }

        private void submitButton_Click(object sender, EventArgs e)
        {
            string startTime = startDatePicker.Value.ToString("yyyy/MM/dd") + " " + startTimePicker.Value.TimeOfDay.ToString();
            string endTime = endDatePicker.Value.ToString("yyyy/MM/dd") + " " + endTimePicker.Value.TimeOfDay.ToString();

            HandleErrors.HandleError(titleTextBox.Text);
            HandleErrors.HandleTimeError(startDatePicker, startTimePicker, endTimePicker, endDatePicker);
            if (string.IsNullOrWhiteSpace(HandleErrors.ErrorMsg)) 
            {
                db.InsertEvent(titleTextBox.Text, descriptionTextBox.Text, startTime, endTime);
                GetData();
            }
            else
            {
                HandleErrors.DisplayErrorMsg();
            }

            titleTextBox.Text = "";
            descriptionTextBox.Text = "";
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
    }
}
