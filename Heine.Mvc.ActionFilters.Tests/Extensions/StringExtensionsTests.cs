using Heine.Mvc.ActionFilters.Extensions;
using NUnit.Framework;

namespace Heine.Mvc.ActionFilters.Tests.Extensions
{
    public class StringExtensionsTests
    {
        [Test]
        [TestCase(0, 2f/3f, ExpectedResult = 0)]
        [TestCase(1, 2f/3f, ExpectedResult = 1)]
        [TestCase(2, 2f/3f, ExpectedResult = 2)]
        [TestCase(3, 2f/3f, ExpectedResult = 3)]
        [TestCase(4, 2f/3f, ExpectedResult = 4)]
        [TestCase(5, 2f/3f, ExpectedResult = 5)]
        [TestCase(6, 2f/3f, ExpectedResult = 6)]
        [TestCase(7, 2f/3f, ExpectedResult = 7)]
        [TestCase(8, 2f/3f, ExpectedResult = 8)]
        [TestCase(9, 2f/3f, ExpectedResult = 9)]
        [TestCase(10, 2f/3f, ExpectedResult = 10)]
        [TestCase(11, 2f/3f, ExpectedResult = 11)]
        [TestCase(12, 2f/3f, ExpectedResult = 12)]
        [TestCase(13, 2f/3f, ExpectedResult = 13)]
        [TestCase(14, 2f/3f, ExpectedResult = 14)]
        [TestCase(15, 2f/3f, ExpectedResult = 15)]
        [TestCase(16, 2f/3f, ExpectedResult = 16)]
        [TestCase(17, 2f/3f, ExpectedResult = 17)]
        [TestCase(18, 2f/3f, ExpectedResult = 18)]
        [TestCase(19, 2f/3f, ExpectedResult = 19)]
        [TestCase(20, 2f/3f, ExpectedResult = 20)]
        [TestCase(1, 1f/3f, ExpectedResult = 1)]
        [TestCase(2, 1f/3f, ExpectedResult = 2)]
        [TestCase(3, 1f/3f, ExpectedResult = 3)]
        [TestCase(4, 1f/3f, ExpectedResult = 4)]
        [TestCase(5, 1f/3f, ExpectedResult = 5)]
        [TestCase(6, 1f/3f, ExpectedResult = 6)]
        [TestCase(7, 1f/3f, ExpectedResult = 7)]
        [TestCase(8, 1f/3f, ExpectedResult = 8)]
        [TestCase(9, 1f/3f, ExpectedResult = 9)]
        [TestCase(10, 1f/3f, ExpectedResult = 10)]
        [TestCase(11, 1f/3f, ExpectedResult = 11)]
        [TestCase(12, 1f/3f, ExpectedResult = 12)]
        [TestCase(13, 1f/3f, ExpectedResult = 13)]
        [TestCase(14, 1f/3f, ExpectedResult = 14)]
        [TestCase(15, 1f/3f, ExpectedResult = 15)]
        [TestCase(16, 1f/3f, ExpectedResult = 16)]
        [TestCase(17, 1f/3f, ExpectedResult = 17)]
        [TestCase(18, 1f/3f, ExpectedResult = 18)]
        [TestCase(19, 1f/3f, ExpectedResult = 19)]
        [TestCase(20, 1f/3f, ExpectedResult = 20)]
        public int ReplaceEndTest(int stringLength, float fraction)
        {
            return new string('*', stringLength).ReplaceEnd('.', fraction).Length;
        }
    }
}