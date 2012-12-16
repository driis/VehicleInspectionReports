using System;
using ServiceStack.Redis;
using ServiceStack.ServiceHost;

namespace MobileVehicleInspection.Api.Services
{
    public class ApiKeyFilter
    {
        private readonly IRedisClient _redisClient;
        public const string ApiKeyUsageKey = "ApiKeyUsage";

        public ApiKeyFilter(IRedisClient redisClient)
        {
            _redisClient = redisClient;
        }

        public void Authorize(IHttpRequest request)
        {
            string apiKey = request.Headers["X-ApiKey"] ?? request.QueryString["ApiKey"];
            if (apiKey == null)
                throw new UnauthorizedAccessException("Access not allowed without a Api key.");
            
            string redisKey = string.Format("ApiKey-{0}", apiKey);
            bool allowed = _redisClient.Get<ApiKey>(redisKey) != null;
            
            if (!allowed)
                throw new UnauthorizedAccessException(string.Format("Invalid ApiKey {0}", apiKey));

            _redisClient.IncrementItemInSortedSet(ApiKeyUsageKey, apiKey, 1);
            request.Items["RequestAuthorized"] = DateTime.Now;
        }
    }
}