using System;
using System.Linq;
using ServiceStack.Redis;
using ServiceStack.ServiceHost;

namespace MobileVehicleInspection.Api.Services
{
    [MustBeLocal]
    public class ApiKeyService : IService
    {
        private readonly IRedisClient _redisClient;

        public ApiKeyService(IRedisClient redisClient)
        {
            _redisClient = redisClient;
        }

        public object Get(ApiKeysRequest request)
        {
            var keyLookup = _redisClient.GetAllWithScoresFromSortedSet(ApiKeyFilter.ApiKeyUsageKey);
            if (keyLookup.Count == 0)
                return new ApiKeyWithUsage[0];

            var itemKeys = keyLookup.Keys.Select(GetItemKey);
            var apiKeys = _redisClient.GetAll<ApiKey>(itemKeys);
            return apiKeys.Values.Select(apiKey => new ApiKeyWithUsage()
                {
                    Key = apiKey.Key,
                    Name = apiKey.Name,
                    UsageCount = (long) keyLookup[apiKey.Key]
                }).ToArray();
        }

        public object Post(NewApiKeyRequest request)
        {
            if (request.Name ==null)
                throw new ArgumentException("No Name for Key");
            if (request.Key == null)
                request.Key = Guid.NewGuid().ToString("N");

            var itemKey = GetItemKey(request.Key);
            if (_redisClient.Get<ApiKey>(itemKey) != null)
                throw new ArgumentException("Already exists");

            var apiKey = new ApiKey() {Name = request.Name, Key = request.Key};
            _redisClient.Set(itemKey, apiKey);
            _redisClient.IncrementItemInSortedSet(ApiKeyFilter.ApiKeyUsageKey, request.Key, 0);
            return Get(new ApiKeysRequest());
        }

        private static string GetItemKey(string rawKey)
        {
            return "ApiKey-" + rawKey;
        }
    }

    [Route("apikeys", "POST")]
    public class NewApiKeyRequest : IReturn<ApiKey[]> 
    {
        public string Name { get; set; }
        public string Key { get; set; }
    }

    [Route("apikeys", "GET")]
    public class ApiKeysRequest : IReturn<ApiKeyWithUsage[]> {}

    public class ApiKeyWithUsage
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public long UsageCount { get; set; }
    }
}