using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace OPFService
{
    internal class OPFDictionaryFile : OPFDictionary
    {
        public override bool IsEnabled() => ApplicationConfiguration.Instance.UseForbiddenList && !ApplicationConfiguration.Instance.ForbiddenListInDatabase;
        
        public override void LoadData()
        {
            lock (LoadingLock)
            {
                if (matcheslist == null)
                {
                    String[] lines = File.ReadAllLines(ApplicationConfiguration.Instance.MatchFile);
                    matcheslist = new List<string>(lines.Length);
                    foreach (String itm in lines)
                    {
                        matcheslist.Add(itm.ToLowerInvariant());
                    }
                }

                if (contMatcheslist == null)
                {
                    String[] lines = File.ReadAllLines(ApplicationConfiguration.Instance.ContMatchFile);
                    contMatcheslist = new List<string>(lines.Length);
                    foreach (String itm in lines)
                    {
                        contMatcheslist.Add(itm.ToLowerInvariant());
                    }
                }
            }
        }
    }
}
