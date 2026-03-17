using HotelBookingSys.Infrastructure.Data;
using HotelBookingSys.Domain.Entities;
using HotelBookingSys.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSys.Infrastructure.Seeders;

public static class DatabaseSeeder
{
    public static void Seed(ApplicationDbContext context)
    {
        // Check if database already contains rooms to prevent duplicate seeding
        if (context.Rooms.Any())
        {
            return;
        }

        var rooms = new List<Room>();
        int roomNumber = 101;

        // Economy (8 rooms)
        for (int i = 0; i < 8; i++)
        {
            rooms.Add(new Room(roomNumber++, RoomType.Economy, 1, 79));
        }

        // Standard (10 rooms)
        for (int i = 0; i < 10; i++)
        {
            rooms.Add(new Room(roomNumber++, RoomType.Standard, 2, 119));
        }

        // Superior (6 rooms)
        for (int i = 0; i < 6; i++)
        {
            rooms.Add(new Room(roomNumber++, RoomType.Superior, 2, 159));
        }

        // Junior Suite (4 rooms)
        for (int i = 0; i < 4; i++)
        {
            rooms.Add(new Room(roomNumber++, RoomType.JuniorSuite, 3, 219));
        }

        // Suite (2 rooms)
        for (int i = 0; i < 2; i++)
        {
            rooms.Add(new Room(roomNumber++, RoomType.Suite, 4, 319));
        }

        context.Rooms.AddRange(rooms);

        // Seed a test customer
        if (!context.Customers.Any())
        {
            var customer = new Customer("John", "Doe", "john@example.com", "020202 020202");
            context.Customers.Add(customer);
        }

        context.SaveChanges();
    }
}
