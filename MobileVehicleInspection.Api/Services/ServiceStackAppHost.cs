using System.Configuration;
using System.Linq;
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

            string redisUrl = GetCleanedRedisUrl();
            container.RegisterAutoWired<DanishTransportAuthorityScraper>();
            container.RegisterAutoWired<DanishTransportAuthorityResponseParser>();
            container.Register<IVehicleInspectionLookup>(c => new CacheVehicleInspectionLookup(c.Resolve<ICacheClient>(), c.Resolve<DanishTransportAuthorityScraper>()));
            container.Register<IRedisClientsManager>(c => new PooledRedisClientManager(redisUrl));
            container.Register(c => c.Resolve<IRedisClientsManager>().GetCacheClient());
        }

        /// <summary>
        /// Cleans up the Redis URL that are injected by RedisCloud, so it can be used with the ServiceStack Redis client.
        /// </summary>
        /// <returns></returns>
        private static string GetCleanedRedisUrl()
        {
            string url = ConfigurationManager.AppSettings["REDISCLOUD_URL"] ?? "localhost:6379";
            url = url.Replace("redis://rediscloud:", string.Empty);
            return url;
        }

        public static void Start()
        {
            new ServiceStackAppHost().Init();
        }
    }
}