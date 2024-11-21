using System;
using System.Text;

namespace MusicRequests.Core.Helpers
{
    public static class StringExtensions
    {
        public static string ToCamelCaseText(this string str, char separator = ' ')
        {
            StringBuilder sb = new StringBuilder();
            char[] separators = { separator };
            if (!string.IsNullOrEmpty(str))
            {
                foreach (string word in str?.Split(separators, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (word.Length > 1)
                    {
                        sb.Append(word.Substring(0, 1).ToUpper());
                        sb.Append(word.Substring(1).ToLower() + separator);
                    }
                    else
                    {
                        sb.Append(word + separator);
                    }
                }
            }

            return sb.ToString().Trim();
        }
    }
}
