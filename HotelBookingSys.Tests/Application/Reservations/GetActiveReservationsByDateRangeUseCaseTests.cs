using FluentAssertions;
using HotelBookingSys.Application.Interfaces;
using HotelBookingSys.Application.UseCases.Reservations;
using HotelBookingSys.Domain.Entities;
using HotelBookingSys.Domain.Enums;
using HotelBookingSys.Application.Common.Result;
using Moq;

namespace HotelBookingSys.Tests.Application.Reservations;

public class GetActiveReservationsByDateRangeUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_WhenFromIsAfterTo_ReturnsValidationError()
    {
        var reservationRepo = new Mock<IReservationRepository>();
        var roomRepo = new Mock<IRoomRepository>();

        var useCase = new GetActiveReservationsByDateRangeUseCase(reservationRepo.Object, roomRepo.Object);

        var result = await useCase.ExecuteAsync(new DateOnly(2026, 2, 12), new DateOnly(2026, 2, 10));

        result.IsFailure.Should().BeTrue();
        result.ErrorCode.Should().Be(ErrorCode.Validation);
    }

    [Fact]
    public async Task ExecuteAsync_WithValidRange_ReturnsActiveReservations()
    {
        var reservationRepo = new Mock<IReservationRepository>();
        var roomRepo = new Mock<IRoomRepository>();

        var room = new Room(101, RoomType.Standard, 2, 100m);
        var reservation = new Reservation(Guid.NewGuid(), room.Id, new DateOnly(2026, 2, 10), new DateOnly(2026, 2, 12), 1, room.RoomCapacity, room.BasePrice);

        reservationRepo.Setup(r => r.GetActiveReservationsByDateRangeAsync(new DateOnly(2026, 2, 10), new DateOnly(2026, 2, 12)))
            .ReturnsAsync(new List<Reservation> { reservation });
        roomRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Room> { room });

        var useCase = new GetActiveReservationsByDateRangeUseCase(reservationRepo.Object, roomRepo.Object);

        var result = await useCase.ExecuteAsync(new DateOnly(2026, 2, 10), new DateOnly(2026, 2, 12));

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.Should().ContainSingle();
        result.Value!.First().RoomNumber.Should().Be(room.RoomNumber);
    }
}
