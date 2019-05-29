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
            documentGraph.Count.Should().Be(5);
            documentGraph.FirstOrDefault(s => s.Contains("Content")).Should().NotBeNullOrEmpty();
            documentGraph.FirstOrDefault(s => s.Contains("Owner.Mobil")).Should().NotBeNullOrEmpty();
            documentGraph.FirstOrDefault(s => s.Contains("Owner.BankAccNo")).Should().NotBeNullOrEmpty();
            documentGraph.FirstOrDefault(s => s.Contains("Persons[*].Item.Mobil")).Should().NotBeNullOrEmpty();
            documentGraph.FirstOrDefault(s => s.Contains("Persons[*].Item.BankAccNo")).Should().NotBeNullOrEmpty();
            personGraph.Count.Should().Be(4);
            personGraph.FirstOrDefault(s => s.Contains("BankAccNo")).Should().NotBeNullOrEmpty();
            personGraph.FirstOrDefault(s => s.Contains("Mobil")).Should().NotBeNullOrEmpty();
            personGraph.FirstOrDefault(s => s.Contains("Documents[*].Item.Content")).Should().NotBeNullOrEmpty();
            personGraph.FirstOrDefault(s => s.Contains("PrivateDocuments[*].Content")).Should().NotBeNullOrEmpty();
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
            documentGraph.FirstOrDefault(s => s.Contains("Content")).Should().NotBeNullOrEmpty();
            personGraph.Count.Should().Be(2);
            personGraph.FirstOrDefault(s => s.Contains("BankAccNo")).Should().NotBeNullOrEmpty();
            personGraph.FirstOrDefault(s => s.Contains("Mobil")).Should().NotBeNullOrEmpty();
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