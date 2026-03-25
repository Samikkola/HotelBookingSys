using FluentAssertions;
using HotelBookingSys.Application.Interfaces;
using HotelBookingSys.Application.UseCases.Reservations;
using HotelBookingSys.Domain.Entities;
using HotelBookingSys.Domain.Enums;
using Moq;

namespace HotelBookingSys.Tests.Application.Reservations;

public class GetReservationsUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_WhenReservationsExist_ReturnsMappedResults()
    {
        var reservationRepo = new Mock<IReservationRepository>();
        var roomRepo = new Mock<IRoomRepository>();

        var room = new Room(101, RoomType.Standard, 2, 100m);
        var reservation = new Reservation(Guid.NewGuid(), room.Id, new DateOnly(2026, 2, 10), new DateOnly(2026, 2, 12), 1, room.RoomCapacity, room.BasePrice);

        reservationRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Reservation> { reservation });
        roomRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Room> { room });

        var useCase = new GetReservationsUseCase(reservationRepo.Object, roomRepo.Object);

        var result = await useCase.ExecuteAsync();

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.Should().ContainSingle();
        result.Value!.First().RoomNumber.Should().Be(room.RoomNumber);
    }
}
