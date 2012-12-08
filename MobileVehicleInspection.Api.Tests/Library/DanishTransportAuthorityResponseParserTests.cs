using System.IO;
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
            var sut = new DanishTransportAuthorityResponseParser();
            string valid;
            using (var reader = new StreamReader(GetType().Assembly.GetManifestResourceStream(GetType(), "valid.html")))
                valid = reader.ReadToEnd();

            Vehicle result = sut.Parse(valid);
            Assert.AreEqual("XK95962", result.RegistrationNumber);
            Assert.AreEqual("VOLKSWAGEN", result.Make);
            Assert.AreEqual("GOLF", result.Model);
            Assert.AreEqual("WVWZZZ1HZSW218578", result.Vin);
        } 
    }
}