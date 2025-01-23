using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using FastEndpoints.Testing;
using Microsoft.EntityFrameworkCore;
using VehicleMonitoring.DataStore.Data;
using VehicleMonitoring.DataStore.Models;

namespace VehicleMonitoring.API.Tests
{
    public class App : AppFixture<Program>
    {
        protected override ValueTask SetupAsync()
        {
            return ValueTask.CompletedTask;
        }

        protected override void ConfigureApp(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<VehicleMonitoringContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<VehicleMonitoringContext>(options =>
                    options.UseInMemoryDatabase("TestDatabase"));

                using var scope = services.BuildServiceProvider().CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<VehicleMonitoringContext>();
                context.Database.EnsureCreated();
                SeedDatabase(context);
            });
        }

        private static void SeedDatabase(VehicleMonitoringContext context)
        {
            var customer = new Customer
            {
                Id = 1,
                Name = "Test Customer",
                Address = "Test Address",
                Vehicles = new List<Vehicle>
            {
                new Vehicle { Vin = "Test VIN", RegistrationNumber = "TEST123", LastPing = DateTime.UtcNow }
            }
            };

            context.Customers.Add(customer);
            context.SaveChanges();
        }

        protected override void ConfigureServices(IServiceCollection s)
        {
        }

        protected override ValueTask TearDownAsync()
        {
            return ValueTask.CompletedTask;
        }
    }
}