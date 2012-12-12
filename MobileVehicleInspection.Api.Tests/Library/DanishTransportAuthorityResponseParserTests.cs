using System;
using System.IO;
using System.Linq;
using MobileVehicleInspection.Api.Library;
using MobileVehicleInspection.Contracts;
using NUnit.Framework;

namespace MobileVehicleInspection.Api.Tests.Library
{
    public class DanishTransportAuthorityResponseParserTests
    {
        public const string ValidMarkup = "";

        [Test]
        public void Parse_ValidInput_ReturnsVehicle()
        {
            var result = ParseVehicleFromValidMarkup();
            
            Assert.AreEqual("XK95962", result.RegistrationNumber);
            Assert.AreEqual("VOLKSWAGEN", result.Make);
            Assert.AreEqual("GOLF", result.Model);
            Assert.AreEqual("WVWZZZ1HZSW218578", result.Vin);
        }

        [Test]
        public void Parse_ValidInput_ReturnsVehicleWithInspections()
        {
            var result = ParseVehicleFromValidMarkup();

            Assert.IsNotNull(result.Inspections);
            Assert.AreEqual(5, result.Inspections.Count);
            Assert.IsFalse(result.Inspections.Any(x => x.RegistrationNumber == null));
            Assert.IsFalse(result.Inspections.Any(x => x.Mileage <= 0));
            Assert.IsFalse(result.Inspections.Any(x => x.Date < new DateTime(1980,1,1)));
        }

        private Vehicle ParseVehicleFromValidMarkup()
        {
            var sut = new DanishTransportAuthorityResponseParser();
            string valid;
            using (var reader = new StreamReader(GetType().Assembly.GetManifestResourceStream(GetType(), "valid.html")))
                valid = reader.ReadToEnd();

            Vehicle result = sut.Parse(valid);
            return result;
        }
    }
}