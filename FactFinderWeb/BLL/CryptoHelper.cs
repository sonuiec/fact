using System;
using System.Text;

namespace FactFinderWeb.BLL
{
    public static class CryptoHelper
    {
        private const long Key = 987654321; // secret key

        private const string Alphabet = "0123456789abcdefghijklmnopqrstuvwxyz";

        public static string Encrypt(long number)
        {
            long encrypted = number ^ Key;
            return ToBase36(encrypted);
        }

        public static long Decrypt(string input)
        {
            long encrypted = FromBase36(input);
            return encrypted ^ Key;
        }

        private static string ToBase36(long value)
        {
            if (value == 0) return "0";

            var sb = new StringBuilder();
            long target = Math.Abs(value);

            while (target > 0)
            {
                sb.Insert(0, Alphabet[(int)(target % 36)]);
                target /= 36;
            }

            return sb.ToString();
        }

        private static long FromBase36(string input)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentNullException(nameof(input));

            input = input.ToLowerInvariant();
            long result = 0;

            foreach (char c in input)
            {
                int val = Alphabet.IndexOf(c);
                if (val == -1)
                    throw new ArgumentException("Invalid base36 character", nameof(input));

                result = result * 36 + val;
            }

            return result;
        }
    }
}
