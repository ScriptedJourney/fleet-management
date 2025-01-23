using Microsoft.EntityFrameworkCore;
using VehicleMonitoring.DataStore.Interfaces;
using VehicleMonitoring.DataStore.Models;

namespace VehicleMonitoring.DataStore.Data.Repositories;

public class VehicleRepository : IVehicleRepository
{
    private readonly VehicleMonitoringContext _context;

    public VehicleRepository(VehicleMonitoringContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Vehicle>> GetAllAsync()
    {
        return await _context.Vehicles.ToListAsync();
    }

    public async Task<IEnumerable<Vehicle>> GetByCustomerIdAsync(int customerId)
    {
        return await _context.Vehicles
            .Where(v => v.Customer.Id == customerId)
            .ToListAsync();
    }
}
