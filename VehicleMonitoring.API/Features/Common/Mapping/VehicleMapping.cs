using VehicleMonitoring.API.Features.Common.ApiModels;
using DataModels = VehicleMonitoring.DataStore.Models;

namespace VehicleMonitoring.API.Features.Common.Mapping;

public static class VehicleMapping
{
    private static readonly Random Random = new();

    public static Vehicle ToApiModel(this DataModels.Vehicle vehicle, Customer? customer = null)
    {
        // Simulate 80% chance of being connected
        var isConnected = Random.NextDouble() < 0.8;
        
        return new Vehicle
        {
            IdentificationNumber = vehicle.Vin,
            RegNumber = vehicle.RegistrationNumber,
            LastPing = isConnected ? DateTime.UtcNow : null,
            IsConnected = isConnected,
            Customer = customer
        };
    }
}