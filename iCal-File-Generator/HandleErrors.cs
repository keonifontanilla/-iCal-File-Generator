using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace iCal_File_Generator
{
    /// <summary>
    /// This class handles form errors.
    /// </summary>
    public static class HandleErrors
    {
        public static string ErrorMsg { get; private set; }

        /// <summary>
        /// Display error messages.
        /// </summary>
        public static void DisplayErrorMsg()
        {
            MessageBox.Show(ErrorMsg);
            ErrorMsg = "";
        }

        /// <summary>
        /// Handles title errors.
        /// </summary>
        /// <param name="input"></param>
        public static void HandleTitleError(string input)
        {
            ErrorMsg += string.IsNullOrWhiteSpace(input) ? "Cannot leave title blank!\n" : "";
        }

        /// <summary>
        /// Handles date and time errors.
        /// </summary>
        /// <param name="startDatePicker">The start date.</param>
        /// <param name="startTimePicker">The start time.</param>
        /// <param name="endTimePicker">The end time.</param>
        /// <param name="endDatePicker">The end date.</param>
        /// <param name="dateNow">The current date.</param>
        /// <param name="recurTime">The recurrence time.</param>
        public static void HandleTimeError(DateTimePicker startDatePicker, DateTimePicker startTimePicker, DateTimePicker endTimePicker, DateTimePicker endDatePicker, DateTime dateNow, DateTime recurTime)
        {
            // Check start time errors
            ErrorMsg += (startTimePicker.Value.TimeOfDay < startDatePicker.Value.TimeOfDay) && (startTimePicker.Value.Date == startDatePicker.Value.Date)
                ? "Cannot pick time in the past!\n" : "";
            // Check end time errors
            ErrorMsg += (TrimTime(endTimePicker.Value).TimeOfDay < TrimTime(startTimePicker.Value).TimeOfDay) && (endDatePicker.Value.Date == startDatePicker.Value.Date)
                ? "Cannot have end time before start time!\n" : "";
            // Check update date error
            ErrorMsg += (startDatePicker.Value.Date < dateNow.Date) && (endDatePicker.Value.Date < dateNow.Date) 
                ? "Update the date!\n" : "";
            // Check recurrence date error
            ErrorMsg += recurTime.Date < startDatePicker.Value.Date
                ? "Update the recurrence date!\n" : "";
        }

        /// <summary>
        /// Handles invalid email inputs.
        /// </summary>
        /// <param name="emails">A list of attendee emails.</param>
        /// <param name="organizer">The organizer's email.</param>
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

        /// <summary>
        /// Trims off milliseconds from date time.
        /// </summary>
        /// <param name="dt">The date and time.</param>
        /// <returns>Returns a formated date and time.</returns>
        private static DateTime TrimTime(DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, 0, dt.Kind);
        }
    }
}
