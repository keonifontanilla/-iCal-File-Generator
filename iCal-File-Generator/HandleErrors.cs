using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
         * Trims off milliseconds from date time
        ***********************************************************************************************/
        private static DateTime TrimTime(DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, 0, dt.Kind);
        }
    }
}
