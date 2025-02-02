using Microsoft.EntityFrameworkCore;
using VehicleMonitoring.DataStore.Data;
using VehicleMonitoring.DataStore.Interfaces;
using VehicleMonitoring.DataStore.Models;

namespace VehicleMonitoring.DataStore.Repositories;

public class VehicleRepository : IVehicleRepository
{
    private readonly VehicleMonitoringDataContext _context;

    public VehicleRepository(VehicleMonitoringDataContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Vehicle>> GetAllAsync()
    {
        return await _context.Vehicles
            .Include(v => v.Customer)
            .ToListAsync();
    }

    public async Task<IEnumerable<Vehicle>> GetByCustomerIdAsync(int customerId)
    {
        return await _context.Vehicles
            .Include(v => v.Customer)
            .Where(v => v.CustomerId == customerId)
            .ToListAsync();
    }
}
