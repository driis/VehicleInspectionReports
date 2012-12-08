using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using MobileVehicleInspection.Contracts;

namespace MobileVehicleInspection.Api.Library
{
    public class DanishTransportAuthorityResponseParser
    {
        public Vehicle Parse(string markup)
        {
            var reader = new Sgml.SgmlReader();
            reader.InputStream = new StringReader(markup);
            XDocument doc = XDocument.Load(reader);
            var container = doc.Root.Descendants("div").Single(x => x.HasId("pnlVehicleInfo"));
            var values = container.Elements().Where(x => x.HasClass("pairValue")).ToArray();
            if (values.Length != 4)
                throw new ApplicationException("Cannot parse markup to Vehicle, wrong number of values found");
           
            return new Vehicle {
                Make = values[0].Value,
                Model = values[1].Value,
                Vin = values[2].Value,
                RegistrationNumber = values[3].Value,
            };
        }
    }

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