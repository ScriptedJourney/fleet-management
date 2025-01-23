namespace VehicleMonitoring.API.Features.Common.ApiModels;

public record Customer
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required string Address { get; init; }
}