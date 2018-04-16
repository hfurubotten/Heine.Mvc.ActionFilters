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
    }
}