using System.Collections.Generic;
using System.Reflection;

namespace Heine.Mvc.ActionFilters.Tests.Services.Models
{
    public class Document
    {
        public string Name { get; set; }
        [Obfuscation]
        public string Content { get; set; }
        public string Type { get; set; }
        public List<Person> Persons { get; set; }
        public Person Owner { get; set; }
        public Document Parent { get; set; }
    }
}