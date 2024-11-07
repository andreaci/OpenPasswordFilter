using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;


namespace OPFService
{
    internal class ApplicationChecks
    {

        // order matters!
        // put "lighter" checks at the beginning of the list
        private readonly List<Type> _userChecks = new List<Type>(){
            typeof(UserPwdCheck)
        };

        private readonly List<Type> _basicChecks = new List<Type>(){
            typeof(OPFMinLength),
            typeof(OPFMaxCharRepetition),
            typeof(OPFDictionaryFile),
            typeof(OPFDictionaryDatabase),
            typeof(PwnedPwdDBCheck)
        };
        public ApplicationChecks()
        {
            var preloading1 = new OPFDictionaryFile();
            var preloading2 = new OPFDictionaryDatabase();
            Console.WriteLine("Preloading complete");
        }

        public bool CheckPasswordRequirements(String userName, String password, out String failedCheck)
        {
            bool userDataLoaded = false;
            UserPrincipal DirectoryData = null;

            foreach (Type _curType in _userChecks)
            {
                failedCheck = _curType.GetType().FullName;

                IUserPwdCheck _curInstance = (IUserPwdCheck)Activator.CreateInstance(_curType);
                if (_curInstance != null)
                {
                    if (_curInstance.IsEnabled())
                    {
                        if (!userDataLoaded)
                        {
                            try
                            {
                                userDataLoaded = true;
                                DirectoryData = GetUserData(userName);
                            }
                            catch (Exception ex)
                            {
                                ApplicationConfiguration.Instance.LogException(ex, "GetUserData");
                            }
                        }

                        if (DirectoryData != null)
                        {
                            if (_curInstance.IsInvalid(DirectoryData, password))
                                return true;
                        }
                    }
                }
            }

            foreach (Type _curType in _basicChecks) 
            {
                failedCheck = _curType.GetType().FullName;

                IBasicPwdCheck _curInstance = (IBasicPwdCheck)Activator.CreateInstance(_curType);
                if (_curInstance != null) {

                    if (_curInstance.IsEnabled()) 
                    {
                        if (_curInstance.IsInvalid(password))
                            return true;
                    }
                }
            }

            failedCheck = "";
            return false;
        }

        private UserPrincipal GetUserData(string userName)
        {
            String domainName = ApplicationConfiguration.Instance.DomainNameForQueries;
            using (var pc = new PrincipalContext(ContextType.Domain, domainName))
            {
                UserPrincipal user = UserPrincipal.FindByIdentity(pc, IdentityType.SamAccountName, $"{domainName}\\{userName}");
                return user;
            }

        }
    }
}