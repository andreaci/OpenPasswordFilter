using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;

namespace OPFService
{
    internal class OPFDictionaryDatabase : OPFDictionary
    {
        public override bool IsEnabled() => ApplicationConfiguration.Instance.UseForbiddenList && ApplicationConfiguration.Instance.ForbiddenListInDatabase;

        public override void LoadData()
        {
            lock (LoadingLock)
            {
                if (matcheslist == null || contMatcheslist == null)
                {
                    ApplicationConfiguration configuration = ApplicationConfiguration.Instance;

                    if (!configuration.InitConnection())
                    {
                        configuration.LogToWindowsEvents("Database unavailable", EventLogEntryType.Error);
                        throw new Exception("Database unavailable");
                    }

                    try
                    {
                        SqlCommand command = new SqlCommand($"Select match from [{configuration.MatchesDatabaseTableName}]", ApplicationConfiguration.Instance.GetConnection());
                        matcheslist = new List<string>(1000);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                matcheslist.Add((String)reader[0]);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        configuration.LogToWindowsEvents($"Matches database loading error: {ex.Message}", EventLogEntryType.Error);
                        throw new Exception($"Matches database loading error: {ex.Message}");
                    }


                    try
                    {
                        SqlCommand command = new SqlCommand($"Select match from [{configuration.ContMatchesDatabaseTableName}]", ApplicationConfiguration.Instance.GetConnection());
                        contMatcheslist = new List<string>(1000);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                contMatcheslist.Add((String)reader[0]);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        configuration.LogToWindowsEvents($"ContMatches database loading error: {ex.Message}", EventLogEntryType.Error);
                        throw new Exception($"ContMatches database loading error: {ex.Message}");
                    }


                    configuration.CloseConnection();
                }
            }
        }
    }
}
