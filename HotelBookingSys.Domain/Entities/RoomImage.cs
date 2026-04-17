namespace HotelBookingSys.Domain.Entities;

public class RoomImage : BaseEntity
{
    public Guid RoomId { get; private set; }

    public string Url { get; private set; }

    public string FileName { get; private set; }

    public Room Room { get; private set; }

    private RoomImage()
    {
        Url = string.Empty;
        FileName = string.Empty;
        Room = null!;
        CreatedAt = DateTime.UtcNow;
    }

    public RoomImage(Guid roomId, string url, string fileName)
    {
        if (roomId == Guid.Empty)
            throw new ArgumentException("Room id is required.", nameof(roomId));

        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentException("Image url is required.", nameof(url));

        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("File name is required.", nameof(fileName));

        RoomId = roomId;
        Url = url;
        FileName = fileName;
        Room = null!;
    }
}
