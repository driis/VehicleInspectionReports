using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;

namespace MobileVehicleInspection.Contracts.Operations
{
    [Route("vehicle/vin/{Vin}", HttpMethods.Get)]
    public class VehicleByVin : IReturn<Vehicle>
    {
        public string Vin { get; set; }
    }
}