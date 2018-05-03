using System;

namespace Heine.Mvc.ActionFilters.Extensions
{
    public static class StringExtensions
    {
        public static string ReplaceEnd(this string source, char c, float fraction)
        {
            if (string.IsNullOrWhiteSpace(source)) return source;
            if (source.Length == 1) return new string(c, 1);
            if (fraction <= 0 || fraction > 1) throw new ArgumentException("The value must be greater than zero (0) and less or equal to one (1).", nameof(fraction));

            var lowFraction = source.Length * (1 - fraction);
            var highFraction = source.Length * fraction;

            var low = (int)Math.Round(lowFraction);
            var high = (int)Math.Round(highFraction);

            return source.Substring(0, low) + new string(c, high);
        }

        /// <summary>
        ///     Convert the string to camel case.
        /// </summary>
        /// <param name="string"></param>
        /// <returns></returns>
        public static string ToCamelCase(this string @string)
        {
            // If there are 0 or 1 characters, just return the string.
            if (@string == null || @string.Length < 2)
                return @string;

            // Split the string into words.
            var words = @string.Split(
                new char[] { },
                StringSplitOptions.RemoveEmptyEntries);

            // Combine the words.
            var result = words[0].FirstLetterToLower();
            for (var i = 1; i < words.Length; i++)
            {
                result +=
                    words[i].Substring(0, 1).ToUpper() +
                    words[i].Substring(1);
            }

            return result;
        }

        /// <summary>
        ///     Turns the first character in any string to lower case.
        /// </summary>
        /// <param name="source">The string to convert the casing on.</param>
        /// <returns>The new string with the changed casing.</returns>
        public static string FirstLetterToLower(this string source)
        {
            if (source == null)
                return null;

            if (source.Length > 1)
                return char.ToLower(source[0]) + source.Substring(1);

            return source.ToUpper();
        }
    }
}