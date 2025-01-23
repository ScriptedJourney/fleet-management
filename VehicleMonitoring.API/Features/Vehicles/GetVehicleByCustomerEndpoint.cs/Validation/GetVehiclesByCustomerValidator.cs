using FastEndpoints;
using FluentValidation;
using VehicleMonitoring.API.Features.Vehicles.GetVehicleByCustomerEndpoint.cs.Queries;

namespace VehicleMonitoring.API.Features.Vehicles.GetVehicleByCustomerEndpoint.cs.Validation;

public class GetVehiclesByCustomerValidator : Validator<GetVehiclesByCustomerRequest>
{
    public GetVehiclesByCustomerValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty()
            .WithMessage("Customer ID is required")
            .GreaterThan(0)
            .WithMessage("Customer ID must be greater than 0");
    }
} 