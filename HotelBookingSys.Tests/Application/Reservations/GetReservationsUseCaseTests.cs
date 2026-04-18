using FluentAssertions;
using HotelBookingSys.Application.Common;
using HotelBookingSys.Application.Common.Result;
using HotelBookingSys.Domain.Interfaces;
using HotelBookingSys.Application.UseCases.Reservations;
using HotelBookingSys.Application.DTOs.ReservationDtos;
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

        reservationRepo
            .Setup(r => r.GetReservationsAsync(null, null, null, null, null, 1, 20))
            .ReturnsAsync((new List<Reservation> { reservation }, 1));
        roomRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Room> { room });

        var useCase = new GetReservationsUseCase(reservationRepo.Object, roomRepo.Object);

        var result = await useCase.ExecuteAsync();

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.Items.Should().ContainSingle();
        result.Value.Items.First().RoomNumber.Should().Be(room.RoomNumber);
        result.Value.TotalCount.Should().Be(1);
        result.Value.TotalPages.Should().Be(1);
    }

    [Fact]
    public async Task ExecuteAsync_WithFilter_PassesFilterToRepository()
    {
        var reservationRepo = new Mock<IReservationRepository>();
        var roomRepo = new Mock<IRoomRepository>();

        var room = new Room(101, RoomType.Standard, 2, 100m);
        var reservation = new Reservation(Guid.NewGuid(), room.Id, new DateOnly(2026, 2, 10), new DateOnly(2026, 2, 12), 1, room.RoomCapacity, room.BasePrice);

        reservationRepo
            .Setup(r => r.GetReservationsAsync(reservation.CustomerId, room.Id, ReservationStatus.Active, new DateOnly(2026, 2, 1), new DateOnly(2026, 2, 28), 1, 20))
            .ReturnsAsync((new List<Reservation> { reservation }, 1));
        roomRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Room> { room });

        var useCase = new GetReservationsUseCase(reservationRepo.Object, roomRepo.Object);

        var result = await useCase.ExecuteAsync(new ReservationFilterDto
        {
            CustomerId = reservation.CustomerId,
            RoomId = room.Id,
            Status = ReservationStatus.Active,
            FromDate = new DateOnly(2026, 2, 1),
            ToDate = new DateOnly(2026, 2, 28)
        });

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.Items.Should().ContainSingle();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task ExecuteAsync_WhenPageIsInvalid_ReturnsValidationFailure(int invalidPage)
    {
        var reservationRepo = new Mock<IReservationRepository>();
        var roomRepo = new Mock<IRoomRepository>();
        var useCase = new GetReservationsUseCase(reservationRepo.Object, roomRepo.Object);

        var result = await useCase.ExecuteAsync(page: invalidPage);

        result.IsFailure.Should().BeTrue();
        result.ErrorCode.Should().Be(ErrorCode.Validation);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(101)]
    public async Task ExecuteAsync_WhenPageSizeIsInvalid_ReturnsValidationFailure(int invalidPageSize)
    {
        var reservationRepo = new Mock<IReservationRepository>();
        var roomRepo = new Mock<IRoomRepository>();
        var useCase = new GetReservationsUseCase(reservationRepo.Object, roomRepo.Object);

        var result = await useCase.ExecuteAsync(pageSize: invalidPageSize);

        result.IsFailure.Should().BeTrue();
        result.ErrorCode.Should().Be(ErrorCode.Validation);
    }

    [Fact]
    public async Task ExecuteAsync_WithValidPagination_ReturnsPagedResultMetadata()
    {
        var reservationRepo = new Mock<IReservationRepository>();
        var roomRepo = new Mock<IRoomRepository>();

        var room = new Room(101, RoomType.Standard, 2, 100m);
        var reservation = new Reservation(Guid.NewGuid(), room.Id, new DateOnly(2026, 2, 10), new DateOnly(2026, 2, 12), 1, room.RoomCapacity, room.BasePrice);

        reservationRepo
            .Setup(r => r.GetReservationsAsync(null, null, null, null, null, 2, 1))
            .ReturnsAsync((new List<Reservation> { reservation }, 3));
        roomRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Room> { room });

        var useCase = new GetReservationsUseCase(reservationRepo.Object, roomRepo.Object);

        var result = await useCase.ExecuteAsync(page: 2, pageSize: 1);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.Page.Should().Be(2);
        result.Value.PageSize.Should().Be(1);
        result.Value.TotalCount.Should().Be(3);
        result.Value.TotalPages.Should().Be(3);
    }
}
