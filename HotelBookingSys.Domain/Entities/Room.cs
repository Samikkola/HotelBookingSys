using HotelBookingSys.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelBookingSys.Domain.Entities;

public class Room
{
    public Guid Id { get; private set; }
    public int RoomNumber { get; private set; }
    public RoomType Type { get; private set; }
    public int RoomCapacity { get; private set; }
    public decimal BasePrice { get; private set; }

    //Navigation property?

    // Constructor
    public Room(int roomNumber, RoomType type, int roomCapacity, decimal basePrice)
    {
        //Domain validation
        if (roomNumber <= 0)
            throw new ArgumentException("Room number must be greater than zero", nameof(roomNumber));

        if (roomCapacity <= 0)
            throw new ArgumentException("Max persons must be greater than zero", nameof(roomCapacity));

        if (basePrice < 0)
            throw new ArgumentException("Base price cannot be negative", nameof(basePrice));

        Id = Guid.NewGuid();
        RoomNumber = roomNumber;
        Type = type;
        RoomCapacity = roomCapacity;
        BasePrice = basePrice;
    }

    //Update ?
}
