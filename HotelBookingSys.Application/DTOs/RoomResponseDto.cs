using HotelBookingSys.Domain.Enums;

namespace HotelBookingSys.Application.DTOs;

public class RoomResponseDto
{
    // public Guid Id { get; set; } TODO: do I need this ?

    public int RoomNumber { get; set; }

    public  string Type { get; set; } = string.Empty;

    public int RoomCapacity { get; set; }

    public decimal BasePrice { get; set; }
}
