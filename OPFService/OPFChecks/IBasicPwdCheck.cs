using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OPFService
{
    internal interface IBasicPwdCheck
    {
        Boolean IsEnabled();
        Boolean IsInvalid(string password);
    }
}
