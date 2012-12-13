using System.Web;
using Funq;
using MobileVehicleInspection.Api.Library;
using ServiceStack.CacheAccess;
using ServiceStack.MiniProfiler;
using ServiceStack.Redis;
using ServiceStack.Text;
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
            JsConfig.EmitCamelCaseNames = true;            
            container.Register(new ScraperSettings{
                UrlTemplate = "http://selvbetjening.trafikstyrelsen.dk/Sider/resultater.aspx?{0}={1}"
            });
            container.RegisterAutoWired<DanishTransportAuthorityScraper>();
            container.RegisterAutoWired<DanishTransportAuthorityResponseParser>();
            container.Register<IVehicleInspectionLookup>(c => new CacheVehicleInspectionLookup(c.Resolve<ICacheClient>(), c.Resolve<DanishTransportAuthorityScraper>()));
            container.Register<IRedisClientsManager>(c => new PooledRedisClientManager("localhost:6379"));
            container.Register(c => c.Resolve<IRedisClientsManager>().GetCacheClient());
        }

        public static void Start()
        {
            new ServiceStackAppHost().Init();
        }
    }
}