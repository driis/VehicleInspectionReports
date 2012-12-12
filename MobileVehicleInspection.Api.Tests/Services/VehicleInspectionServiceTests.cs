using MobileVehicleInspection.Api.Library;
using MobileVehicleInspection.Api.Services;
using MobileVehicleInspection.Contracts;
using MobileVehicleInspection.Contracts.Operations;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using ServiceStack.Common.Web;

namespace MobileVehicleInspection.Api.Tests.Services
{
    [TestFixture]
    public class VehicleInspectionServiceTests
    {
        private Fixture _fixture;
        private VehicleInspectionService _sut;
        private IVehicleInspectionLookup _inspectionLookup;

        [SetUp]
        public void BeforeEach()
        {
            _fixture = new Fixture();
            _inspectionLookup = Mock.Of<IVehicleInspectionLookup>(x =>
                x.ByRegistration(It.IsAny<RegistrationNumber>()) == _fixture.CreateAnonymous<Vehicle>() && 
                x.ByVehicleIdentificationNumber(It.IsAny<VehicleIdentificationNumber>()) == _fixture.CreateAnonymous<Vehicle>());
            _sut = new VehicleInspectionService(_inspectionLookup);
        }

        [Test]
        public void Get_ByRegistration_CallsService()
        {
            var request = new VehicleByRegistration {RegistrationNumber = "XK96123"};

            var result = _sut.Get(request);

            Assert.IsNotNull(result);
            Mock.Get(_inspectionLookup).Verify(x => 
                x.ByRegistration(It.Is<RegistrationNumber>(reg => reg.Value == request.RegistrationNumber)));
        }

        [Test]
        public void Get_ByRegistration_WhenNotFound_Throws()
        {
            Mock.Get(_inspectionLookup).Setup(x => x.ByRegistration(It.IsAny<RegistrationNumber>()))
                .Returns((Vehicle) null);
            var request = new VehicleByRegistration { RegistrationNumber = "XK96123" };

            Assert.Throws<HttpError>(() => _sut.Get(request));
        }

        [Test]
        public void Get_ByVin_CallsService()
        {
            var request = _fixture.CreateAnonymous<VehicleByVin>();

            var result = _sut.Get(request);

            Assert.IsNotNull(result);
            Mock.Get(_inspectionLookup).Verify(x =>
                x.ByVehicleIdentificationNumber(It.Is<VehicleIdentificationNumber>(reg => reg.Value == request.Vin)));
        }

        [Test]
        public void Get_ByVin_WhenNotFound_Throws()
        {
            Mock.Get(_inspectionLookup).Setup(x => x.ByVehicleIdentificationNumber(It.IsAny<VehicleIdentificationNumber>()))
                .Returns((Vehicle)null);
            var request = _fixture.CreateAnonymous<VehicleByVin>();

            Assert.Throws<HttpError>(() => _sut.Get(request));
        }
    }
}