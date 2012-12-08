using MobileVehicleInspection.Contracts;

namespace MobileVehicleInspection.Api.Library
{
    public interface IVehicleInspectionLookup
    {
        Vehicle ByRegistration(RegistrationNumber registration);
        Vehicle ByVin(VehicleIdentificationNumber vin);
    }
}