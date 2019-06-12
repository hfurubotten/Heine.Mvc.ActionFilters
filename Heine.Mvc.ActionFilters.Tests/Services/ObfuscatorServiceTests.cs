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
            documentGraph.Count.Should().Be(15);
            documentGraph.FirstOrDefault(s => s.Equals("Content")).Should().NotBeNullOrEmpty();
            documentGraph.FirstOrDefault(s => s.Equals("Owner.Mobil")).Should().NotBeNullOrEmpty();
            documentGraph.FirstOrDefault(s => s.Equals("Owner.BankAccNo")).Should().NotBeNullOrEmpty();
            documentGraph.FirstOrDefault(s => s.Equals("Persons[*].Mobil")).Should().NotBeNullOrEmpty();
            documentGraph.FirstOrDefault(s => s.Equals("Persons[*].BankAccNo")).Should().NotBeNullOrEmpty();
            documentGraph.FirstOrDefault(s => s.Equals("Parent.Content")).Should().NotBeNullOrEmpty();
            documentGraph.FirstOrDefault(s => s.Equals("Parent.Owner.Mobil")).Should().NotBeNullOrEmpty();
            documentGraph.FirstOrDefault(s => s.Equals("Parent.Owner.BankAccNo")).Should().NotBeNullOrEmpty();
            documentGraph.FirstOrDefault(s => s.Equals("Parent.Persons[*].Mobil")).Should().NotBeNullOrEmpty();
            documentGraph.FirstOrDefault(s => s.Equals("Parent.Persons[*].BankAccNo")).Should().NotBeNullOrEmpty();
            documentGraph.FirstOrDefault(s => s.Equals("Children[*].Content")).Should().NotBeNullOrEmpty();
            documentGraph.FirstOrDefault(s => s.Equals("Children[*].Owner.Mobil")).Should().NotBeNullOrEmpty();
            documentGraph.FirstOrDefault(s => s.Equals("Children[*].Owner.BankAccNo")).Should().NotBeNullOrEmpty();
            documentGraph.FirstOrDefault(s => s.Equals("Children[*].Persons[*].Mobil")).Should().NotBeNullOrEmpty();
            documentGraph.FirstOrDefault(s => s.Equals("Children[*].Persons[*].BankAccNo")).Should().NotBeNullOrEmpty();
            personGraph.Count.Should().Be(4);
            personGraph.FirstOrDefault(s => s.Equals("BankAccNo")).Should().NotBeNullOrEmpty();
            personGraph.FirstOrDefault(s => s.Equals("Mobil")).Should().NotBeNullOrEmpty();
            personGraph.FirstOrDefault(s => s.Equals("Documents[*].Content")).Should().NotBeNullOrEmpty();
            personGraph.FirstOrDefault(s => s.Equals("PrivateDocuments[*].Content")).Should().NotBeNullOrEmpty();
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
            documentGraph.Count.Should().Be(1);
            documentGraph.FirstOrDefault(s => s.Equals("Content")).Should().NotBeNullOrEmpty();
            personGraph.Count.Should().Be(2);
            personGraph.FirstOrDefault(s => s.Equals("BankAccNo")).Should().NotBeNullOrEmpty();
            personGraph.FirstOrDefault(s => s.Equals("Mobil")).Should().NotBeNullOrEmpty();
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