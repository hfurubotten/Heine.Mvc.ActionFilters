using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using Heine.Mvc.ActionFilters.ValidationAttributes;
using NUnit.Framework;

namespace Heine.Mvc.ActionFilters.Tests.ValidationAttributes
{
    public class GuidModel
    {
        [GuidNotEmpty]
        public Guid? Foo { get; set; }
        [GuidNotEmpty]
        public Guid Bar { get; set; }
    }

    [TestFixture]
    public class GuidNotEmptyAttributeTests
    {
        [Test]
        public void GuidNotEmptyAttribute_WhenGuidPropertyIsEmpty_ValidationShouldFail()
        {
            var guidModel = new GuidModel
            {
                Foo = null,
                Bar = Guid.Empty
            };

            var context = new ValidationContext(guidModel);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(guidModel, context, results, true);
            isValid.Should().BeFalse();
            results.Count.Should().Be(1);
        }
    }
}