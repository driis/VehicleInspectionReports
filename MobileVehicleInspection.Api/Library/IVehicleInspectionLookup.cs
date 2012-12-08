using MobileVehicleInspection.Contracts;

namespace MobileVehicleInspection.Api.Library
{
    public interface IVehicleInspectionLookup
    {
        Vehicle ByRegistration(RegistrationNumber registration);
    }

    public class ScraperSettings
    {
        public string UrlTemplate { get; set; }
    }
}