using Microsoft.EntityFrameworkCore;
using VehicleMonitoring.DataStore.Models;
using Microsoft.Extensions.Hosting;

namespace VehicleMonitoring.DataStore.Data;

public class VehicleMonitoringDataContext : DbContext
{
    private readonly bool _isTestEnvironment;
    private readonly IHostEnvironment? _env;

    public VehicleMonitoringDataContext(
        DbContextOptions<VehicleMonitoringDataContext> options,
        IHostEnvironment? env = null)
        : base(options)
    {
        _env = env;
        _isTestEnvironment = _env?.EnvironmentName == "Testing";
    }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Vehicle>()
            .HasKey(v => v.Vin);

        modelBuilder.Entity<Vehicle>()
            .HasOne(v => v.Customer)
            .WithMany(c => c.Vehicles)
            .HasForeignKey(v => v.CustomerId);

        if (!_isTestEnvironment)
        {
            modelBuilder.Entity<Customer>().HasData(
                new Customer { Id = 1, Name = "Kalles Grustransporter AB", Address = "Cementvägen 8, 111 11 Södertälje" },
                new Customer { Id = 2, Name = "Johans Bulk AB", Address = "Balkvägen 12, 222 22 Stockholm" },
                new Customer { Id = 3, Name = "Haralds Värdetransporter AB", Address = "Budgetvägen 1, 333 33 Uppsala" }
            );

            modelBuilder.Entity<Vehicle>().HasData(
                new Vehicle { Vin = "YS2R4X20005399401", RegistrationNumber = "ABC123", CustomerId = 1 },
                new Vehicle { Vin = "VLUR4X20009093588", RegistrationNumber = "DEF456", CustomerId = 1 },
                new Vehicle { Vin = "VLUR4X20009048066", RegistrationNumber = "GHI789", CustomerId = 1 },
                new Vehicle { Vin = "YS2R4X20005388011", RegistrationNumber = "JKL012", CustomerId = 2 },
                new Vehicle { Vin = "YS2R4X20005387949", RegistrationNumber = "MNO345", CustomerId = 2 },
                new Vehicle { Vin = "YS2R4X20005387765", RegistrationNumber = "PQR678", CustomerId = 3 },
                new Vehicle { Vin = "YS2R4X20005387055", RegistrationNumber = "STU901", CustomerId = 3 }
            );
        }
    }
}