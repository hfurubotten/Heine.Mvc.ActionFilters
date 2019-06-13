using System.Linq;
using System.Reflection;
using FluentAssertions;
using Heine.Mvc.ActionFilters.Services;
using Heine.Mvc.ActionFilters.Tests.Services.Models;
using NUnit.Framework;

namespace Heine.Mvc.ActionFilters.Tests.Services
{
    [TestFixture]
    public class ObfuscatorServiceTests
    {
        private Assembly documentAssembly;

        [SetUp]
        public void Init()
        {
            // Arrange
            documentAssembly = typeof(Document).Assembly;
        }

        [Test]
        public void ObfuscatorService_ExpandDepthIsFive_ShouldReturnMultiplePaths()
        {
            // Act
            var obfuscationService = new ObfuscaterService(5, documentAssembly);

            var obfuscationGraph = obfuscationService.TypeObfuscationGraphs;
            var documentGraph = obfuscationGraph[typeof(Document)];
            var personGraph = obfuscationGraph[typeof(Person)];

            // Assert
            obfuscationGraph.Should().NotBeEmpty();
            documentGraph.Should().NotBeEmpty();
            personGraph.Should().NotBeEmpty();
            documentGraph.Should().HaveCount(27);
            documentGraph.Should().Contain(s => s.Equals("Content"));
            documentGraph.Should().Contain(s => s.Equals("Owner.Mobil"));
            documentGraph.Should().Contain(s => s.Equals("Owner.BankAccNo"));
            documentGraph.Should().Contain(s => s.Equals("Owner.Remarks[*]"));
            documentGraph.Should().Contain(s => s.Equals("Owner.CreditDetail"));
            documentGraph.Should().Contain(s => s.Equals("Persons[*].Mobil"));
            documentGraph.Should().Contain(s => s.Equals("Persons[*].BankAccNo"));
            documentGraph.Should().Contain(s => s.Equals("Persons[*].Remarks[*]"));
            documentGraph.Should().Contain(s => s.Equals("Persons[*].CreditDetail"));
            documentGraph.Should().Contain(s => s.Equals("Parent.Content"));
            documentGraph.Should().Contain(s => s.Equals("Parent.Owner.Mobil"));
            documentGraph.Should().Contain(s => s.Equals("Parent.Owner.BankAccNo"));
            documentGraph.Should().Contain(s => s.Equals("Parent.Owner.Remarks[*]"));
            documentGraph.Should().Contain(s => s.Equals("Parent.Owner.CreditDetail"));
            documentGraph.Should().Contain(s => s.Equals("Parent.Persons[*].Mobil"));
            documentGraph.Should().Contain(s => s.Equals("Parent.Persons[*].BankAccNo"));
            documentGraph.Should().Contain(s => s.Equals("Parent.Persons[*].Remarks[*]"));
            documentGraph.Should().Contain(s => s.Equals("Parent.Persons[*].CreditDetail"));
            documentGraph.Should().Contain(s => s.Equals("Children[*].Content"));
            documentGraph.Should().Contain(s => s.Equals("Children[*].Owner.Mobil"));
            documentGraph.Should().Contain(s => s.Equals("Children[*].Owner.BankAccNo"));
            documentGraph.Should().Contain(s => s.Equals("Children[*].Owner.CreditDetail"));
            documentGraph.Should().Contain(s => s.Equals("Children[*].Owner.Remarks[*]"));
            documentGraph.Should().Contain(s => s.Equals("Children[*].Persons[*].Mobil"));
            documentGraph.Should().Contain(s => s.Equals("Children[*].Persons[*].BankAccNo"));
            documentGraph.Should().Contain(s => s.Equals("Children[*].Persons[*].Remarks[*]"));
            documentGraph.Should().Contain(s => s.Equals("Children[*].Persons[*].CreditDetail"));
            personGraph.Should().HaveCount(6);
            personGraph.Should().Contain(s => s.Equals("BankAccNo"));
            personGraph.Should().Contain(s => s.Equals("Mobil"));
            personGraph.Should().Contain(s => s.Equals("Documents[*].Content"));
            personGraph.Should().Contain(s => s.Equals("PrivateDocuments[*].Content"));
            personGraph.Should().Contain(s => s.Equals("CreditDetail"));
            personGraph.Should().Contain(s => s.Equals("Remarks[*]"));
        }

        [Test]
        public void ObfuscatorService_ExpandDepthIsOne_ShouldReturnMultiplePaths()
        {
            // Act
            var obfuscationService = new ObfuscaterService(1, documentAssembly);

            var obfuscationGraph = obfuscationService.TypeObfuscationGraphs;
            var documentGraph = obfuscationGraph[typeof(Document)];
            var personGraph = obfuscationGraph[typeof(Person)];


            // Assert
            obfuscationGraph.Should().NotBeEmpty();
            documentGraph.Should().NotBeEmpty();
            personGraph.Should().NotBeEmpty();
            documentGraph.Should().HaveCount(1);
            documentGraph.Should().Contain(s => s.Equals("Content"));
            personGraph.Should().HaveCount(4);
            personGraph.Should().Contain(s => s.Equals("BankAccNo"));
            personGraph.Should().Contain(s => s.Equals("Mobil"));
            personGraph.Should().Contain(s => s.Equals("CreditDetail"));
            personGraph.Should().Contain(s => s.Equals("Remarks[*]"));
        }

        [Test]
        public void ObfuscatorService_ExpandDepthIsZero_ShouldNotReturnAnyPaths()
        {
            // Act
            var obfuscationService = new ObfuscaterService(0, documentAssembly);

            var obfuscationGraph = obfuscationService.TypeObfuscationGraphs;

            // Assert
            obfuscationGraph.Should().BeEmpty();
        }
    }
}