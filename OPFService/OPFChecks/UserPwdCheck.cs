using System;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Runtime.Remoting.Messaging;

namespace OPFService
{
    internal class UserPwdCheck : IUserPwdCheck
    {
        public bool IsEnabled() => ApplicationConfiguration.Instance.PwdNotIncludingUserData;

        public bool IsInvalid(UserPrincipal user, string password)
        {
            String passwordCleaned = password.ToLower();
            String passwordCleanedSub = passwordCleaned;

            if (ApplicationConfiguration.Instance.PwdNotIncludingUserDataCommonSubstitution)
            {
                passwordCleanedSub = passwordCleanedSub
                    .Replace("1", "i")
                    .Replace("!", "i")
                    .Replace("|", "i")
                    .Replace("0", "o")
                    .Replace("4", "a")
                    .Replace("3", "e")
                    .Replace("5", "s")
                    .Replace("7", "t")
                    .Replace("9", "g")
                    .Replace("@", "a")
                    .Replace("$", "s")
                    .Replace("€", "e")
                    .Replace("£", "l");
            }

            if (CheckForForbiddenWords(passwordCleaned, passwordCleanedSub, user.Description))
                return true;
            if (CheckForForbiddenWords(passwordCleaned, passwordCleanedSub, user.DisplayName))
                return true;
            if (CheckForForbiddenWords(passwordCleaned, passwordCleanedSub, user.EmailAddress))
                return true;
            if (CheckForForbiddenWords(passwordCleaned, passwordCleanedSub, user.GivenName))
                return true;
            if (CheckForForbiddenWords(passwordCleaned, passwordCleanedSub, user.MiddleName))
                return true;
            if (CheckForForbiddenWords(passwordCleaned, passwordCleanedSub, user.UserPrincipalName))
                return true;
            if (CheckForForbiddenWords(passwordCleaned, passwordCleanedSub, user.SamAccountName))
                return true;
            if (CheckForForbiddenWords(passwordCleaned, passwordCleanedSub, user.Surname))
                return true;

            return false;
        }

        private bool CheckForForbiddenWords(string passwordCleaned, string passwordCleanedSub, string userDetailToCheck)
        {
            if (userDetailToCheck == null)  
                return false;

            String[] words = userDetailToCheck.ToLower().Split(' ');

            foreach (String word in words)
            {
                if (passwordCleaned.Contains(word))
                    return true;

                if (passwordCleaned != passwordCleanedSub && passwordCleanedSub.Contains(word))
                    return true;
            }

            return false;
        }
    }
}
