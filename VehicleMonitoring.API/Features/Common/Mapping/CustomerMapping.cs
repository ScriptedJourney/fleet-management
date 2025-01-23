using VehicleMonitoring.API.Features.Common.ApiModels;
using DataModels = VehicleMonitoring.DataStore.Models;

namespace VehicleMonitoring.API.Features.Common.Mapping;

public static class CustomerMapping
{
    public static Customer ToApiModel(this DataModels.Customer customer)
    {
        return new Customer
        {
            Id = customer.Id,
            Name = customer.Name,
            Address = customer.Address
        };
    }
}