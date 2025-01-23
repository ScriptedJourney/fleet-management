using Microsoft.EntityFrameworkCore;
using VehicleMonitoring.DataStore.Interfaces;
using VehicleMonitoring.DataStore.Models;

namespace VehicleMonitoring.DataStore.Data.Repositories;

public class CustomerRepository(VehicleMonitoringContext context) : ICustomerRepository
{
    private readonly VehicleMonitoringContext _context = context;

    public async Task<IEnumerable<Customer>> GetAllAsync()
    {
        return await _context.Customers.ToListAsync();
    }

    public async Task<Customer?> GetByIdAsync(int id)
    {
        return await _context.Customers.FirstOrDefaultAsync(c => c.Id == id);
    }
}
