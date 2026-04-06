using FluentAssertions;
using HotelBookingSys.Domain.Interfaces;
using HotelBookingSys.Application.UseCases.Reservations;
using HotelBookingSys.Domain.Entities;
using HotelBookingSys.Domain.Enums;
using HotelBookingSys.Application.Common.Result;
using Moq;

namespace HotelBookingSys.Tests.Application.Reservations;

public class CancelReservationUseCaseTests
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

        var useCase = new CancelReservationUseCase(reservationRepo.Object, roomRepo.Object);

        var result = await useCase.ExecuteAsync(ReservationId);

        result.IsFailure.Should().BeTrue();
        result.ErrorCode.Should().Be(HotelBookingSys.Application.Common.Result.ErrorCode.NotFound);
    }

    [Fact]
    public async Task ExecuteAsync_WhenRoomNotFound_ReturnsNotFound()
    {
        var reservationRepo = new Mock<IReservationRepository>();
        var roomRepo = new Mock<IRoomRepository>();

        var reservation = new Reservation(Guid.NewGuid(), RoomId, CheckIn, CheckOut, 1, 2, 100m);
        reservationRepo.Setup(r => r.GetByIdAsync(ReservationId)).ReturnsAsync(reservation);
        roomRepo.Setup(r => r.GetByIdAsync(RoomId)).ReturnsAsync((Room?)null);

        var useCase = new CancelReservationUseCase(reservationRepo.Object, roomRepo.Object);

        var result = await useCase.ExecuteAsync(ReservationId);

        result.IsFailure.Should().BeTrue();
        result.ErrorCode.Should().Be(ErrorCode.NotFound);
    }

    [Fact]
    public async Task ExecuteAsync_WhenReservationIsNotActive_ReturnsConflict()
    {
        var reservationRepo = new Mock<IReservationRepository>();
        var roomRepo = new Mock<IRoomRepository>();

        var reservation = new Reservation(Guid.NewGuid(), RoomId, CheckIn, CheckOut, 1, 2, 100m);
        reservation.CancelReservation();
        var room = new Room(101, RoomType.Standard, 2, 100m);

        reservationRepo.Setup(r => r.GetByIdAsync(ReservationId)).ReturnsAsync(reservation);
        roomRepo.Setup(r => r.GetByIdAsync(RoomId)).ReturnsAsync(room);

        var useCase = new CancelReservationUseCase(reservationRepo.Object, roomRepo.Object);

        var result = await useCase.ExecuteAsync(ReservationId);

        result.IsFailure.Should().BeTrue();
        result.ErrorCode.Should().Be(ErrorCode.Conflict);
    }

    [Fact]
    public async Task ExecuteAsync_WithValidReservation_ReturnsSuccess()
    {
        var reservationRepo = new Mock<IReservationRepository>();
        var roomRepo = new Mock<IRoomRepository>();

        var reservation = new Reservation(Guid.NewGuid(), RoomId, CheckIn, CheckOut, 1, 2, 100m);
        var room = new Room(101, RoomType.Standard, 2, 100m);

        reservationRepo.Setup(r => r.GetByIdAsync(ReservationId)).ReturnsAsync(reservation);
        roomRepo.Setup(r => r.GetByIdAsync(RoomId)).ReturnsAsync(room);

        var useCase = new CancelReservationUseCase(reservationRepo.Object, roomRepo.Object);

        var result = await useCase.ExecuteAsync(ReservationId);

        result.IsSuccess.Should().BeTrue();
        result.Value?.Status.Should().Be(ReservationStatus.Cancelled.ToString());

        reservationRepo.Verify(r => r.UpdateAsync(reservation), Times.Once);
    }
}
