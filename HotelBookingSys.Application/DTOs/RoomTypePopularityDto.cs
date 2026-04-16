namespace HotelBookingSys.Application.DTOs;

public class RoomTypePopularityDto
{
    public string RoomType { get; set; } = string.Empty;

    public int BookingCount { get; set; }
}
