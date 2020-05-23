using Microsoft.SqlServer.Server;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
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
         * Write input from form to text file
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
         * Format input
        ***********************************************************************************************/
        public void FormatInput(Event newEvent)
        {
            Dictionary<string, string> inputs = new Dictionary<string, string>();
            List<string> newInputs = new List<string>();
            string formatedStr = "";

            inputs.Add("TITLE", newEvent.summary);
            inputs.Add("DESCRIPTION", newEvent.description);

            newInputs.Add("BEGIN:VCALENDAR");
            newInputs.Add("BEGIN:VEVENT");
            foreach (KeyValuePair<string, string> str in inputs)
            {
                formatedStr = Foldline($"{str.Key}:{str.Value}");
                newInputs.Add(formatedStr);
            }
            newInputs.Add("END:VCALENDAR");
            newInputs.Add("END:VEVENT");
            GenerateFile(newInputs);
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
