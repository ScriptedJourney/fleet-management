using MediatR;
using VehicleMonitoring.API.Features.Common.ApiModels;
using VehicleMonitoring.API.Features.Common.Mapping;
using VehicleMonitoring.DataStore.Interfaces;

namespace VehicleMonitoring.API.Features.Vehicles.GetVehicleByCustomerEndpoint.Queries
{
    public abstract record VehicleResult
    {
        public sealed record Success(List<Vehicle> Vehicles) : VehicleResult;
        public sealed record NotFound : VehicleResult;
        public sealed record Error(string Message) : VehicleResult;
    } 

    public record GetVehiclesByCustomerRequest(int CustomerId) : IRequest<VehicleResult>;
    
    public class GetVehiclesByCustomerIdQueryHandler(
        IVehicleRepository vehicleRepository, 
        ICustomerRepository customerRepository) 
        : IRequestHandler<GetVehiclesByCustomerRequest, VehicleResult>
    {
        private readonly IVehicleRepository _vehicleRepository = vehicleRepository;
        private readonly ICustomerRepository _customerRepository = customerRepository;

        public async Task<VehicleResult> Handle(GetVehiclesByCustomerRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var vehicles = await _vehicleRepository.GetByCustomerIdAsync(request.CustomerId);
                var customer = await _customerRepository.GetByIdAsync(request.CustomerId);

                var enumerable = vehicles as DataStore.Models.Vehicle[] ?? vehicles.ToArray();
                if (enumerable.Length == 0)
                {
                    return new VehicleResult.NotFound();
                }

                var mappedCustomer = customer?.ToApiModel();
                var mappedVehicles = enumerable
                    .Select(v => v.ToApiModel(mappedCustomer))
                    .ToList();

                return new VehicleResult.Success(mappedVehicles);
            }
            catch (Exception ex)
            {
                return new VehicleResult.Error($"Failed to fetch vehicles: {ex.Message}");
            }
        }
    }
}
