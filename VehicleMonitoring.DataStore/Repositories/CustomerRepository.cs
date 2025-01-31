using Microsoft.EntityFrameworkCore;
using VehicleMonitoring.DataStore.Data;
using VehicleMonitoring.DataStore.Interfaces;
using VehicleMonitoring.DataStore.Models;

namespace VehicleMonitoring.DataStore.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly VehicleMonitoringContext _context;

    public CustomerRepository(VehicleMonitoringContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Customer>> GetAllAsync()
    {
        return await _context.Customers
            .Include(c => c.Vehicles)
            .ToListAsync();
    }

    public async Task<Customer?> GetByIdAsync(int id)
    {
        return await _context.Customers
            .Include(c => c.Vehicles)
            .FirstOrDefaultAsync(c => c.Id == id);
    }
}