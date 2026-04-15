using HotelBookingSys.Domain.Enums;


namespace HotelBookingSys.Domain.Entities;

public class Room : BaseEntity
{
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

        RoomNumber = roomNumber;
        Type = type;
        RoomCapacity = roomCapacity;
        BasePrice = basePrice;

    }

    //Update, Create ?
}
