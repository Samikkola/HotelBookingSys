using HotelBookingSys.Application.Interfaces;
using HotelBookingSys.Domain.Interfaces;
using HotelBookingSys.Infrastructure.Data;
using HotelBookingSys.Infrastructure.Repositories;
using HotelBookingSys.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HotelBookingSys.Infrastructure.DepencyInjection;

/// <summary>
/// Extension method to register infrastructure services in the dependency injection container.
/// </summary>
public static class InfrastructureServiceCollection
{
    /// <summary>
    /// Registers infrastructure services, including the SQL Server database context.
    /// </summary>
    /// <param name="services">The dependency injection service collection.</param>
    /// <param name="connectionString">The database connection string.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        string connectionString)
    {



        // Register infrastructure services
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<IReservationRepository, ReservationRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IRoomRepository, RoomRepository>();
        services.AddScoped<IImageRepository, ImageRepository>();

        var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

        if (string.Equals(environmentName, "Development", StringComparison.OrdinalIgnoreCase))
        {
            services.AddScoped<IImageStorageService, LocalImageStorageService>();
        }
        else
        {
            var azureStorageConnectionString = Environment.GetEnvironmentVariable("AzureStorage__ConnectionString");
            if (string.IsNullOrWhiteSpace(azureStorageConnectionString))
                throw new InvalidOperationException("Configuration value 'AzureStorage__ConnectionString' is missing or empty.");

            services.AddScoped<IImageStorageService>(_ => new AzureBlobStorageService(azureStorageConnectionString));
        }
        
        return services;
    }
}
