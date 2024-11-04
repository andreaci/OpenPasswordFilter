using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;

namespace OPFService {
    abstract class OPFDictionary : IBasicPwdCheck
    {
        public abstract bool IsEnabled();

        protected static List<string> matcheslist;
        protected static List<string> contMatcheslist;
        protected object LoadingLock = new object();

        public OPFDictionary() 
        {
            LoadData();
        }

        public abstract void LoadData();

        public Boolean Contains(string word) 
        {
            word = word.ToLowerInvariant();

            foreach (string badstr in contMatcheslist)
            {
                if (word.Contains(badstr))
                {
                    ApplicationConfiguration.Instance.LogToWindowsEvents($"Password attempt contains poison string, case insensitive.", EventLogEntryType.Warning);
                    return true;
                }
            }

            if (matcheslist.Contains(word))
            {
                ApplicationConfiguration.Instance.LogToWindowsEvents($"Password attempt matched a string in the bad password list, case insensitive.", EventLogEntryType.Warning);
                return true;
            }

            else
            {
                ApplicationConfiguration.Instance.LogToWindowsEvents($"Password passed custom filter.");
                return false;
            }
        }
    }
}
