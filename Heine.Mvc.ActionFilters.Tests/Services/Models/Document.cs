using System.Collections.Generic;
using System.Reflection;

namespace Heine.Mvc.ActionFilters.Tests.Services.Models
{
    public class Document
    {
        public ICollection<Document> Children { get; set; }
        public List<Person> Persons { get; set; }
        public string Name { get; set; }
        [Obfuscation]
        public string Content { get; set; }
        public string Type { get; set; }
        public Person Owner { get; set; }
        public Document Parent { get; set; }
    }
}