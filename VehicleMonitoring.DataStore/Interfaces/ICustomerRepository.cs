using VehicleMonitoring.DataStore.Models;

namespace VehicleMonitoring.DataStore.Interfaces;

public interface ICustomerRepository
{
    Task<IEnumerable<Customer>> GetAllAsync();
    Task<Customer?> GetByIdAsync(int id);
}