using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using FastEndpoints.Testing;
using Microsoft.EntityFrameworkCore;
using VehicleMonitoring.DataStore.Data;
using VehicleMonitoring.DataStore.Models;

namespace VehicleMonitoring.IntegrationTests
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
                    d => d.ServiceType == typeof(DbContextOptions<VehicleMonitoringDataContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<VehicleMonitoringDataContext>(options =>
                    options.UseInMemoryDatabase("TestDatabase"));

                using var scope = services.BuildServiceProvider().CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<VehicleMonitoringDataContext>();
                context.Database.EnsureCreated();
                SeedDatabase(context);
            });
        }

        private static void SeedDatabase(VehicleMonitoringDataContext context)
        {
            var customer = new Customer
            {
                Id = 1,
                Name = "Olof Carlsson",
                Address = "Some address 90210",
                Vehicles =
                [
                    new() { Vin = "ABCDEF12345", RegistrationNumber = "ABC 123", LastPing = DateTime.UtcNow },
                    new() { Vin = "IOIEPC89182", RegistrationNumber = "EYD 983", LastPing = DateTime.UtcNow }
                ]
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