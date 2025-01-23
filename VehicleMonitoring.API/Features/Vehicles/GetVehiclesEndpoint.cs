using FastEndpoints;
using MediatR;
using VehicleMonitoring.API.Features.Common.ApiModels;
using VehicleMonitoring.API.Features.Vehicles.GetVehicles.Queries;

namespace VehicleMonitoring.API.Features.Vehicles;

public class GetVehiclesEndpoint : EndpointWithoutRequest<List<Vehicle>>
{
    private readonly IMediator _mediator;

    public GetVehiclesEndpoint(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public override void Configure()
    {
        Get("vehicles");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Gets all vehicles";
            s.Description = "Retrieves a list of all vehicles with their status and customer information";
            s.Responses[200] = "Successfully retrieved the vehicles";
            s.Responses[404] = "No vehicles found";
            s.Responses[500] = "Error occurred while fetching vehicles";
        });
        Options(x => x.WithTags("Vehicles"));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var query = new GetVehiclesQuery();
        var result = await _mediator.Send(query, ct);

        await (result switch
        {
            VehiclesResult.Success success => SendOkAsync(success.Vehicles, ct),
            VehiclesResult.NotFound => SendNotFoundAsync(ct),
            VehiclesResult.Error error => SendErrorsAsync(500, ct),
            _ => throw new InvalidOperationException($"Unexpected result type: {result}")
        });
    }
}