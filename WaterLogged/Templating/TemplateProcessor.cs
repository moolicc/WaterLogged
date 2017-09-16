using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace WaterLogged.Templating
{
    public static class TemplateProcessor
    {
        public static StructuredMessage ProcessTemplate(string template, params object[] holeValues)
        {
            var message = new StructuredMessage(template);
            var holes = ProcessHoles(template);

            for (var i = 0; i < holes.Length; i++)
            {
                var hole = holes[i];
                if (hole.Id.IdType == HoleIdTypes.Positional)
                {
                    if (holeValues.Length > hole.Id.PositionalId)
                    {
                        message.Values.Add(hole, new HoleValue(hole, holeValues[hole.Id.PositionalId]));
                    }
                    else
                    {
                        message.Values.Add(hole, new HoleValue(hole, hole.ToString()));
                    }
                }
                else
                {
                    if (holeValues.Length > i)
                    {
                        message.Values.Add(hole, new HoleValue(hole, holeValues[hole.HoleIndex]));
                    }
                    else
                    {
                        message.Values.Add(hole, new HoleValue(hole, hole.ToString()));
                    }
                }
            }

            return message;
        }

        public static StructuredMessage ProcessParentedTemplate(string template, object parentObject)
        {
            return ProcessParentedTemplate(template, parentObject, parentObject.GetType());
        }

        public static StructuredMessage ProcessParentedTemplate<T>(string template, T parentObject)
        {
            return ProcessParentedTemplate(template, parentObject, parentObject.GetType());
        }

        public static StructuredMessage ProcessParentedTemplate(string template, Type parentType)
        {
            return ProcessParentedTemplate(template, null, parentType);
        }

        public static StructuredMessage ProcessParentedTemplate<T>(string template)
        {
            return ProcessParentedTemplate(template, null, typeof(T));
        }

        public static StructuredMessage ProcessNamedTemplate(string template, params (string, object)[] holeValues)
        {
            var message = new StructuredMessage(template);
            var holes = ProcessHoles(template);
            List<int> usedIndices = new List<int>();

            foreach (var hole in holes)
            {
                if (hole.Id.IdType == HoleIdTypes.Positional)
                {
                    if (holeValues.Length > hole.Id.PositionalId)
                    {
                        message.Values.Add(hole, new HoleValue(hole, holeValues[hole.Id.PositionalId].Item2));
                    }
                    else
                    {
                        message.Values.Add(hole, new HoleValue(hole, hole.ToString()));
                    }
                }
                else
                {
                    bool itemFound = false;
                    for (var i = 0; i < holeValues.Length; i++)
                    {
                        var holeValue = holeValues[i];
                        if (holeValue.Item1 == hole.Id.NamedId && !usedIndices.Contains(i))
                        {
                            message.Values.Add(hole, new HoleValue(hole, holeValue.Item2));
                            itemFound = true;
                            usedIndices.Add(i);
                            break;
                        }
                    }
                    if (!itemFound)
                    {
                        message.Values.Add(hole, new HoleValue(hole, hole.ToString()));
                    }
                }
            }

            for (int i = 0; i < holeValues.Length; i++)
            {
                if (!usedIndices.Contains(i))
                {
                    var value = holeValues[i];
                    message.ContextValues.Add(value.Item1, value.Item2);
                }
            }

            return message;
        }
         

        public static Hole[] ProcessHoles(string template)
        {
            List<Hole> holes = new List<Hole>();
            for (int i = 0; i < template.Length; i++)
            {
                char curChar = template[i];
                char nextChar = '\0';
                if (i + 1 < template.Length)
                {
                    nextChar = template[i + 1];
                }
                if (curChar == '{' && nextChar != '{')
                {
                    var hole = ProcessHole(template, i + 1, out var newIndex);
                    hole.HoleIndex = holes.Count;
                    holes.Add(hole);
                    i = newIndex;
                }
                else if (curChar == '{' && nextChar == '{')
                {
                    i++;
                }
            }
            return holes.ToArray();
        }


        public static string ProcessMessage(StructuredMessage message)
        {
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < message.Template.Length; i++)
            {
                if (message.Template[i] == '{')
                {
                    var holeValue = message.Values.FirstOrDefault(k => k.Key.TemplateStartIndex == i).Value;
                    if (holeValue != null)
                    {
                        builder.AppendFormat("{0:" + holeValue.Hole.Suffix + "}", holeValue.Value);
                        i = holeValue.Hole.TemplateEndIndex;
                        continue;
                    }
                }
                builder.Append(message.Template[i]);
            }

            return builder.ToString();
        }


        private static StructuredMessage ProcessParentedTemplate(string template, object parentObject, Type type)
        {
            var parentAttrib = type.GetTypeInfo().GetCustomAttribute<ParentObjectAttribute>();
            if (parentAttrib == null)
            {
                parentAttrib = new ParentObjectAttribute(ParentObjectResolveRules.All);
            }

            var bindingFlags = BindingFlags.Public | BindingFlags.NonPublic;
            if (parentObject == null)
            {
                bindingFlags |= BindingFlags.Static;
            }
            else
            {
                bindingFlags |= BindingFlags.Instance;
            }
            List<(string, object)> values = new List<(string, object)>();
            foreach (var propertyInfo in type.GetProperties())
            {
                var attrib = propertyInfo.GetCustomAttribute<ParentObjectValueAttribute>();
                if (attrib != null)
                {
                    if (attrib.GetInclusion() == ParentObjectValueInclusion.Ignore)
                    {
                        continue;
                    }
                    else
                    {
                        values.Add((propertyInfo.Name, propertyInfo.GetValue(parentObject)));
                        break;
                    }
                }
                if (!propertyInfo.CanWrite && (parentAttrib.GetRules() & ParentObjectResolveRules.ReadonlyProperties) == 0)
                {
                    continue;
                }
                if ((parentAttrib.GetRules() & ParentObjectResolveRules.PrivateProperties) == 0 && propertyInfo.GetGetMethod(true).IsPrivate)
                {
                    continue;
                }
                if ((parentAttrib.GetRules() & ParentObjectResolveRules.PublicProperties) == 0 && propertyInfo.GetGetMethod(true).IsPublic)
                {
                    continue;
                }
                values.Add((propertyInfo.Name, propertyInfo.GetValue(parentObject)));
            }

            foreach (var fieldInfo in type.GetFields(bindingFlags))
            {
                var attrib = fieldInfo.GetCustomAttribute<ParentObjectValueAttribute>();
                if (attrib != null)
                {
                    if (attrib.GetInclusion() == ParentObjectValueInclusion.Ignore)
                    {
                        continue;
                    }
                    else
                    {
                        values.Add((fieldInfo.Name, fieldInfo.GetValue(parentObject)));
                        break;
                    }
                }
                if (fieldInfo.IsInitOnly && (parentAttrib.GetRules() & ParentObjectResolveRules.ReadonlyProperties) == 0)
                {
                    continue;
                }
                if ((parentAttrib.GetRules() & ParentObjectResolveRules.PrivateProperties) == 0 && fieldInfo.IsPrivate)
                {
                    continue;
                }
                if ((parentAttrib.GetRules() & ParentObjectResolveRules.PublicProperties) == 0 && fieldInfo.IsPublic)
                {
                    continue;
                }
                values.Add((fieldInfo.Name, fieldInfo.GetValue(parentObject)));
            }

            return ProcessNamedTemplate(template, values.ToArray());
        }

        private static Hole ProcessHole(string template, int index, out int newIndex)
        {
            newIndex = -1;
            HoleId id = null;
            HolePrefix prefix = HolePrefix.None;
            string suffix = "";

            string name = "";
            bool parsingSuffix = false;
            for (int i = index; i < template.Length; i++)
            {
                char curChar = template[i];
                char nextChar = '\0';
                if (i + 1 < template.Length)
                {
                    nextChar = template[i + 1];
                }
                if (i == index && curChar == '$')
                {
                    prefix = HolePrefix.Stringification;
                    continue;
                }
                if (i == index && curChar == '@')
                {
                    prefix = HolePrefix.Destructuring;
                    continue;
                }
                if (curChar == ':')
                {
                    parsingSuffix = true;
                    continue;
                }
                if (parsingSuffix)
                {
                    suffix += curChar;
                    continue;
                }
                if (curChar == '}')
                {
                    newIndex = i;
                    break;
                }
                name += curChar;
            }

            if (int.TryParse(name, out var result))
            {
                id = new HoleId(result);
            }
            else
            {
                id = new HoleId(name);
            }

            var hole = new Hole(id, prefix, suffix);
            hole.TemplateStartIndex = index - 1;
            hole.TemplateEndIndex = newIndex;
            return hole;
        }
        
    }
}