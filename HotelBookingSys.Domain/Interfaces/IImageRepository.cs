using HotelBookingSys.Domain.Entities;

namespace HotelBookingSys.Domain.Interfaces;

public interface IImageRepository
{
    /// <summary>
    /// Retrieves an image by identifier.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<RoomImage?> GetByIdAsync(Guid id);

    /// <summary>
    /// Adds a room image.
    /// </summary>
    /// <param name="image"></param>
    /// <returns></returns>
    Task AddAsync(RoomImage image);

    /// <summary>
    /// Deletes a room image.
    /// </summary>
    /// <param name="image"></param>
    /// <returns></returns>
    Task DeleteAsync(RoomImage image);

    /// <summary>
    /// Retrieves images for the specified room.
    /// </summary>
    /// <param name="roomId"></param>
    /// <returns></returns>
    Task<IEnumerable<RoomImage>> GetByRoomIdAsync(Guid roomId);
}
