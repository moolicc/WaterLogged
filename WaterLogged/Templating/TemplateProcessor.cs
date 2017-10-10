using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using WaterLogged.Parsing.Templater.Tokens;

namespace WaterLogged.Templating
{
    /// <summary>
    /// Contains functions for processing templated messages.
    /// </summary>
    public static class TemplateProcessor
    {
        /// <summary>
        /// Parses a template and its property values and returns the resulting message.
        /// </summary>
        /// <param name="templateSource">The source of the template.</param>
        /// <param name="values">An array of values to resolve for the template's propeties.</param>
        public static StructuredMessage BuildMessage(string templateSource, params object[] values)
        {
            return BuildMessage(Template.FromTemplateCache(templateSource), templateSource, values);
        }

        /// <summary>
        /// Parses a template and its property values and returns the resulting message.
        /// </summary>
        /// <param name="template">The template.</param>
        /// <param name="values">An array of values to resolve for the template's propeties.</param>
        public static StructuredMessage BuildMessage(Template template, params object[] values)
        {
            return BuildMessage(template, template.BuildSource(), values);
        }

        /// <summary>
        /// Parses a template and its property values and returns the resulting message.
        /// </summary>
        /// <param name="template">The template.</param>
        /// <param name="templateSource">The source of the template.</param>
        /// <param name="values">An array of values to resolve for the template's propeties.</param>
        public static StructuredMessage BuildMessage(Template template, string templateSource, params object[] values)
        {
            Dictionary<string, int> indices = new Dictionary<string, int>();
            int valuesIndex = 0;
            StructuredMessage message = new StructuredMessage(templateSource, template);

            var tokens = template.GetTokens();
            for (var i = 0; i < tokens.Length; i++)
            {
                var token = tokens[i];
                if (token is PropertyHoleToken propertyToken)
                {
                    if (!indices.ContainsKey(propertyToken.PropertyName))
                    {
                        if (valuesIndex >= values.Length)
                        {
                            continue;
                        }
                        indices.Add(propertyToken.PropertyName, valuesIndex);
                        message.TemplateValues.Add(i, new PropertyValue(values[valuesIndex], propertyToken.GetModifier()));
                        valuesIndex++;
                    }
                    else
                    {
                        var valueIndex = indices[propertyToken.PropertyName];
                        if (valueIndex >= values.Length)
                        {
                            continue;
                        }
                        message.TemplateValues.Add(i, new PropertyValue(values[valueIndex], propertyToken.GetModifier()));
                    }
                }
            }

            return message;
        }



        /// <summary>
        /// Parses a template and its property values by the property names and returns the resulting message.
        /// </summary>
        /// <param name="templateSource">The source of the template.</param>
        /// <param name="values">An array of values to resolve for the template's propeties.</param>
        public static StructuredMessage BuildNamedMessage(string templateSource,
            params (string name, object val)[] values)
        {
            return BuildNamedMessage(Template.FromTemplateCache(templateSource), templateSource, values);
        }

        /// <summary>
        /// Parses a template and its property values by the property names and returns the resulting message.
        /// </summary>
        /// <param name="template">The template.</param>
        /// <param name="values">An array of values to resolve for the template's propeties.</param>
        public static StructuredMessage BuildNamedMessage(Template template,
            params (string name, object val)[] values)
        {
            return BuildNamedMessage(template, template.BuildSource(), values);
        }

        /// <summary>
        /// Parses a template and its property values by the property names and returns the resulting message.
        /// </summary>
        /// <param name="template">The template.</param>
        /// <param name="templateSource">The source of the template.</param>
        /// <param name="values">An array of values to resolve for the template's propeties.</param>
        public static StructuredMessage BuildNamedMessage(Template template, string templateSource, params (string name, object val)[] values)
        {
            StructuredMessage message = new StructuredMessage(templateSource, template);

            var tokens = template.GetTokens();
            for (var i = 0; i < tokens.Length; i++)
            {
                var token = tokens[i];
                if (token is PropertyHoleToken propertyToken)
                {
                    try
                    {
                        var valueItem = values.First(v => v.name == propertyToken.PropertyName);
                        message.TemplateValues.Add(i, new PropertyValue(valueItem.val, propertyToken.GetModifier()));
                    }
                    catch
                    {
                    }
                }
            }

            return message;
        }



