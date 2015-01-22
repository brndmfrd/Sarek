using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Sarek.AppLog
{
    public class MessageLogging
    {
        private string report_message;
        private StreamWriter logfile;
        private string log_filename;
        private string title;


        // Method used when we just want to delete the old log file
        public MessageLogging()
        {
            log_filename = "events.log";
        }//-- end MessageLogging()

        public MessageLogging(string source_sect)
        {
            title = source_sect;
            log_filename = "events.log";
            logfile = new StreamWriter(log_filename, true, UTF8Encoding.Default);
            logfile.WriteLine("==================== " + title + " Open Section ====================");
        }


        public void AddMessage(string msg)
        {
            report_message = System.DateTime.Now.ToString() + ": " + msg;
            logfile.WriteLine(report_message);
        }//-- end AddMessage()


        public void CloseLog()
        {
            logfile.WriteLine("==================== " + title + " Close Section ====================");
            logfile.Close();
        }//-- end CloseLog()


        public void Clear()
        {
            if (File.Exists(log_filename))
            {
                File.Delete(log_filename);
            }
        }//-- end Clear()


    }//-- end class MessageLogging
}
