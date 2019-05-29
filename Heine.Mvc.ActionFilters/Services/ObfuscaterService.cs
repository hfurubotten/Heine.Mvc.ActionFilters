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

            void AddObfuscateProperties(Type type, ref List<string> properties, List<PropertyInfo> prevPropList,
                int depth)
            {
                if (depth == 0)
                    return;

                // Check if type is already present in list of previous properties.
                // If so it should not be traversed.
                if (prevPropList != null)
                {
                    var typeName = type.FullName;
                    foreach (var prop in prevPropList)
                    {
                        if (prop.DeclaringType?.FullName == typeName)
                        {
                            return;
                        }
                    }
                }

                foreach (var propertyInfo in type.GetProperties())
                {
                    // Empty all previous properties because method is at root.
                    if (depth == expandDepth)
                    {
                        prevPropList = new List<PropertyInfo>();
                    }

                    // When circling back after a recursive call there will be element/s in list
                    // which should not be present at current depth.
                    // Therefore need to remove all the last elements in list which whould not be at current depth.
                    if (prevPropList.Count > expandDepth - depth)
                    {
                        var numberOfRecordsToDelete = prevPropList.Count - (expandDepth - depth);
                        for (var i = 0; i < numberOfRecordsToDelete; i++)
                        {
                            prevPropList.RemoveAt(prevPropList.Count - 1);
                        }
                    }

                    if (Attribute.IsDefined(propertyInfo, typeof(ObfuscationAttribute)))
                    {
                        if (!prevPropList.Any())
                        {
                            properties.Add(propertyInfo.Name);
                        }
                        else
                        {
                            var jPath = string.Empty;

                            foreach (var prop in prevPropList)
                            {
                                jPath += $"{BuildPropertyName(prop)}.";
                            }

                            properties.Add($"{jPath}{propertyInfo.Name}");
                        }
                    }
                    else if (propertyInfo.PropertyType.IsClass)
                    {
                        if (depth - 1 <= 0) continue;
                        var propertyType = GetCorrectType(propertyInfo.PropertyType);

                        prevPropList.Add(propertyInfo);
                        AddObfuscateProperties(propertyType, ref properties, prevPropList, depth - 1);
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
                        AddObfuscateProperties(type, ref properties, null, expandDepth);
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