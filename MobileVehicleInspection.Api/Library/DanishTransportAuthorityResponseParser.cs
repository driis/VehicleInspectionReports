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
            var container = doc.Root.Descendants("div").SingleOrDefault(x => x.HasId("pnlVehicleInfo"));
            if (container == null)
                return null;
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
}