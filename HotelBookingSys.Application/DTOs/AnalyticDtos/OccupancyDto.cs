namespace HotelBookingSys.Application.DTOs.AnalyticDtos;

public class OccupancyDto
{
    public int TotalRooms { get; set; }

    public int TotalNights { get; set; }

    public int BookedRoomNights { get; set; }

    public double OccupancyRate { get; set; }
}
