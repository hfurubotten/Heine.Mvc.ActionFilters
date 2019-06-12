using System.Collections.Generic;
using System.Reflection;

namespace Heine.Mvc.ActionFilters.Tests.Services.Models
{
    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<Document> Documents { get; set; }
        public Document[] PrivateDocuments { get; set; }
        [Obfuscation]
        public string Mobil { get; set; }
        [Obfuscation]
        public string BankAccNo { get; set; }
        [Obfuscation]
        public CreditDetail CreditDetail { get; set; }
        [Obfuscation]
        public List<Remark> Remarks { get; set; }
    }
}