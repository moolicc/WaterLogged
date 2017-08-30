using System.Xml.Linq;

namespace WaterLogged.Serialization.Xml
{
    static class Extensions
    {
        public static (bool, XAttribute) GetAttribute(this XElement element, string attributeName)
        {
            var attrib = element.Attribute(attributeName);
            return (attrib != null, attrib);
        }

        public static string GetValueOrAttribute(this XElement element, string attributeName)
        {
            var attribInfo = element.GetAttribute(attributeName);
            if (attribInfo.Item1)
            {
                return attribInfo.Item2.Value;
            }
            return element.Value;
        }
    }
}
