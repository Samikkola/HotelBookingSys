using HotelBookingSys.Domain.Interfaces;
using HotelBookingSys.Infrastructure.Data;
using HotelBookingSys.Infrastructure.Repositories;
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
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {



        // Register infrastructure services
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<IReservationRepository, ReservationRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IRoomRepository, RoomRepository>();
        
        return services;
    }
}
