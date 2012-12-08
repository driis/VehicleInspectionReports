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
            //var keys = doc.Root.Descendants("div").Where(x => x.Attribute("class").)
            return new Vehicle();
        }
    }
}