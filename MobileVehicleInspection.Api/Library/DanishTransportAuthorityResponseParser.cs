using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using MobileVehicleInspection.Contracts;

namespace MobileVehicleInspection.Api.Library
{
    public class DanishTransportAuthorityResponseParser
    {
        private static readonly CultureInfo ParseFormatProvider = new CultureInfo("da-DK");

        private readonly Dictionary<string, InspectionResult> KnownResults = new Dictionary<string, InspectionResult>(StringComparer.OrdinalIgnoreCase)
            {
                {"GOD", InspectionResult.Approved},
                {"BET", InspectionResult.ConditionallyApproved},
                {"OOM", InspectionResult.ReinspectionRequired},
                {"todo", InspectionResult.NotApproved}
            };

        public Vehicle Parse(string markup)
        {
            var reader = new Sgml.SgmlReader {InputStream = new StringReader(markup)};
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
                Inspections = ParseInspections(doc)
            };
        }

        private List<Inspection> ParseInspections(XDocument doc)
        {
            var table = doc.Root.Descendants("table").SingleOrDefault(x => x.HasId("tblInspections"));
            if (table == null)
                return new List<Inspection>(0);

            var rows = table.Elements("tbody").Elements("tr");
            return rows.Select(InspectionFromRow).OrderByDescending(x => x.Date).ToList();
        }

        private Inspection InspectionFromRow(XElement rows)
        {
            var cells = rows.Elements("td").ToArray();
            if (cells.Length != 5)
                throw new ApplicationException(string.Format("Invalid format, expected 5 table cells but found {0}", cells.Length));
            InspectionResult result;
            if (!KnownResults.TryGetValue(cells[1].Value, out result))
                result = InspectionResult.Unknown;
            
            DateTime date;
            if(!DateTime.TryParse(cells[0].Value,ParseFormatProvider , DateTimeStyles.None, out date))
                throw new ApplicationException(string.Format("Unable to parse the string '{0}' as a DateTime.", cells[0].Value));

            string registration = cells[3].Value;
            int mileage;
            if (!Int32.TryParse(cells[2].Value, NumberStyles.Any, ParseFormatProvider, out mileage))
                throw new ApplicationException(string.Format("Unable to parse mileage from value '{0}'", cells[2].Value));

            return new Inspection
                {
                    Date = date,
                    Mileage = mileage,
                    RegistrationNumber = registration,
                    Result = result
                };
        }
    }
}