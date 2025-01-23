using VehicleMonitoring.DataStore.Models;

namespace VehicleMonitoring.DataStore.Interfaces;

public interface IVehicleRepository
{
    Task<IEnumerable<Vehicle>> GetAllAsync();
    Task<IEnumerable<Vehicle>> GetByCustomerIdAsync(int customerId);
} 
