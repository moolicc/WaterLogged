using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime;
using System.Text;

namespace WaterLogged.Serialization
{
    public class TypeCreator
    {
        public Dictionary<string, string> MemberValues { get; private set; }
        public string TypeName { get; set; }

        public TypeCreator(string type)
        {
            MemberValues = new Dictionary<string, string>();
            TypeName = type;
        }

        public object Create()
        {
            //TODO: Support for method calling
            //TODO: Support for non-default ctor usage
            //TODO: Support for Dictionaries

            Type type = null;
            object value = null;

            try
            {
                type = Type.GetType(TypeName);
                value = Activator.CreateInstance(type);
            }
            catch (Exception e)
            {
                throw new TypeLoadException(string.Format("Failed to create object of type '{0}'. See the inner-exception for details.", TypeName), e);
            }

            var properties = type.GetProperties(BindingFlags.NonPublic | BindingFlags.Public);
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic);
            var methods = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Public);

            foreach (var memberValue in MemberValues)
            {
                var property = properties.FirstOrDefault(p => string.Equals(p.Name, memberValue.Key, StringComparison.OrdinalIgnoreCase));
                if (property != null)
                {
                    if (property.PropertyType == typeof(List<string>))
                    {
                        List<string> propValue = (List<string>) property.GetValue(value);
                        propValue.AddRange(memberValue.Value.Split('|'));
                    }
                    else if (property.PropertyType == typeof(string[]))
                    {
                        property.SetValue(value, memberValue.Value.Split('|'));
                    }
                    else
                    {
                        property.SetValue(value,
                            StringConversion.Converter.Convert(memberValue.Value, property.PropertyType));
                    }
                    continue;
                }
                var field = fields.FirstOrDefault(f => string.Equals(f.Name, memberValue.Key, StringComparison.OrdinalIgnoreCase));
                if (field != null)
                {
                    if (field.FieldType == typeof(List<string>))
                    {
                        List<string> fieldValue = (List<string>)field.GetValue(value);
                        fieldValue.AddRange(memberValue.Value.Split('|'));
                    }
                    else if (field.FieldType == typeof(string[]))
                    {
                        field.SetValue(value, memberValue.Value.Split('|'));
                    }
                    else
                    {
                        field.SetValue(value,
                            StringConversion.Converter.Convert(memberValue.Value, field.FieldType));
                    }
                    continue;
                }

                var method =
                    methods.FirstOrDefault(
                        m => string.Equals(m.Name, memberValue.Key, StringComparison.OrdinalIgnoreCase));
                if (method != null)
                {
                    var methodParams = method.GetParameters();
                    List<object> parameters = new List<object>();
                    string[] parts = new string[1] { memberValue.Value };
                    Dictionary<string, string> parameterMap = new Dictionary<string, string>();

                    if (memberValue.Value.Contains(","))
                    {
                        parts = memberValue.Value.Split(',');
                    }

                    foreach (var part in parts)
                    {
                        if (part.Contains(":"))
                        {
                            var keyvalue = part.Split(':');
                            parameterMap.Add(keyvalue[0], keyvalue[1]);
                        }
                    }

                    foreach (var param in methodParams)
                    {
                        if (!parameterMap.ContainsKey(param.Name))
                        {
                            throw new KeyNotFoundException("Parameter mismatch.");
                        }
                        parameters.Add(StringConversion.Converter.Convert(parameterMap[param.Name], param.ParameterType));
                    }

                    method.Invoke(value, parameters.ToArray());
                }
                throw new KeyNotFoundException(string.Format("Member '{0}' not found for type '{1}'.", memberValue.Key, TypeName));
            }

            return value;
        }
    }
}
