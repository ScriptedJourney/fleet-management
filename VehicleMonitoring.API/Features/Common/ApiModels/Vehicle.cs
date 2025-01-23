namespace VehicleMonitoring.API.Features.Common.ApiModels;

public record Vehicle
{
    public required string IdentificationNumber { get; init; }
    public required string RegNumber { get; init; }
    public DateTime? LastPing { get; init; }
    public bool IsConnected { get; init; }
    public required Customer? Customer { get; init; }
}