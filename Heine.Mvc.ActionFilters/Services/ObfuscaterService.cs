using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using Heine.Mvc.ActionFilters.Extensions;

namespace Heine.Mvc.ActionFilters.Services
{
    public class ObfuscaterService : IObfuscaterService
    {
        private readonly bool camelCase;
        private const string HeaderKeyXObfuscate = "X-Obfuscate";

        public ObfuscaterService(bool camelCase, params Assembly[] assemblies)
        {
            this.camelCase = camelCase;
            TypeObfuscationGraphs = Initialize(assemblies);
        }

        //TODO: Refactor this to be recursive and prettier :) But it works for now as a PoC! :D
        public IDictionary<Type, IList<string>> Initialize(params Assembly[] assemblies)
        {
            string FormatPropertyName(string propertyName)
            {
                return camelCase 
                    ? propertyName.ToCamelCase()
                    : propertyName;
            }

            string BuildPropertyName(PropertyInfo propertyInfo)
            {
                return typeof(IEnumerable).IsAssignableFrom(propertyInfo.PropertyType)
                    ? $"{FormatPropertyName(propertyInfo.Name)}[*]" //Array
                    : FormatPropertyName(propertyInfo.Name); // Object or scalar value
            }

            Type GetCorrectType(Type type)
            {
                if (type.IsArray)
                    return type.GetElementType();

                if (typeof(IEnumerable<>).IsAssignableFrom(type))
                    return type.GetGenericTypeDefinition();

                return type;
            }

            var typeObfuscationGraphs = new Dictionary<Type, IList<string>>();
            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    var properties = new List<string>();
                    if (Attribute.IsDefined(type, typeof(ObfuscationAttribute)))
                    {
                        foreach (var propertyInfo in type.GetProperties())
                        {
                            properties.Add(FormatPropertyName(propertyInfo.Name));
                        }
                    }
                    else
                    {
                        foreach (var propertyInfo1 in type.GetProperties())
                        {
                            if (Attribute.IsDefined(propertyInfo1, typeof(ObfuscationAttribute)))
                            {
                                properties.Add(FormatPropertyName(propertyInfo1.Name));
                            }
                            else if (propertyInfo1.PropertyType.IsClass)
                            {
                                var propertyType1 = GetCorrectType(propertyInfo1.PropertyType);

                                foreach (var propertyInfo2 in propertyType1.GetProperties())
                                {
                                    if (Attribute.IsDefined(propertyInfo2, typeof(ObfuscationAttribute)))
                                    {
                                        properties.Add($"{BuildPropertyName(propertyInfo1)}.{FormatPropertyName(propertyInfo2.Name)}");
                                    }
                                    else if (propertyInfo2.PropertyType.IsClass)
                                    {
                                        var propertyType2 = GetCorrectType(propertyInfo2.PropertyType);

                                        foreach (var propertyInfo3 in propertyType2.GetProperties())
                                        {
                                            if (Attribute.IsDefined(propertyInfo3, typeof(ObfuscationAttribute)))
                                            {
                                                properties.Add($"{BuildPropertyName(propertyInfo1)}.{BuildPropertyName(propertyInfo2)}.{FormatPropertyName(propertyInfo3.Name)}");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (properties.Any())
                        typeObfuscationGraphs.Add(type, properties);
                }
            }
            return typeObfuscationGraphs;
        }

        public IDictionary<Type, IList<string>> TypeObfuscationGraphs { get; set; }

        public void SetObfuscateHeader(HttpHeaders httpHeaders, params Type[] types)
        {
            if (httpHeaders == null) return;
            foreach (var type in types)
            {
                if (!TypeObfuscationGraphs.ContainsKey(type)) return;
                if (httpHeaders.TryGetValues(HeaderKeyXObfuscate, out var values))
                {
                    if (!httpHeaders.Remove(HeaderKeyXObfuscate)) return;
                    
                    httpHeaders.TryAddWithoutValidation(HeaderKeyXObfuscate, values.Concat(TypeObfuscationGraphs[type]).Distinct());
                }
                else
                {
                    httpHeaders.TryAddWithoutValidation(HeaderKeyXObfuscate, TypeObfuscationGraphs[type].Distinct());
                }
            }
        }
    }
}