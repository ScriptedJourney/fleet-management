namespace VehicleMonitoring.DataStore.Models;
    
public record Vehicle
{
    public string Vin { get; init; } = string.Empty;
    public string RegistrationNumber { get; init; } = string.Empty;
    public int CustomerId { get; init; }
    public Customer? Customer { get; init; }
    public DateTime LastPing { get; init; }
}
