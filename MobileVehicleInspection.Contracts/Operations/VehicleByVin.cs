using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;

namespace MobileVehicleInspection.Contracts.Operations
{
    [Route("vehicle/vin/{Vin}", HttpMethods.Get)]
    public class VehicleByVin : IReturn<Vehicle>
    {
        public string Vin { get; set; }
    }

    [Route("vehicle/registration/{RegistrationNumber}", HttpMethods.Get)]
    public class VehicleByRegistration : IReturn<Vehicle>
    {
        public string RegistrationNumber { get; set; }
    }
}