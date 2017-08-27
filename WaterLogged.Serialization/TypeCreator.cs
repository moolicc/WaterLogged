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
            //TODO: Support for Lists and Dictionaries

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

            foreach (var memberValue in MemberValues)
            {
                var property = properties.FirstOrDefault(p => p.Name == memberValue.Key);
                if (property != null)
                {
                    property.SetValue(value,
                        StringConversion.Converter.Convert(memberValue.Value, property.PropertyType));
                    continue;
                }
                var field = fields.FirstOrDefault(f => f.Name == memberValue.Key);
                if (field != null)
                {
                    field.SetValue(value, StringConversion.Converter.Convert(memberValue.Value, field.FieldType));
                    continue;
                }
                throw new KeyNotFoundException(string.Format("Member '{0}' not found for type '{1}'.", memberValue.Key, TypeName));
            }
            return value;
        }
    }
}
