using HotelBookingSys.Application.DTOs.ReservationDtos;
using HotelBookingSys.Domain.Entities;

namespace HotelBookingSys.Application.Mappings.Reservations;

public static class ReservationMapper
{
    /// <summary>
    /// Maps reservation creation DTO to reservation domain entity.
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="roomId"></param>
    /// <param name="roomCapacity"></param>
    /// <param name="basePrice"></param>
    /// <returns></returns>
    public static Reservation ToDomain(CreateReservationDto dto, Guid roomId, int roomCapacity, decimal basePrice)
    {
        return new Reservation(dto.CustomerId, roomId, dto.CheckInDate, dto.CheckOutDate, dto.NumberOfGuests, roomCapacity, basePrice);
    }

    /// <summary>
    /// Maps reservation domain entity to response DTO using room number.
    /// </summary>
    /// <param name="reservation"></param>
    /// <param name="roomNumber"></param>
    /// <returns></returns>
    public static ReservationResponseDto ToResponseDto(Reservation reservation, int roomNumber)
    {
        return new ReservationResponseDto
        {
            Id = reservation.Id,
            CustomerId = reservation.CustomerId,
            RoomId = reservation.RoomId,
            RoomNumber = roomNumber,
            CheckInDate = reservation.CheckInDate,
            CheckOutDate = reservation.CheckOutDate,
            NumberOfGuests = reservation.NumberOfGuests,
            TotalPrice = reservation.TotalPrice,
            Status = reservation.Status.ToString(),
            CreatedAt = reservation.CreatedAt,
            UpdatedAt = reservation.UpdatedAt,
        };
    }

    /// <summary>
    /// Maps reservation domain entity to response DTO using room number dictionary.
    /// </summary>
    /// <param name="reservation"></param>
    /// <param name="roomNumbers"></param>
    /// <returns></returns>
    public static ReservationResponseDto ToResponseDto(Reservation reservation, IReadOnlyDictionary<Guid, int> roomNumbers)
    {
        return ToResponseDto(
            reservation,
            roomNumbers.TryGetValue(reservation.RoomId, out var roomNumber) ? roomNumber : 0);
    }
}
