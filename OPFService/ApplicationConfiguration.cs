using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace OPFService
{
    internal class ApplicationConfiguration
    {
        private static ApplicationConfiguration instance;   
        public static ApplicationConfiguration Instance => instance ?? (instance = new ApplicationConfiguration());


        private SqlConnection connection;
        private readonly Logger logger = null;

        public String PwnedPwdDBTableName { get; private set; }
        public String ClientRecognitionKey { get; private set; }
        private String DatabaseConnectionString { get; set; }
        private String LogFilePath { get; set; }
        private String LogEventSource { get; set; }

        public Boolean UsePwnedPwd { get; private set; }
        public Boolean LoggingEnabled { get; private set; }


        public Boolean UseForbiddenList { get; private set; }
        public Boolean ForbiddenListInDatabase { get; private set; }
        public String MatchesDatabaseTableName { get; private set; }
        public String ContMatchesDatabaseTableName { get; private set; }
        public String MatchFile { get; private set; }
        public String ContMatchFile { get; private set; }  


        public ApplicationConfiguration()
        {
            ClientRecognitionKey = ConfigurationManager.AppSettings["OPFClientRecognitionKeyword"];
            DatabaseConnectionString = ConfigurationManager.AppSettings["DatabaseConnectionString"];

            UsePwnedPwd = Convert.ToBoolean(ConfigurationManager.AppSettings["UsePwnedPwd"]);
            PwnedPwdDBTableName = ConfigurationManager.AppSettings["PwnedPwdDatabaseTableName"];
            
            UseForbiddenList = Convert.ToBoolean(ConfigurationManager.AppSettings["UseForbiddenList"]);
            ForbiddenListInDatabase = Convert.ToBoolean(ConfigurationManager.AppSettings["ForbiddenListInDatabase"]);
            MatchesDatabaseTableName = ConfigurationManager.AppSettings["MatchesDatabaseTableName"];
            ContMatchesDatabaseTableName = ConfigurationManager.AppSettings["ContMatchesDatabaseTableName"];
            MatchFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["MatchFile"]);
            ContMatchFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["ContMatchFile"]);

            LoggingEnabled = Convert.ToBoolean(ConfigurationManager.AppSettings["LoggingEnabled"]);
            LogFilePath = ConfigurationManager.AppSettings["LogFilePath"];
            LogEventSource = ConfigurationManager.AppSettings["LogEventSource"];

            if (LoggingEnabled)
                logger = new Logger(LogFilePath);
        }

        public Boolean InitConnection()
        {
            connection = new SqlConnection(DatabaseConnectionString);

            try
            {
                connection.Open();
                return true;
            }
            catch (Exception e)
            {
                LogException(e);
                return false;
            }

        }

        public void CloseConnection()
        {
            try
            {
                connection.Close();
            }
            catch (Exception e)
            {
                LogException(e);
            }
            connection.Dispose();

        }

        internal void LogException(Exception e)
        {
            if(LoggingEnabled)
                logger.logException(e);
        }
        internal void LogToWindowsEvents(String message, EventLogEntryType logType = EventLogEntryType.Information)
        {
            using (EventLog eventLog = new EventLog("Application"))
            {
                eventLog.Source = LogEventSource;
                eventLog.WriteEntry(message, logType, 101, 1);
            }
        }
    
        internal SqlConnection GetConnection()
        {
            return connection;
        }
    }
}
