using FluentAssertions;
using HotelBookingSys.Application.DTOs.ReservationDtos;
using HotelBookingSys.Domain.Interfaces;
using HotelBookingSys.Application.UseCases.Reservations;
using HotelBookingSys.Domain.Entities;
using HotelBookingSys.Domain.Enums;
using HotelBookingSys.Application.Common.Result;
using Moq;

namespace HotelBookingSys.Tests.Application.Reservations;

public class CreateReservationUseCaseTests
{
    private static readonly Guid CustomerId = Guid.NewGuid();
    private const int RoomNumber = 101;
    private static readonly DateOnly CheckIn = new(2026, 2, 10);
    private static readonly DateOnly CheckOut = new(2026, 2, 12);

    [Fact]
    public async Task ExecuteAsync_WhenCustomerNotFound_ReturnsNotFound()
    {
        var dto = new CreateReservationDto
        {
            CustomerId = CustomerId,
            RoomNumber = RoomNumber,
            CheckInDate = CheckIn,
            CheckOutDate = CheckOut,
            NumberOfGuests = 1
        };

        var customerRepo = new Mock<ICustomerRepository>();
        var roomRepo = new Mock<IRoomRepository>();
        var reservationRepo = new Mock<IReservationRepository>();

        customerRepo.Setup(r => r.GetByIdAsync(CustomerId)).ReturnsAsync((Customer?)null);

        var useCase = new CreateReservationUseCase(customerRepo.Object, roomRepo.Object, reservationRepo.Object);

        var result = await useCase.ExecuteAsync(dto);

        result.IsFailure.Should().BeTrue();
        result.ErrorCode.Should().Be(ErrorCode.NotFound);
    }

    [Fact]
    public async Task ExecuteAsync_WhenRoomNotFound_ReturnsNotFound()
    {
        var dto = new CreateReservationDto
        {
            CustomerId = CustomerId,
            RoomNumber = RoomNumber,
            CheckInDate = CheckIn,
            CheckOutDate = CheckOut,
            NumberOfGuests = 1
        };

        var customerRepo = new Mock<ICustomerRepository>();
        var roomRepo = new Mock<IRoomRepository>();
        var reservationRepo = new Mock<IReservationRepository>();

        customerRepo.Setup(r => r.GetByIdAsync(CustomerId)).ReturnsAsync(new Customer("Jane", "Doe", "jane@example.com", "123"));
        roomRepo.Setup(r => r.GetByRoomNumberAsync(RoomNumber)).ReturnsAsync((Room?)null);

        var useCase = new CreateReservationUseCase(customerRepo.Object, roomRepo.Object, reservationRepo.Object);

        var result = await useCase.ExecuteAsync(dto);
        result.Should().NotBeNull();
        result.IsFailure.Should().BeTrue();
        result.ErrorCode.Should().Be(ErrorCode.NotFound);
    }

    [Fact]
    public async Task ExecuteAsync_WhenGuestCountExceedsCapacity_ReturnsValidationError()
    {
        var dto = new CreateReservationDto
        {
            CustomerId = CustomerId,
            RoomNumber = RoomNumber,
            CheckInDate = CheckIn,
            CheckOutDate = CheckOut,
            NumberOfGuests = 3
        };

        var customerRepo = new Mock<ICustomerRepository>();
        var roomRepo = new Mock<IRoomRepository>();
        var reservationRepo = new Mock<IReservationRepository>();

        customerRepo.Setup(r => r.GetByIdAsync(CustomerId)).ReturnsAsync(new Customer("Jane", "Doe", "jane@example.com", "123"));
        roomRepo.Setup(r => r.GetByRoomNumberAsync(RoomNumber)).ReturnsAsync(new Room(RoomNumber, RoomType.Standard, 2, 100m));

        var useCase = new CreateReservationUseCase(customerRepo.Object, roomRepo.Object, reservationRepo.Object);

        var result = await useCase.ExecuteAsync(dto);
        result.Should().NotBeNull();
        result.IsFailure.Should().BeTrue();
        result.ErrorCode.Should().Be(ErrorCode.Validation);
    }

    [Fact]
    public async Task ExecuteAsync_WhenOverlappingReservationsExist_ReturnsConflict()
    {
        var dto = new CreateReservationDto
        {
            CustomerId = CustomerId,
            RoomNumber = RoomNumber,
            CheckInDate = CheckIn,
            CheckOutDate = CheckOut,
            NumberOfGuests = 1
        };

        var customerRepo = new Mock<ICustomerRepository>();
        var roomRepo = new Mock<IRoomRepository>();
        var reservationRepo = new Mock<IReservationRepository>();

        var customer = new Customer("Jane", "Doe", "jane@example.com", "123");
        var room = new Room(RoomNumber, RoomType.Standard, 2, 100m);
        var existingReservation = new Reservation(customer.Id, room.Id, CheckIn, CheckOut, 1, room.RoomCapacity, room.BasePrice);

        customerRepo.Setup(r => r.GetByIdAsync(CustomerId)).ReturnsAsync(customer);
        roomRepo.Setup(r => r.GetByRoomNumberAsync(RoomNumber)).ReturnsAsync(room);
        reservationRepo.Setup(r => r.GetOverlappingReservationsByRoomIdAsync(room.Id, dto.CheckInDate, dto.CheckOutDate))
            .ReturnsAsync(new List<Reservation> { existingReservation });

        var useCase = new CreateReservationUseCase(customerRepo.Object, roomRepo.Object, reservationRepo.Object);

        var result = await useCase.ExecuteAsync(dto);

        result.IsFailure.Should().BeTrue();
        result.ErrorCode.Should().Be(ErrorCode.Conflict);
    }

    [Fact]
    public async Task ExecuteAsync_WithValidRequest_ReturnsSuccess()
    {
        var dto = new CreateReservationDto
        {
            CustomerId = CustomerId,
            RoomNumber = RoomNumber,
            CheckInDate = CheckIn,
            CheckOutDate = CheckOut,
            NumberOfGuests = 1
        };

        var customerRepo = new Mock<ICustomerRepository>();
        var roomRepo = new Mock<IRoomRepository>();
        var reservationRepo = new Mock<IReservationRepository>();

        var customer = new Customer("Jane", "Doe", "jane@example.com", "123");
        var room = new Room(RoomNumber, RoomType.Standard, 2, 100m);

        customerRepo.Setup(r => r.GetByIdAsync(CustomerId)).ReturnsAsync(customer);
        roomRepo.Setup(r => r.GetByRoomNumberAsync(RoomNumber)).ReturnsAsync(room);
        reservationRepo.Setup(r => r.GetOverlappingReservationsByRoomIdAsync(room.Id, dto.CheckInDate, dto.CheckOutDate))
            .ReturnsAsync(new List<Reservation>());

        var useCase = new CreateReservationUseCase(customerRepo.Object, roomRepo.Object, reservationRepo.Object);

        var result = await useCase.ExecuteAsync(dto);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value?.RoomNumber.Should().Be(RoomNumber);

        reservationRepo.Verify(r => r.AddAsync(It.IsAny<Reservation>()), Times.Once);
    }
}
