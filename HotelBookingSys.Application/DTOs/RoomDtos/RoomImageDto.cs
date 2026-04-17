namespace HotelBookingSys.Application.DTOs.RoomDtos;

public class RoomImageDto
{
    public Guid Id { get; set; }

    public Guid RoomId { get; set; }

    public string Url { get; set; } = string.Empty;

    public string FileName { get; set; } = string.Empty;
}
