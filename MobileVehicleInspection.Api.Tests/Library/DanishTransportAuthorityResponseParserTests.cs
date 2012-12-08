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
            Assert.Inconclusive();
            Assert.AreEqual(new RegistrationNumber("XK95962"), result.RegistrationNumber);
        } 
    }
}