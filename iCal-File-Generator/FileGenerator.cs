using Microsoft.SqlServer.Server;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            newInputs.Add("BEGIN:VCALENDAR");
            newInputs.Add("BEGIN:VEVENT");
            foreach (KeyValuePair<string, string> str in newEvent.GetInputs())
            {
                switch(str.Key)
                {
                    case "DTSTART":
                    case "DTEND":
                        formatedStr = Foldline($"{str.Key}:{FormatTime(str.Value)}");
                        newInputs.Add(formatedStr);
                        continue;
                }
                formatedStr = Foldline($"{str.Key}:{str.Value}");
                newInputs.Add(formatedStr);
            }
            newInputs.Add("END:VCALENDAR");
            newInputs.Add("END:VEVENT");
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