        /// <summary>
        /// Parses a template and its property values by the members contained in a parent object and returns the resulting message.
        /// </summary>
        /// <param name="templateSource">The source of the template.</param>
        /// <param name="parentObject">An object containing members to resolve from.</param>
        public static StructuredMessage BuildParentMessage(string templateSource, object parentObject)
        {
            return BuildParentMessage(Template.FromTemplateCache(templateSource), templateSource, parentObject);
        }

        /// <summary>
        /// Parses a template and its property values by the members contained in a parent object and returns the resulting message.
        /// </summary>
        /// <param name="template">The template.</param>
        /// <param name="parentObject">An object containing members to resolve from.</param>
        public static StructuredMessage BuildParentMessage(Template template, object parentObject)
        {
            return BuildParentMessage(template, template.BuildSource(), parentObject);
        }

        /// <summary>
        /// Parses a template and its property values by the members contained in a parent object and returns the resulting message.
        /// </summary>
        /// <param name="template">The template.</param>
        /// <param name="templateSource">The source of the template.</param>
        /// <param name="parentObject">An object containing members to resolve from.</param>
        public static StructuredMessage BuildParentMessage(Template template, string templateSource,
            object parentObject)
        {
            List<(string, object)> values = new List<(string, object)>();

            Type parentType = parentObject.GetType();
            ParentObjectAttribute parentAttrib = parentType.GetTypeInfo().GetCustomAttribute<ParentObjectAttribute>();
            if (parentAttrib == null)
            {
                parentAttrib = new ParentObjectAttribute(ParentObjectRules.All);
            }

            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            
            foreach (var property in parentType.GetProperties(bindingFlags))
            {
                bool forceInclude = false;
                var attrib = property.GetCustomAttribute<IncludeValueAttribute>();
                if (attrib != null)
                {
                    if (!attrib.Value)
                    {
                        continue;
                    }
                    forceInclude = attrib.Value;
                }
                if (!forceInclude && !parentAttrib.Rules.HasFlag(ParentObjectRules.PublicProperties) && property.GetGetMethod().IsPublic)
                {
                    continue;
                }
                if (!forceInclude && !parentAttrib.Rules.HasFlag(ParentObjectRules.PrivateProperties) && !property.GetGetMethod().IsPublic)
                {
                    continue;
                }
                if (!forceInclude && !property.CanWrite && !parentAttrib.Rules.HasFlag(ParentObjectRules.ReadonlyProperties))
                {
                    continue;
                }
                values.Add((property.Name, property.GetValue(parentObject)));
            }
            
            foreach (var field in parentType.GetFields(bindingFlags))
            {
                bool forceInclude = false;
                var attrib = field.GetCustomAttribute<IncludeValueAttribute>();
                if (attrib != null)
                {
                    if (!attrib.Value)
                    {
                        continue;
                    }
                    forceInclude = attrib.Value;
                }
                if (!forceInclude && !parentAttrib.Rules.HasFlag(ParentObjectRules.PublicFields) && field.IsPublic)
                {
                    continue;
                }
                if (!forceInclude && !parentAttrib.Rules.HasFlag(ParentObjectRules.PrivateProperties) && !field.IsPublic)
                {
                    continue;
                }

                values.Add((field.Name, field.GetValue(parentObject)));
            }

            return BuildNamedMessage(template, templateSource, values.ToArray());
        }


        /// <summary>
        /// Renders a structured log message to a string and returns the result.
        /// </summary>
        /// <param name="message">The message to render.</param>
        /// <returns></returns>
        public static string BuildString(StructuredMessage message)
        {
            StringBuilder builder = new StringBuilder();
            var tokens = message.ParsedTemplate.GetTokens();
            for (var i = 0; i < tokens.Length; i++)
            {
                if (tokens[i].GetType() == typeof(LiteralToken))
                {
                    builder.Append(tokens[i].BuildString());
                }
                else if (tokens[i] is PropertyHoleToken token)
                {
                    if (message.TemplateValues.ContainsKey(i))
                    {
                        if (string.IsNullOrWhiteSpace(token.Argument))
                        {
                            builder.Append(message.TemplateValues[i].Value);
                        }
                        else
                        {
                            builder.AppendFormat("{0:" + token.Argument + "}", message.TemplateValues[i].Value);
                        }
                    }
                    else
                    {
                        builder.Append(token.BuildString());
                    }
                }
            }
            return builder.ToString();
        }
    }
}
