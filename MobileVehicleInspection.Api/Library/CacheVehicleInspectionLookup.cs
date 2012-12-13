using System;
using MobileVehicleInspection.Contracts;
using ServiceStack.CacheAccess;

namespace MobileVehicleInspection.Api.Library
{
    public class CacheVehicleInspectionLookup : IVehicleInspectionLookup
    {
        private readonly ICacheClient _client;
        private readonly IVehicleInspectionLookup _inner;

        public CacheVehicleInspectionLookup(ICacheClient client, IVehicleInspectionLookup inner)
        {
            _client = client;
            _inner = inner;
        }

        public Vehicle ByRegistration(RegistrationNumber registration)
        {
            return GetCached(registration, _inner.ByRegistration);
        }

        public Vehicle ByVehicleIdentificationNumber(VehicleIdentificationNumber vin)
        {
            return GetCached(vin, _inner.ByVehicleIdentificationNumber);
        }

        private Vehicle GetCached<TKey>(TKey key, Func<TKey, Vehicle> getter)
        {
            string formattedKey = String.Format("{0}_{1}", key.GetType().Name, key);
            var v = _client.Get<Vehicle>(formattedKey);
            if (v == null)
            {
                v = getter(key);
                _client.Set(formattedKey, v, TimeSpan.FromHours(2));
            }
            return v;
        }
    }
}