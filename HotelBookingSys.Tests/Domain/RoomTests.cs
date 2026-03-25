using FluentAssertions;
using HotelBookingSys.Domain.Entities;
using HotelBookingSys.Domain.Enums;

namespace HotelBookingSys.Tests.Domain;

public class RoomTests
{
    private const int RoomNumber = 101;
    private const int RoomCapacity = 2;
    private const decimal BasePrice = 120m;
    private const RoomType Type = RoomType.Standard;

    private Room ValidRoom => new(RoomNumber, Type, RoomCapacity, BasePrice);

    [Fact]
    public void Constructor_WithValidData_ShouldCreateRoom()
    {
        var room = ValidRoom;

        room.Should().NotBeNull();
        room.RoomNumber.Should().Be(RoomNumber);
        room.Type.Should().Be(Type);
        room.RoomCapacity.Should().Be(RoomCapacity);
        room.BasePrice.Should().Be(BasePrice);
    }

    [Fact]
    public void Constructor_WithInvalidRoomNumber_ShouldThrowArgumentException()
    {
        Action act = () => new Room(0, Type, RoomCapacity, BasePrice);

        act.Should().Throw<ArgumentException>()
            .WithMessage("*Room number must be greater than zero*");
    }

    [Fact]
    public void Constructor_WithInvalidRoomCapacity_ShouldThrowArgumentException()
    {
        Action act = () => new Room(RoomNumber, Type, 0, BasePrice);

        act.Should().Throw<ArgumentException>()
            .WithMessage("*Max persons must be greater than zero*");
    }

    [Fact]
    public void Constructor_WithNegativeBasePrice_ShouldThrowArgumentException()
    {
        Action act = () => new Room(RoomNumber, Type, RoomCapacity, -1m);

        act.Should().Throw<ArgumentException>()
            .WithMessage("*Base price cannot be negative*");
    }
}
