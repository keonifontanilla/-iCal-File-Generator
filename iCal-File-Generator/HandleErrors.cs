using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace iCal_File_Generator
{
    public static class HandleErrors
    {
        public static string ErrorMsg { get; private set; }

        /***********************************************************************************************
         * Display error messages
        ***********************************************************************************************/
        public static void DisplayErrorMsg()
        {
            MessageBox.Show(ErrorMsg);
            ErrorMsg = "";
        }

        /***********************************************************************************************
         * Handles empty required inputs
        ***********************************************************************************************/
        public static void HandleError(string input)
        {
            ErrorMsg += string.IsNullOrWhiteSpace(input) ? "Cannot leave title blank!\n" : "";
        }

        /***********************************************************************************************
         * Handles empty required inputs
        ***********************************************************************************************/
        public static void HandleTimeError(DateTimePicker startDatePicker, DateTimePicker startTimePicker, DateTimePicker endTimePicker, DateTimePicker endDatePicker)
        {
            // Check start time errors
            ErrorMsg += (startTimePicker.Value.TimeOfDay < startDatePicker.Value.TimeOfDay) && (startTimePicker.Value.Date == startDatePicker.Value.Date)
                ? "Cannot pick time in the past!\n" : "";
            // Check end time errors
            ErrorMsg += (TrimTime(endTimePicker.Value).TimeOfDay < TrimTime(startTimePicker.Value).TimeOfDay) && (endDatePicker.Value.Date == startDatePicker.Value.Date)
                ? "Cannot have end time before start time!\n" : "";
        }

        /***********************************************************************************************
         * Handles invalid email inputs
        ***********************************************************************************************/
        public static void HandleEmailError(List<string> emails, string organizer)
        {
            Regex rx = new Regex(@"^[-!#$%&'*+/0-9=?A-Z^_a-z{|}~](\.?[-!#$%&'*+/0-9=?A-Z^_a-z{|}~])*@[a-zA-Z](-?[a-zA-Z0-9])*(\.[a-zA-Z](-?[a-zA-Z0-9])*)+$");

            if (organizer != "")
            {
                if (!rx.IsMatch(organizer))
                {
                    ErrorMsg += "Invalid email format!\n ";
                }
            }

            foreach(string email in emails)
            {
                if (!rx.IsMatch(email))
                {
                    ErrorMsg += "Invalid email format!\n ";
                }
            }
        }

        /***********************************************************************************************
         * Trims off milliseconds from date time
        ***********************************************************************************************/
        private static DateTime TrimTime(DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, 0, dt.Kind);
        }
    }
}
