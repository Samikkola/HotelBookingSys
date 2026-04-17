namespace HotelBookingSys.Application.DTOs.AnalyticDtos;

public class RoomTypePopularityDto
{
    public string RoomType { get; set; } = string.Empty;

    public int BookingCount { get; set; }

    public int TotalBookedNights { get; set; }
}
