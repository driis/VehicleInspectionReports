using System;

namespace MobileVehicleInspection.Api.Library
{
    public class VehicleIdentificationNumber
    {
        
        private readonly string _value;

        public VehicleIdentificationNumber(string registration)
        {
            Guard.NotNullOrWhitespace(() => registration);
            string cleaned = registration.Trim().Replace(" ", String.Empty);
            
            
            _value = cleaned;
        }

        public string Value
        {
            get { return _value; }
        }
    }
}