using System;
using System.Collections.Generic;
using System.IO;

namespace iCal_File_Generator
{
    public class FileGenerator
    {
        private static string path = $@"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\test.txt";

        /***********************************************************************************************
         * Write input and generate an ics file
        ***********************************************************************************************/
        private void GenerateFile(List<string> formatedInputs)
        {
            using(StreamWriter streamWriter = new StreamWriter(path, false))
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
            newEvent.GetTimezoneOffset();

            newInputs.Add("BEGIN:VCALENDAR");
            newInputs.Add("VERSION:2.0");
            newInputs.Add("PRODID:-//ics-file-generator//iCal File Generator");
            newInputs.Add("BEGIN:VTIMEZONE");
            newInputs.Add($"TZID:{newEvent.GetTZID()}");
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
                        formatedStr = Foldline($"{str.Key}:{str.Value}").Replace("\r\n", "\\n");
                        newInputs.Add(formatedStr);
                        continue;
                    case "DTSTART":
                    case "DTEND":
                    case "DTSTAMP":
                        formatedStr = Foldline($"{str.Key}:{FormatTime(str.Value)}");
                        newInputs.Add(formatedStr);
                        continue;
                }
                formatedStr = Foldline($"{str.Key}:{str.Value}");
                newInputs.Add(formatedStr);
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
