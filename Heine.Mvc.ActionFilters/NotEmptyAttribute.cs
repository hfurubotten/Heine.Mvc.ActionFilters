using System;
using System.ComponentModel.DataAnnotations;

namespace Heine.Mvc.ActionFilters
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public sealed class NotEmptyAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return !(value is Guid guid) || guid != Guid.Empty;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"The field {name} cannot be empty GUID ({Guid.Empty})";
        }
    }
}