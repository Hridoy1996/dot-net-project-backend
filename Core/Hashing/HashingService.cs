using System.Text;
using XSystem.Security.Cryptography;

namespace Infrastructure.Core.Hashing
{
    public static class HashingService
    {
        public static string GetHashString(Dictionary<string, string> keyValues)
        {
            StringBuilder sha1StringBuilder = new StringBuilder();

            foreach (var valuePair in keyValues.OrderBy(key => key.Key))
            {
                if (!string.IsNullOrEmpty(valuePair.Value))
                {
                    sha1StringBuilder.Append($"{valuePair.Key}={valuePair.Value}");
                }
            }

            var hash1 = new SHA1CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(sha1StringBuilder.ToString()));

            return string.Concat(hash1.Select(b => b.ToString("x2")));
        }

    }
}
