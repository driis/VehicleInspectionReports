using System;
using MobileVehicleInspection.Api.Library;
using NUnit.Framework;

namespace MobileVehicleInspection.Api.Tests.Library
{
    [TestFixture]
    public class RegistrationNumberTests
    {
        [Test]
        [TestCase("x")]
        [TestCase("AB000000")]
        [TestCase("AB04~AA")]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("     ")]
        [TestCase("  ")]
        public void Ctor_InvalidRegistrationNumber_ThrowsArgumentException(string registrationNumber)
        {
            Assert.Throws<ArgumentException>(() => new RegistrationNumber(registrationNumber));
        }

        [Test]
        [TestCase("XK95962", "XK95962")]
        [TestCase("XK 95962", "XK95962")]
        [TestCase("xk 95962", "XK95962")]
        [TestCase("xk  95962", "XK95962")]
        [TestCase("xk", "XK")]
        [TestCase("AB 100", "AB100")]
        [TestCase("Devil", "DEVIL")]
        [TestCase("010", "010")]
        public void Ctor_ValidRegistrationNumber_ConstructsValidRegistrationNumber(string registrationNumber, string expected)
        {
            var reg = new RegistrationNumber(registrationNumber);
            Assert.AreEqual(expected, reg.Value);
        }
    }
}