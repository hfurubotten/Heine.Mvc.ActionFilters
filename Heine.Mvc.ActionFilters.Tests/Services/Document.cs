using System.Collections.Generic;
using System.Reflection;

namespace Heine.Mvc.ActionFilters.Tests.Services
{
    public class Document
    {
        public string Name { get; set; }
        [Obfuscation]
        public string Content { get; set; }
        public string Type { get; set; }
        public Person Owner { get; set; }
        public List<Person> Persons { get; set; }
    }
}