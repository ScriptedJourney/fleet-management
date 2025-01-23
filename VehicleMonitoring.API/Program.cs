using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using VehicleMonitoring.DataStore.Data;
using VehicleMonitoring.DataStore.Interfaces;
using VehicleMonitoring.DataStore.Repositories;
using System.Text.Json;
using System.Text.Json.Serialization;
using FastEndpoints.Swagger;

namespace VehicleMonitoring.API;

public partial class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddFastEndpoints()
            .SwaggerDocument(o => 
            {
                o.ShortSchemaNames = true;
                o.DocumentSettings = s =>
                {
                    s.Title = "Vehicle Monitoring API";
                    s.Version = "v1";
                };
            });

        builder.Services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

        builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
        builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

        var jsonOptions = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        if (builder.Environment.EnvironmentName != "Testing")
        {
            builder.Services.AddDbContextPool<VehicleMonitoringContext>(options =>
            {
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
        }

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAngular",
                builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
        });

        builder.WebHost.UseUrls("http://localhost:5001", "https://localhost:7001");

        var app = builder.Build();

        app.UseHttpsRedirection();
        app.UseCors("AllowAngular");

        app.UseFastEndpoints(c => 
        {
            c.Serializer.Options.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            c.Serializer.Options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            c.Endpoints.RoutePrefix = "api";
        });

        if (app.Environment.IsDevelopment())
        {
            app.UseSwaggerGen();
        }

        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<VehicleMonitoringContext>();
            context.Database.EnsureCreated();
        }

        app.Run();
    }
}