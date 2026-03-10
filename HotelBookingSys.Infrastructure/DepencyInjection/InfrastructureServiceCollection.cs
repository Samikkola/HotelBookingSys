using HotelBookingSys.Application.Interfaces;
using HotelBookingSys.Infrastructure;
using HotelBookingSys.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace HotelBookingSys.Infrastructure.DepencyInjection;

public static class InfrastructureServiceCollection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {

        //Register the in-memory database as a singleton
        services.AddSingleton<InMemoryDatabase>();

        // Register infrastructure services
        services.AddScoped<IReservationRepository, InMemoryReservationRepository>();
        services.AddScoped<ICustomerRepository, InMemoryCustomerRepository>();
        services.AddScoped<IRoomRepository, InMemoryRoomRepository>();
        
        return services;
    }
}
