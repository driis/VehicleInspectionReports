using MobileVehicleInspection.Api.Library;
using MobileVehicleInspection.Contracts;
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
            return new Vehicle {Vin = vehicle.Vin};
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