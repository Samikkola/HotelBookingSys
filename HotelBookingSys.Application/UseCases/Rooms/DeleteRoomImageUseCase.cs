using HotelBookingSys.Application.Common.Result;
using HotelBookingSys.Application.Interfaces;
using HotelBookingSys.Domain.Interfaces;

namespace HotelBookingSys.Application.UseCases.Rooms;

public class DeleteRoomImageUseCase
{
    private readonly IRoomRepository _roomRepository;
    private readonly IImageRepository _imageRepository;
    private readonly IImageStorageService _imageStorageService;

    public DeleteRoomImageUseCase(
        IRoomRepository roomRepository,
        IImageRepository imageRepository,
        IImageStorageService imageStorageService)
    {
        _roomRepository = roomRepository;
        _imageRepository = imageRepository;
        _imageStorageService = imageStorageService;
    }

    /// <summary>
    /// Deletes an image for the specified room.
    /// </summary>
    /// <param name="roomId"></param>
    /// <param name="imageId"></param>
    /// <returns></returns>
    public async Task<Result> ExecuteAsync(Guid roomId, Guid imageId)
    {
        if (roomId == Guid.Empty)
            return Result.Failure(ErrorCode.Validation, "Room id is required.");

        if (imageId == Guid.Empty)
            return Result.Failure(ErrorCode.Validation, "Image id is required.");

        var room = await _roomRepository.GetByIdAsync(roomId);
        if (room is null)
            return Result.Failure(ErrorCode.NotFound, "Room not found.");

        var image = await _imageRepository.GetByIdAsync(imageId);
        if (image is null)
            return Result.Failure(ErrorCode.NotFound, "Image not found.");

        if (image.RoomId != roomId)
            return Result.Failure(ErrorCode.Conflict, "Image does not belong to the specified room.");

        await _imageStorageService.DeleteAsync(image.FileName);
        await _imageRepository.DeleteAsync(image);

        return Result.Success();
    }
}
