using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;

namespace OPFService
{
    class Logger
    {
        private string logFilePath;

        public Logger(string logFilePath)
        {
            this.logFilePath = logFilePath;
        }

        public void logException(Exception e) => writeErrorToLog(e);

        internal void logEntry(string entry)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(logFilePath, true))
                {
                    writer.WriteLine($"{DateTime.Now}: {entry}");
                }

            }
            catch (Exception e)
            {

            }
        }

        private void writeErrorToLog(Exception errorException)
        {
            logEntry($"{errorException.Message}\nStackTrace: {errorException.StackTrace}\n");

        }
   }
}