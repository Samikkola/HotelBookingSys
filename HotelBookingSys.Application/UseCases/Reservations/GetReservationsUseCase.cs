using HotelBookingSys.Application.Common.Result;
using HotelBookingSys.Application.DTOs.ReservationDtos;
using HotelBookingSys.Application.Interfaces;
using HotelBookingSys.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBookingSys.Application.UseCases.Reservations;

public class GetReservationsUseCase
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IRoomRepository _roomRepository;

    public GetReservationsUseCase(IReservationRepository reservationRepository, IRoomRepository roomRepository)
    {
        _reservationRepository = reservationRepository;
        _roomRepository = roomRepository;
    }

    /// <summary>
    /// Retrieves all reservations from the repository.
    /// Maps the domain entities to ReservationResponseDto, including room numbers, and returns a Result containing the list of reservations.
    /// </summary>
    /// <returns></returns>
    public async Task<Result<IEnumerable<ReservationResponseDto>>> ExecuteAsync()
    {
        // Fetch reservations and rooms in parallel to optimize performance
        var reservationsTask = _reservationRepository.GetAllAsync();
        var roomsTask = _roomRepository.GetAllAsync();
        // Wait for both tasks to complete
        await Task.WhenAll(reservationsTask, roomsTask);

        // Create a dictionary to quickly lookup room numbers by room ID
        var reservations = reservationsTask.Result;
        var rooms = roomsTask.Result.ToDictionary(r => r.Id, r => r.RoomNumber);

        // Map reservations to DTOs, including room numbers
        return Result<IEnumerable<ReservationResponseDto>>.Success(reservations.Select(r => MapToDto(r, rooms)));

    }

    private ReservationResponseDto MapToDto(
        Reservation reservation,
        IReadOnlyDictionary<Guid, int> rooms)
    {
        return new ReservationResponseDto
        {
            Id = reservation.Id,
            CustomerId = reservation.CustomerId,
            RoomId = reservation.RoomId,
            RoomNumber = rooms.TryGetValue(reservation.RoomId, out var roomNum) ? roomNum : 0,
            CheckInDate = reservation.CheckInDate,
            CheckOutDate = reservation.CheckOutDate,
            NumberOfGuests = reservation.NumberOfGuests,
            TotalPrice = reservation.TotalPrice,
            Status = reservation.Status.ToString()
        };
    }

}
