using HotelBookingSys.Application.Interfaces;
using HotelBookingSys.Infrastructure;
using HotelBookingSys.Infrastructure.Data;
using HotelBookingSys.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HotelBookingSys.Infrastructure.DepencyInjection;

public static class InfrastructureServiceCollection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {

        //KEEP FOR TESTING AND DEVELOPMENT PURPOSES IF NEEDED
        ////Register the in-memory database as a singleton
        //services.AddSingleton<InMemoryDatabase>();

        ////REgister in-memory repositories for testing and development
        //services.AddScoped<IReservationRepository, InMemoryReservationRepository>();
        //services.AddScoped<ICustomerRepository, InMemoryCustomerRepository>();
        //services.AddScoped<IRoomRepository, InMemoryRoomRepository>();

        // Register infrastructure services
        services.AddScoped<IReservationRepository, ReservationRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IRoomRepository, RoomRepository>();
        
        return services;
    }
}
