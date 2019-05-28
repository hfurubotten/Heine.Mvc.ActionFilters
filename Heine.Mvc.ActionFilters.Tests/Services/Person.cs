using System.Collections.Generic;
using System.Reflection;

namespace Heine.Mvc.ActionFilters.Tests.Services
{
    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Obfuscation]
        public string Mobil { get; set; }
        [Obfuscation]
        public string BankAccNo { get; set; }
        public List<Document> Documents { get; set; }
    }
}