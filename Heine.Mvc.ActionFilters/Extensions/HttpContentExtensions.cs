﻿using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;

namespace Heine.Mvc.ActionFilters.Extensions
{
    public static class HttpContentExtensions
    {

        public static string ReadAsString(this HttpContent httpContent)
        {
            if (httpContent == null) return string.Empty;

            string content;

            try
            {
                var contentStream = httpContent.ReadAsStreamAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                contentStream.Position = 0;
                var streamReader = new StreamReader(contentStream, Encoding.UTF8);
                content = streamReader.ReadToEnd();
                contentStream.Position = 0;
            }
            catch (NotSupportedException e)
            {
                content = $"Unable to read body of HTTP content:\n{string.Join("\n", e.GetMessages().Select(message => $"- '{message}'"))}";
            }

            return content;
        }

        internal static object ReadContent(this HttpContent httpContent)
        {
            var body = httpContent?.ReadAsString();

            switch (httpContent?.Headers?.ContentType?.MediaType)
            {
                case "application/json":
                    try { return JToken.Parse(body); }
                    catch { return body; }
                case "application/xml":
                    try { return XDocument.Parse(body); }
                    catch { return body; }
            }

            return body;
        }
    }
}