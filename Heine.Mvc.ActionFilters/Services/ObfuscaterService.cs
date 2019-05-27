using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;

namespace Heine.Mvc.ActionFilters.Services
{
    public class ObfuscaterService : IObfuscaterService
    {
        private readonly int expandDepth;
        private const string HeaderKeyXObfuscate = "X-Obfuscate";

        public ObfuscaterService(int expandDepth, params Assembly[] assemblies)
        {
            this.expandDepth = expandDepth;
            TypeObfuscationGraphs = Initialize(assemblies);
        }

        private IDictionary<Type, IList<string>> Initialize(params Assembly[] assemblies)
        {
            string BuildPropertyName(PropertyInfo propertyInfo)
            {
                return typeof(IEnumerable).IsAssignableFrom(propertyInfo.PropertyType)
                    ? $"{propertyInfo.Name}[*]" //Array
                    : propertyInfo.Name; // Object or scalar value
            }

            Type GetCorrectType(Type type)
            {
                if (type.IsArray)
                    return type.GetElementType();

                if (typeof(IEnumerable<>).IsAssignableFrom(type))
                    return type.GetGenericTypeDefinition();

                return type;
            }

            void AddObfuscateProperties(Type type, ref List<string> properties, string prevBuildProps,
                PropertyInfo prevPropInfo, int depth)
            {
                if (depth == 0)
                    return;

                foreach (var propertyInfo in type.GetProperties())
                {
                    if (Attribute.IsDefined(propertyInfo, typeof(ObfuscationAttribute)))
                    {
                        // First expand
                        if (depth == expandDepth)
                        {
                            properties.Add(propertyInfo.Name);
                        }
                        // Second expand
                        else if (depth == expandDepth - 1)
                        {
                            prevBuildProps = BuildPropertyName(prevPropInfo);
                            properties.Add($"{prevBuildProps}.{propertyInfo.Name}");
                        }
                        else
                        {
                            prevBuildProps = $"{prevBuildProps}.{BuildPropertyName(prevPropInfo)}";
                            properties.Add($"{prevBuildProps}.{propertyInfo.Name}");
                        }
                    }
                    else if (propertyInfo.PropertyType.IsClass)
                    {
                        if (depth-1 <= 0) continue;
                        var propertyType = GetCorrectType(propertyInfo.PropertyType);

                        AddObfuscateProperties(propertyType, ref properties, prevBuildProps, propertyInfo, depth--);
                    }
                }
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
                            properties.Add(propertyInfo.Name);
                        }
                    }
                    else
                    {
                        AddObfuscateProperties(type, ref properties, string.Empty, null, expandDepth);
                    }

                    if (properties.Any())
                        typeObfuscationGraphs.Add(type, properties);
                }
            }

            return typeObfuscationGraphs;
        }

        private IDictionary<Type, IList<string>> TypeObfuscationGraphs { get; set; }

        public void SetObfuscateHeader(HttpHeaders httpHeaders, params Type[] types)
        {
            if (httpHeaders == null) return;
            foreach (var type in types)
            {
                if (!TypeObfuscationGraphs.ContainsKey(type)) return;
                if (httpHeaders.TryGetValues(HeaderKeyXObfuscate, out var values))
                {
                    if (!httpHeaders.Remove(HeaderKeyXObfuscate)) return;

                    httpHeaders.TryAddWithoutValidation(HeaderKeyXObfuscate,
                        values.Concat(TypeObfuscationGraphs[type]).Distinct());
                }
                else
                {
                    httpHeaders.TryAddWithoutValidation(HeaderKeyXObfuscate, TypeObfuscationGraphs[type].Distinct());
                }
            }
        }
    }
}