using System;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace Heine.Mvc.ActionFilters
{
    public static class StringExtensions
    {
        public static string PrettyPrint(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            try
            {
                return XDocument.Parse(input).ToString();
            }
            catch(Exception)
            {
                // ignored
            }

            try
            {
                var t = JsonConvert.DeserializeObject<object>(input);
                return JsonConvert.SerializeObject(t, Formatting.Indented);
            }
            catch (Exception)
            {
                // ignored
            }

            return input;
        }
    }
}
