using System;
using System.Linq;

namespace MobileVehicleInspection.Api.Library
{
    public class RegistrationNumber : IEquatable<RegistrationNumber>
    {
        private const int MinLength = 2;
        private const int MaxLength = 7;

        private readonly string _value;
        
        public RegistrationNumber(string registration)
        {
            Guard.NotNullOrWhitespace(() => registration);
            string cleaned = registration
                .Trim()
                .Replace(" ", String.Empty)
                .ToUpperInvariant();
            if (cleaned.Length < MinLength)
                throw new ArgumentException(String.Format("{0} is not a valid registration number a minimum length of {1} is required.", registration, MinLength), "registration");
            if (cleaned.Length > MaxLength)
                throw new ArgumentException(String.Format("{0} is not a valid registration number a maximum length of {1} is required.", registration, MaxLength), "registration");
            if (cleaned.Any(ch => !Char.IsLetterOrDigit(ch)))
                throw new ArgumentException(String.Format("{0} is not a valid registration number; illegal characters encountered.", registration));
            
            _value = cleaned;
        }

        public string Value
        {
            get { return _value; }
        }

        public bool Equals(RegistrationNumber other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return string.Equals(_value, other._value);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as RegistrationNumber);
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public override string ToString()
        {
            return Value;
        }

        public static bool operator ==(RegistrationNumber left, RegistrationNumber right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(RegistrationNumber left, RegistrationNumber right)
        {
            return !Equals(left, right);
        }

        public static implicit operator string(RegistrationNumber right)
        {
            if (right == null)
                return null;
            return right.Value;
        }
    }
}