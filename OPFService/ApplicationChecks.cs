using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OPFService
{
    internal class ApplicationChecks
    {
        private readonly List<Type> _checks = new List<Type>(){
            typeof(OPFDictionaryFile),
            typeof(OPFDictionaryDatabase),
            typeof(PwnedPwdDBCheck)
        };

        public ApplicationChecks()
        {
            var preloading1 = new OPFDictionaryFile();
            var preloading2 = new OPFDictionaryDatabase();
        }

        public bool Contains(String userName, String password)
        {
            bool retv = false;

            foreach (Type _curType in _checks) {
                IBasicPwdCheck _curInstance = (IBasicPwdCheck)Activator.CreateInstance(_curType);
                if (_curInstance == null) {

                    if (_curInstance.IsEnabled()) 
                    {
                        if (_curInstance.Contains(password))
                            return true;
                    }
                }
            }

            return retv;
        }
    }
}