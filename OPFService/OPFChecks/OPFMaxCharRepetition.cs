using System.Linq;

namespace OPFService
{
    internal class OPFMaxCharRepetition : IBasicPwdCheck
    {
        public bool IsInvalid(string password)
        {
            int longestRun =
                password.Select((c, index) => password.Substring(index).TakeWhile(e => e == c))
                    .OrderByDescending(e => e.Count())
                                   .First().Count();

            return longestRun > ApplicationConfiguration.Instance.PwdMaxSameCharRepetition;
        }

        public bool IsEnabled() => ApplicationConfiguration.Instance.PwdMaxSameCharRepetition > 0;
    }
}
