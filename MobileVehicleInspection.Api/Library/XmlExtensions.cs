using System.Xml.Linq;

namespace MobileVehicleInspection.Api.Library
{
    public static class XmlExtensions
    {
        public static string AttributeValue(this XElement element, XName name)
        {
            var att = element.Attribute(name);
            if (att == null)
                return null;

            return att.Value;
        }

        public static bool HasClass(this XElement element, string className)
        {
            string value = AttributeValue(element, "class");
            if (value == null)
                return false;
            return value.Contains(className);
        }

        public static bool HasId(this XElement element, string id)
        {
            string value = AttributeValue(element, "id");
            if (value == null)
                return false;
            return value.Contains(id);
        }
    }
}