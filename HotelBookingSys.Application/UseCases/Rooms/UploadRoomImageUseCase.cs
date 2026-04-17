using HotelBookingSys.Application.Common.Result;
using HotelBookingSys.Application.DTOs.RoomDtos;
using HotelBookingSys.Application.Interfaces;
using HotelBookingSys.Domain.Entities;
using HotelBookingSys.Domain.Interfaces;

namespace HotelBookingSys.Application.UseCases.Rooms;

public class UploadRoomImageUseCase
{
    private readonly IRoomRepository _roomRepository;
    private readonly IImageRepository _imageRepository;
    private readonly IImageStorageService _imageStorageService;

    public UploadRoomImageUseCase(
        IRoomRepository roomRepository,
        IImageRepository imageRepository,
        IImageStorageService imageStorageService)
    {
        _roomRepository = roomRepository;
        _imageRepository = imageRepository;
        _imageStorageService = imageStorageService;
    }

    /// <summary>
    /// Uploads an image for the specified room and saves metadata.
    /// </summary>
    /// <param name="roomId"></param>
    /// <param name="fileStream"></param>
    /// <param name="fileName"></param>
    /// <param name="contentType"></param>
    /// <returns></returns>
    public async Task<Result<RoomImageDto>> ExecuteAsync(Guid roomId, Stream fileStream, string fileName, string contentType)
    {
        if (roomId == Guid.Empty)
            return Result<RoomImageDto>.Failure(ErrorCode.Validation, "Room id is required.");

        if (fileStream is null)
            return Result<RoomImageDto>.Failure(ErrorCode.Validation, "Image file is required.");

        if (string.IsNullOrWhiteSpace(fileName))
            return Result<RoomImageDto>.Failure(ErrorCode.Validation, "File name is required.");

        if (string.IsNullOrWhiteSpace(contentType))
            return Result<RoomImageDto>.Failure(ErrorCode.Validation, "Content type is required.");

        var room = await _roomRepository.GetByIdAsync(roomId);
        if (room is null)
            return Result<RoomImageDto>.Failure(ErrorCode.NotFound, "Room not found.");

        var storedFileName = $"{Guid.NewGuid():N}_{Path.GetFileName(fileName)}";
        var url = await _imageStorageService.UploadAsync(fileStream, storedFileName, contentType);

        RoomImage image;
        try
        {
            image = new RoomImage(roomId, url, storedFileName);
        }
        catch (ArgumentException ex)
        {
            return Result<RoomImageDto>.Failure(ErrorCode.Validation, ex.Message);
        }

        await _imageRepository.AddAsync(image);

        return Result<RoomImageDto>.Success(new RoomImageDto
        {
            Id = image.Id,
            RoomId = image.RoomId,
            Url = image.Url,
            FileName = image.FileName
        });
    }
}
