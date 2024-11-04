using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;

namespace OPFService
{
    class PwnedPwdDBCheck : IBasicPwdCheck
    {

        public Boolean Contains(string hash)
        {
            ApplicationConfiguration configuration = ApplicationConfiguration.Instance;

            if (!configuration.InitConnection())
                return false;

            try
            {
                SqlCommand command = new SqlCommand($"Select hash from {configuration.PwnedPwdDBTableName} where hash='{Hash(hash)}'", ApplicationConfiguration.Instance.GetConnection());

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    bool result = reader.Read();
                    configuration.CloseConnection();
                    return result;
                }
            }
            catch (Exception e)
            {
                configuration.LogException(e);
                configuration.CloseConnection();
                return true;
            }

        }

        static string Hash(string input)
        {
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
                var sb = new StringBuilder(hash.Length * 2);

                foreach (byte b in hash)
                {
                    sb.Append(b.ToString("X2"));
                }

                return sb.ToString();
            }
        }

        public bool IsEnabled() => ApplicationConfiguration.Instance.UsePwnedPwd;
    }
}