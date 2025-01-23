namespace VehicleMonitoring.DataStore.Models;

public record Customer
{
    public int Id { get; init; }
    public required string Name { get; init; }
    public required string Address { get; init; }
    public ICollection<Vehicle> Vehicles { get; init; } = [];
}