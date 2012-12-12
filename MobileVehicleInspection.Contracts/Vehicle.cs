using System;
using System.Collections.Generic;

namespace MobileVehicleInspection.Contracts
{
    public class Vehicle
    {
        public string Vin { get; set; }
        public string RegistrationNumber { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public List<Inspection> Inspections { get; set; }
    }

    public class Inspection
    {
        public DateTime Date { get; set; }
        public int Mileage { get; set; }
        public string RegistrationNumber { get; set; }
        public InspectionResult Result { get; set; }
    }

    public enum InspectionResult
    {
        Unknown = 0,
        Approved,
        ConditionallyApproved,
        ReinspectionRequired,
        NotApproved
    }
}