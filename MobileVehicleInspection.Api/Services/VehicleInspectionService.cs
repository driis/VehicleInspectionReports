using MobileVehicleInspection.Contracts;
using MobileVehicleInspection.Contracts.Operations;
using ServiceStack.ServiceHost;

namespace MobileVehicleInspection.Api.Services
{
    public class VehicleInspectionService : IService
    {
        public object Get(VehicleByVin vehicle)
        {
            return new Vehicle {Vin = vehicle.Vin};
        }
    }
}