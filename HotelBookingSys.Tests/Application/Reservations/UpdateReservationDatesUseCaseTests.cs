using FluentAssertions;
using HotelBookingSys.Application.DTOs.ReservationDtos;
using HotelBookingSys.Application.Interfaces;
using HotelBookingSys.Application.UseCases.Reservations;
using HotelBookingSys.Domain.Entities;
using HotelBookingSys.Domain.Enums;
using HotelBookingSys.Application.Common.Result;
using Moq;

namespace HotelBookingSys.Tests.Application.Reservations;

public class UpdateReservationDatesUseCaseTests
{
    private static readonly Guid ReservationId = Guid.NewGuid();
    private static readonly Guid RoomId = Guid.NewGuid();
    private static readonly DateOnly CheckIn = new(2026, 2, 10);
    private static readonly DateOnly CheckOut = new(2026, 2, 12);

    [Fact]
    public async Task ExecuteAsync_WhenReservationNotFound_ReturnsNotFound()
    {
        var reservationRepo = new Mock<IReservationRepository>();
        var roomRepo = new Mock<IRoomRepository>();

        reservationRepo.Setup(r => r.GetByIdAsync(ReservationId)).ReturnsAsync((Reservation?)null);

        var useCase = new UpdateReservationDatesUseCase(reservationRepo.Object, roomRepo.Object);

        var result = await useCase.ExecuteAsync(ReservationId, CheckIn, CheckOut);

        result.IsFailure.Should().BeTrue();
        result.ErrorCode.Should().Be(ErrorCode.NotFound);
    }

    [Fact]
    public async Task ExecuteAsync_WhenRoomNotFound_ReturnsNotFound()
    {
        var reservationRepo = new Mock<IReservationRepository>();
        var roomRepo = new Mock<IRoomRepository>();

        var reservation = new Reservation(Guid.NewGuid(), RoomId, CheckIn, CheckOut, 1, 2, 100m);
        reservationRepo.Setup(r => r.GetByIdAsync(ReservationId)).ReturnsAsync(reservation);
        roomRepo.Setup(r => r.GetByIdAsync(RoomId)).ReturnsAsync((Room?)null);

        var useCase = new UpdateReservationDatesUseCase(reservationRepo.Object, roomRepo.Object);

        var result = await useCase.ExecuteAsync(ReservationId, CheckIn, CheckOut);

        result.IsFailure.Should().BeTrue();
        result.ErrorCode.Should().Be(ErrorCode.NotFound);
    }

    [Fact]
    public async Task ExecuteAsync_WhenOverlappingReservationExists_ReturnsConflict()
    {
        var reservationRepo = new Mock<IReservationRepository>();
        var roomRepo = new Mock<IRoomRepository>();

        var reservation = new Reservation(Guid.NewGuid(), RoomId, CheckIn, CheckOut, 1, 2, 100m);
        var overlapping = new Reservation(Guid.NewGuid(), RoomId, CheckIn, CheckOut, 1, 2, 100m);
        var room = new Room(101, RoomType.Standard, 2, 100m);

        reservationRepo.Setup(r => r.GetByIdAsync(ReservationId)).ReturnsAsync(reservation);
        roomRepo.Setup(r => r.GetByIdAsync(RoomId)).ReturnsAsync(room);
        reservationRepo.Setup(r => r.GetOverlappingReservationsByRoomIdAsync(RoomId, CheckIn, CheckOut))
            .ReturnsAsync(new List<Reservation> { overlapping });

        var useCase = new UpdateReservationDatesUseCase(reservationRepo.Object, roomRepo.Object);

        var result = await useCase.ExecuteAsync(ReservationId, CheckIn, CheckOut);

        result.IsFailure.Should().BeTrue();
        result.ErrorCode.Should().Be(ErrorCode.Conflict);
    }

    [Fact]
    public async Task ExecuteAsync_WithValidRequest_ReturnsUpdatedReservation()
    {
        var reservationRepo = new Mock<IReservationRepository>();
        var roomRepo = new Mock<IRoomRepository>();

        var reservation = new Reservation(Guid.NewGuid(), RoomId, CheckIn, CheckOut, 1, 2, 100m);
        var room = new Room(101, RoomType.Standard, 2, 100m);

        var newCheckIn = CheckIn.AddDays(1);
        var newCheckOut = CheckOut.AddDays(1);

        reservationRepo.Setup(r => r.GetByIdAsync(ReservationId)).ReturnsAsync(reservation);
        roomRepo.Setup(r => r.GetByIdAsync(RoomId)).ReturnsAsync(room);
        reservationRepo.Setup(r => r.GetOverlappingReservationsByRoomIdAsync(RoomId, newCheckIn, newCheckOut))
            .ReturnsAsync(new List<Reservation>());

        var useCase = new UpdateReservationDatesUseCase(reservationRepo.Object, roomRepo.Object);

        var result = await useCase.ExecuteAsync(ReservationId, newCheckIn, newCheckOut);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value?.CheckInDate.Should().Be(newCheckIn);
        result.Value?.CheckOutDate.Should().Be(newCheckOut);

        reservationRepo.Verify(r => r.UpdateAsync(reservation), Times.Once);
    }
}
