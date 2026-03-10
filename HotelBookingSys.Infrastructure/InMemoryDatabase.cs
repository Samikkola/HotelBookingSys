using System;
using System.Collections.Generic;
using System.Text;
using HotelBookingSys.Domain.Entities;
using HotelBookingSys.Domain.Enums;

namespace HotelBookingSys.Infrastructure;

/// <summary>
/// InMemory database for MVP -phase to store customers, rooms and reservations in memory. 
/// </summary>
public class InMemoryDatabase
{   
    public List<Customer> Customers { get; set; } = new List<Customer>();
    public List<Room> Rooms { get; set; } = new List<Room>();
    public List<Reservation> Reservations { get; set; } = new List<Reservation>();

    //Constructor to seed initial data for rooms, customers and reservations for testing purposes.
    public InMemoryDatabase()
    {
        SeedRooms();
        SeedCustomers();
    }

    //Seeds the right amount of rooms for each room type with unique room numbers, base prices and capacities as specified in the requirements.
    private void SeedRooms()
    {
        int roomNumber = 101;

        // Economy (8 rooms)
        for (int i = 0; i < 8; i++)
        {
            Rooms.Add(new Room(roomNumber++, RoomType.Economy, 1, 79));
        }

        // Standard (10 rooms)
        for (int i = 0; i < 10; i++)
        {
            Rooms.Add(new Room(roomNumber++, RoomType.Standard, 2, 119));
        }

        // Superior (6 rooms)
        for (int i = 0; i < 6; i++)
        {
            Rooms.Add(new Room(roomNumber++, RoomType.Superior, 2, 159));
        }

        // Junior Suite (4 rooms)
        for (int i = 0; i < 4; i++)
        {
            Rooms.Add(new Room(roomNumber++, RoomType.JuniorSuite, 3, 219));
        }

        // Suite (2 rooms)
        for (int i = 0; i < 2; i++)
        {
            Rooms.Add(new Room(roomNumber++, RoomType.Suite, 4, 319));
        }
    }

    //Seeds a single customer for testing purposes.
    private void SeedCustomers()
    {
        Customers.Add(
            new Customer(
                "John",
                "Doe",
                "john@example.com",
                "020202 020202"
            )
        );
    }
}
