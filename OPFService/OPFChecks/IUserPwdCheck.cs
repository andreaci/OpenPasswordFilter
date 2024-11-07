using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;

namespace OPFService
{
    internal interface IUserPwdCheck
    {
        Boolean IsEnabled();
        Boolean IsInvalid(UserPrincipal user, string password);
    }
}
