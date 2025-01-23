using MediatR;
using VehicleMonitoring.API.Features.Common.ApiModels;
using VehicleMonitoring.API.Features.Common.Mapping;
using VehicleMonitoring.DataStore.Interfaces;

namespace VehicleMonitoring.API.Features.Vehicles.GetVehicles.Queries;

public abstract record VehiclesResult
{
    public sealed record Success(List<Vehicle> Vehicles) : VehiclesResult;
    public sealed record NotFound : VehiclesResult;
    public sealed record Error(string Message) : VehiclesResult;
}

public record GetVehiclesQuery : IRequest<VehiclesResult>;

public class GetVehiclesQueryHandler : IRequestHandler<GetVehiclesQuery, VehiclesResult>
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly ICustomerRepository _customerRepository;

    public GetVehiclesQueryHandler(
        IVehicleRepository vehicleRepository,
        ICustomerRepository customerRepository)
    {
        _vehicleRepository = vehicleRepository ?? throw new ArgumentNullException(nameof(vehicleRepository));
        _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
    }

    public async Task<VehiclesResult> Handle(GetVehiclesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var vehicles = await _vehicleRepository.GetAllAsync();

            var enumerable = vehicles.ToList();
            if (enumerable.Count == 0)
                return new VehiclesResult.NotFound();

            var customerIds = enumerable.Select(v => v.CustomerId).Distinct();

            var customers = await Task.WhenAll(
                customerIds.Select(_customerRepository.GetByIdAsync)
            );

            var customerLookup = customers
                .Where(c => c != null)
                .ToDictionary(
                    c => c!.Id,
                    c => c?.ToApiModel());
            if (customerLookup == null) throw new InvalidOperationException(nameof(customerLookup));

            var mappedVehicles = enumerable
                .Select(v =>
                {
                    customerLookup.TryGetValue((int)v.CustomerId, out var customer);
                    return v.ToApiModel(customer);
                })
                .ToList();

            return new VehiclesResult.Success(mappedVehicles);
        }
        catch (Exception ex)
        {
            return new VehiclesResult.Error($"Failed to fetch vehicles: {ex.Message}");
        }
    }
}
