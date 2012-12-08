using System.Web;
using Funq;
using ServiceStack.WebHost.Endpoints;

[assembly: PreApplicationStartMethod(typeof(MobileVehicleInspection.Api.Services.ServiceStackAppHost), "Start")]

namespace MobileVehicleInspection.Api.Services
{

    public class ServiceStackAppHost : AppHostBase
    {
        public ServiceStackAppHost() : base("VehicleInspections", typeof(ServiceStackAppHost).Assembly)
        {
        }

        public override void Configure(Container container)
        {           
        }

        public static void Start()
        {
            new ServiceStackAppHost().Init();
        }
    }
}