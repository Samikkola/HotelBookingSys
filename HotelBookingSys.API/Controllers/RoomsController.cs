using HotelBookingSys.Application.Common.Result;
using HotelBookingSys.Application.DTOs.RoomDtos;
using HotelBookingSys.Application.UseCases.Rooms;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingSys.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomsController : BaseController
{
    private static readonly HashSet<string> AllowedImageExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".jpg", ".jpeg", ".png", ".webp"
    };

    private const long MaxImageSizeInBytes = 5 * 1024 * 1024;

    private readonly GetAllRoomsUseCase _getAllRoomsUseCase;
    private readonly GetAvailableRoomsUseCase _getAvailableRoomsUseCase;
    private readonly UploadRoomImageUseCase _uploadRoomImageUseCase;
    private readonly DeleteRoomImageUseCase _deleteRoomImageUseCase;

    public RoomsController(
        GetAllRoomsUseCase getAllRoomsUseCase,
        GetAvailableRoomsUseCase getAvailableRoomsUseCase,
        UploadRoomImageUseCase uploadRoomImageUseCase,
        DeleteRoomImageUseCase deleteRoomImageUseCase)
    {
        _getAllRoomsUseCase = getAllRoomsUseCase;
        _getAvailableRoomsUseCase = getAvailableRoomsUseCase;
        _uploadRoomImageUseCase = uploadRoomImageUseCase;
        _deleteRoomImageUseCase = deleteRoomImageUseCase;
    }

    /// <summary>
    /// Uploads an image for the specified room.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="file"></param>
    /// <returns></returns>
    [HttpPost("{id:guid}/images")]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<RoomImageDto>> UploadRoomImage(Guid id, IFormFile? file)
    {
        if (file is null || file.Length == 0)
        {
            return ToActionResult(Result<RoomImageDto>.Failure(
                ErrorCode.Validation,
                "Image file is required."));
        }

        if (file.Length > MaxImageSizeInBytes)
        {
            return ToActionResult(Result<RoomImageDto>.Failure(
                ErrorCode.Validation,
                "Image file size must be 5 MB or less."));
        }

        var extension = Path.GetExtension(file.FileName);
        if (!AllowedImageExtensions.Contains(extension))
        {
            return ToActionResult(Result<RoomImageDto>.Failure(
                ErrorCode.Validation,
                "Only jpg, jpeg, png, and webp images are allowed."));
        }

        await using var stream = file.OpenReadStream();
        var result = await _uploadRoomImageUseCase.ExecuteAsync(id, stream, file.FileName, file.ContentType);

        if (result.IsSuccess)
            return Created($"/api/rooms/{id}/images/{result.Value!.Id}", result.Value);

        return ToActionResult(result);
    }

    /// <summary>
    /// Deletes an image for the specified room.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="imageId"></param>
    /// <returns></returns>
    [HttpDelete("{id:guid}/images/{imageId:guid}")]
    public async Task<ActionResult> DeleteRoomImage(Guid id, Guid imageId)
    {
        var result = await _deleteRoomImageUseCase.ExecuteAsync(id, imageId);
        return ToActionResult(result);
    }

    [HttpGet("available")]
    public async Task<ActionResult<IEnumerable<RoomResponseDto>>> GetAvailableRooms([FromQuery] DateOnly? checkInDate, [FromQuery] DateOnly? checkOutDate)
    {
        if(!checkInDate.HasValue || !checkOutDate.HasValue)
        {   
            return ToActionResult(Result<IEnumerable<RoomResponseDto>>.Failure(
                ErrorCode.Validation, 
                "Both checkInDate and checkOutDate query parameters are required."));
        }

        var result = await _getAvailableRoomsUseCase.ExecuteAsync(checkInDate.Value, checkOutDate.Value);
        return ToActionResult(result);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RoomResponseDto>>> GetAllRooms()
    {
        var result = await _getAllRoomsUseCase.ExecuteAsync();
        return ToActionResult(result);
    }


}
