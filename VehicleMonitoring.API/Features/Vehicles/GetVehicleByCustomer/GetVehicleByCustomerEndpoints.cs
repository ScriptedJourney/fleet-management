using FastEndpoints;
using MediatR;
using VehicleMonitoring.API.Features.Common.ApiModels;
using VehicleMonitoring.API.Features.Vehicles.GetVehicleByCustomerEndpoint.Queries;

namespace VehicleMonitoring.API.Features.Vehicles.GetVehicleByCustomerEndpoint;

public class GetVehiclesByCustomerEndpoint : Endpoint<GetVehiclesByCustomerRequest, List<Vehicle>>
{
    private readonly IMediator _mediator;

    public GetVehiclesByCustomerEndpoint(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public override void Configure()
    {
        Get("vehicles/by-customer/{CustomerId}");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Gets all vehicles for a specific customer";
            s.Description = "Retrieves a list of all vehicles associated with the specified customer ID";
            s.Responses[200] = "Successfully retrieved the vehicles";
            s.Responses[400] = "Invalid customer ID";
            s.Responses[404] = "No vehicles found for the customer";
            s.Responses[500] = "Error occurred while fetching vehicles";
        });
        Options(x => x.WithTags("Vehicles"));
    }

    public override async Task HandleAsync(GetVehiclesByCustomerRequest req, CancellationToken ct)
    {
        var query = new GetVehiclesByCustomerRequest(req.CustomerId);
        var result = await _mediator.Send(query, ct);

        await (result switch
        {
            VehicleResult.Success success => SendOkAsync(success.Vehicles, ct),
            VehicleResult.NotFound => SendNotFoundAsync(ct),
            VehicleResult.Error => SendErrorsAsync(500, ct),
            _ => throw new InvalidOperationException($"Unexpected result type: {result}")
        });
    }
}