using MobileVehicleInspection.Api.Library;
using MobileVehicleInspection.Contracts.Operations;
using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;

namespace MobileVehicleInspection.Api.Services
{
    public class VehicleInspectionService : IService
    {
        private readonly IVehicleInspectionLookup _lookup;

        public VehicleInspectionService(IVehicleInspectionLookup lookup)
        {
            _lookup = lookup;
        }

        public object Get(VehicleByVin vehicle)
        {
            var result = _lookup.ByVehicleIdentificationNumber(new VehicleIdentificationNumber(vehicle.Vin));
            if (result == null)
                throw HttpError.NotFound(
                    string.Format("The vehicle with registration number {0} could not be found in the database.",
                                  vehicle.Vin));

            return result;
        }

        public object Get(VehicleByRegistration vehicle)
        {
            var result = _lookup.ByRegistration(new RegistrationNumber(vehicle.RegistrationNumber));
            if (result == null)
                throw HttpError.NotFound(
                    string.Format("The vehicle with registration number {0} could not be found in the database.",
                                  vehicle.RegistrationNumber));

            return result;
        }
    }
}