using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPFService
{
    internal class OPFMinLength : IBasicPwdCheck
    {
        public bool IsInvalid(string password)
        {
            return password.Length < ApplicationConfiguration.Instance.PwdMinLength;
        }

        public bool IsEnabled() => ApplicationConfiguration.Instance.PwdMinLength > 0;
    }
}
