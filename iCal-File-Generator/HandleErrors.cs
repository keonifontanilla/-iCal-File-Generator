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
        private static string errMsg = "";

        public static string ErrorMsg { get; private set; }

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
            if (string.IsNullOrWhiteSpace(input))
            {
                errMsg = "Cannot leave title blank!";
            }
            else
            {
                errMsg = "";
            }
            ErrorMsg += errMsg;
        }

        /***********************************************************************************************
         * Handles empty required inputs
        ***********************************************************************************************/
        public static void HandleTimeError(DateTimePicker startDatePicker, DateTimePicker startTimePicker)
        {
            if (startTimePicker.Value.TimeOfDay < startDatePicker.Value.TimeOfDay && startTimePicker.Value.Date == startDatePicker.Value.Date)
            {
                errMsg = "Cannot pick time in the past!";
            }
            else
            {
                errMsg = "";
            }
            ErrorMsg += errMsg;
        }
    }
}
