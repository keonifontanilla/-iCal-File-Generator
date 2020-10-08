using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace iCal_File_Generator
{
    public class FileGenerator
    {
        SaveFileDialog saveFileDialog;

        public FileGenerator(SaveFileDialog saveFileDialog)
        {
            this.saveFileDialog = saveFileDialog;
        }

        /***********************************************************************************************
         * Write input and generate an ics file
        ***********************************************************************************************/
        private void GenerateFile(List<string> formatedInputs)
        {
            using(StreamWriter streamWriter = new StreamWriter(saveFileDialog.FileName, false))
            {
                foreach(string str in formatedInputs)
                {
                    streamWriter.WriteLine(str);
                }
            }
        }

        /***********************************************************************************************
         * Format input from user
        ***********************************************************************************************/
        public void FormatInput(Event newEvent)
        {
            List<string> newInputs = new List<string>();            
            string formatedStr = "";
            newEvent.GetTimeZoneOffset();

            newInputs.Add("BEGIN:VCALENDAR");
            newInputs.Add("VERSION:2.0");
            newInputs.Add("PRODID:-//ics-file-generator//iCal File Generator");
            newInputs.Add("BEGIN:VTIMEZONE");
            newInputs.Add($"TZID:{newEvent.tzid}");
            newInputs.Add("BEGIN:STANDARD");
            newInputs.Add("DTSTART:19981025T020000");
            newInputs.Add($"TZOFFSETFROM:{newEvent.tzOffSetFrom}");
            newInputs.Add($"TZOFFSETTO:{newEvent.tzOffSetTo}");
            newInputs.Add("END:STANDARD");
            newInputs.Add("BEGIN:DAYLIGHT");
            newInputs.Add("DTSTART:19981025T020000");
            newInputs.Add($"TZOFFSETFROM:{newEvent.tzOffSetTo}");
            newInputs.Add($"TZOFFSETTO:{newEvent.tzOffSetFrom}");
            newInputs.Add("END:DAYLIGHT");
            newInputs.Add("END:VTIMEZONE");
            newInputs.Add("BEGIN:VEVENT");

            foreach (KeyValuePair<string, string> str in newEvent.GetInputs())
            {
                switch(str.Key)
                {
                    case "DESCRIPTION":
                        if (str.Value == "") { continue; }
                        formatedStr = Foldline(($"{str.Key}:{str.Value}").Replace("\r\n", "\\n"));
                        newInputs.Add(formatedStr);
                        continue;
                    case "DTSTART":
                    case "DTEND":
                        formatedStr = Foldline($"{str.Key};TZID={newEvent.tzid}:{FormatTime(str.Value)}");
                        newInputs.Add(formatedStr);
                        continue;
                    case "DTSTAMP":
                        formatedStr = Foldline($"{str.Key}:{FormatTime(str.Value)}");
                        newInputs.Add(formatedStr);
                        continue;
                    case "ORGANIZER":
                        if (str.Value == "") { continue; }
                        formatedStr = Foldline($"{str.Key};SENT-BY=\"mailto:{str.Value}\":mailto:{str.Value}");
                        newInputs.Add(formatedStr);
                        continue;
                    case "RRULE":
                        string getMonthDay = newEvent.startTime.Substring(8, 1) == "0" ? newEvent.startTime.Substring(9, 1) : newEvent.startTime.Substring(8, 2);
                        string getMonth = newEvent.startTime.Substring(5, 1) == "0" ? newEvent.startTime.Substring(6, 1) : newEvent.startTime.Substring(5, 2);
                        if (newEvent.recurUntil == "")
                        {
                            if (str.Value == "Daily")
                            {
                                formatedStr = Foldline($"{str.Key}:FREQ={str.Value.ToUpper()}");
                                newInputs.Add(formatedStr);
                            }
                            else if (str.Value == "Weekly")
                            {
                                formatedStr = Foldline($"{str.Key}:FREQ={str.Value.ToUpper()}");
                                newInputs.Add(formatedStr);
                            }
                            else if (str.Value == "Monthly")
                            {
                                formatedStr = Foldline($"{str.Key}:FREQ={str.Value.ToUpper()};BYMONTHDAY={getMonthDay}");
                                newInputs.Add(formatedStr);
                            }
                            else if (str.Value == "Yearly")
                            {
                                formatedStr = Foldline($"{str.Key}:FREQ={str.Value.ToUpper()};BYMONTH={getMonth};BYMONTHDAY={getMonthDay}");
                                newInputs.Add(formatedStr);
                            }
                        }
                        else
                        {
                            if (str.Value == "Daily")
                            {
                                formatedStr = Foldline($"{str.Key}:FREQ={str.Value.ToUpper()};UNTIL={FormatTime(newEvent.recurUntil)}Z");
                                newInputs.Add(formatedStr);
                            }
                            else if (str.Value == "Weekly")
                            {
                                formatedStr = Foldline($"{str.Key}:FREQ={str.Value.ToUpper()};UNTIL={FormatTime(newEvent.recurUntil)}Z");
                                newInputs.Add(formatedStr);
                            }
                            else if (str.Value == "Monthly")
                            {
                                formatedStr = Foldline($"{str.Key}:FREQ={str.Value.ToUpper()};UNTIL={FormatTime(newEvent.recurUntil)}Z;BYMONTHDAY={getMonthDay}");
                                newInputs.Add(formatedStr);
                            }
                            else if (str.Value == "Yearly")
                            {
                                formatedStr = Foldline($"{str.Key}:FREQ={str.Value.ToUpper()};UNTIL={FormatTime(newEvent.recurUntil)}Z;BYMONTH={getMonth};BYMONTHDAY={getMonthDay}");
                                newInputs.Add(formatedStr);
                            }
                        }
                        continue;
                }
                if (str.Value == "") { continue; }
                formatedStr = Foldline($"{str.Key}:{str.Value}");
                newInputs.Add(formatedStr);
            }

            if (newEvent.attendees != null)
            {
                for (int i = 0; i < newEvent.attendees.Count; i++)
                {
                    if (newEvent.attendees[i] == "") { continue; }
                    newInputs.Add(Foldline($"ATTENDEE;PARTSTAT=NEEDS-ACTION;ROLE=REQ-PARTICIPANT;RSVP={newEvent.attendeesRsvp[i]}:mailto:{newEvent.attendees[i]}"));
                }
            }

            newInputs.Add("END:VEVENT");
            newInputs.Add("END:VCALENDAR");
            GenerateFile(newInputs);
        }

        /***********************************************************************************************
         * Format time to the proper ics file format
        ***********************************************************************************************/
        private string FormatTime(string dateTime)
        {
            dateTime = dateTime.Replace("/","");
            dateTime = dateTime.Replace(" ", "");
            dateTime = dateTime.Replace(":", "");

            return dateTime.Contains(".") ? dateTime.Insert(8, "T").Substring(0, dateTime.LastIndexOf(".") + 1) : dateTime.Insert(8, "T");
        }

        /***********************************************************************************************
        * Lines of text should not be longer that 75 octets. 
        * This function splits long content lines in to multiple lines 
        ***********************************************************************************************/
        private string Foldline(string line)
        {
            List<string> lines = new List<string>();
            int length = 75;

            while(line.Length > length)
            {
                lines.Add(line.Substring(0, length));
                line = line.Substring(length);
                length = 74;
            }
            lines.Add(line);

            return string.Join("\r\n ", lines);
        }
    }
}
