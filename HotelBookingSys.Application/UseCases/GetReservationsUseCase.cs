using HotelBookingSys.Application.DTOs;
using HotelBookingSys.Application.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBookingSys.Application.UseCases;

public class GetReservationsUseCase
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IRoomRepository _roomRepository;

    public GetReservationsUseCase(IReservationRepository reservationRepository, IRoomRepository roomRepository)
    {
        _reservationRepository = reservationRepository;
        _roomRepository = roomRepository;
    }

    public async Task<IEnumerable<ReservationResponseDto>> ExecuteAsync()
    {
        var reservationsTask = _reservationRepository.GetAllAsync();
        var roomsTask = _roomRepository.GetAllAsync();

        await Task.WhenAll(reservationsTask, roomsTask);

        var reservations = reservationsTask.Result;
        var rooms = roomsTask.Result.ToDictionary(r => r.Id, r => r.RoomNumber);

        return reservations.Select(r => new ReservationResponseDto
        {
            Id = r.Id,
            CustomerId = r.CustomerId,
            RoomId = r.RoomId,
            RoomNumber = rooms.TryGetValue(r.RoomId, out var roomNum) ? roomNum : 0,
            CheckInDate = r.CheckInDate,
            CheckOutDate = r.CheckOutDate,
            TotalPrice = r.TotalPrice,
            Status = r.Status.ToString()
        });
    }
}
