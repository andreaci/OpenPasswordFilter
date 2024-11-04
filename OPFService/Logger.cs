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


        private void writeErrorToLog(Exception errorException)
        {


            try
            {
                using (StreamWriter writer = new StreamWriter(logFilePath, true))
                {
                    writer.WriteLine(
                        DateTime.Now.ToString() + ": " +
                        errorException.Message + Environment.NewLine +
                        "StackTrace :" + errorException.StackTrace +
                        Environment.NewLine);
                }

            }
            catch (Exception e)
            {

            }

        }

    }
}