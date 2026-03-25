using FluentAssertions;
using HotelBookingSys.Domain.Entities;

namespace HotelBookingSys.Tests.Domain;

public class CustomerTests
{
    private const string FirstName = "Jane";
    private const string LastName = "Doe";
    private const string Email = "jane@example.com";
    private const string PhoneNumber = "123456";
    private const string Notes = "VIP";

    private Customer ValidCustomer => new(FirstName, LastName, Email, PhoneNumber, Notes);

    [Fact]
    public void Constructor_WithValidData_ShouldCreateCustomer()
    {
        var customer = ValidCustomer;

        customer.Should().NotBeNull();
        customer.FirstName.Should().Be(FirstName);
        customer.LastName.Should().Be(LastName);
        customer.Email.Should().Be(Email);
        customer.PhoneNumber.Should().Be(PhoneNumber);
        customer.Notes.Should().Be(Notes);     
    }

    [Fact]
    public void Constructor_WithEmptyFirstName_ShouldThrowArgumentException()
    {
        Action act = () => new Customer(string.Empty, LastName, Email, PhoneNumber);

        act.Should().Throw<ArgumentException>()
            .WithMessage("*First name is required*");
    }

    [Fact]
    public void Constructor_WithEmptyLastName_ShouldThrowArgumentException()
    {
        Action act = () => new Customer(FirstName, string.Empty, Email, PhoneNumber);

        act.Should().Throw<ArgumentException>()
            .WithMessage("*Last name is required*");
    }

    [Fact]
    public void Constructor_WithEmptyEmail_ShouldThrowArgumentException()
    {
        Action act = () => new Customer(FirstName, LastName, string.Empty, PhoneNumber);

        act.Should().Throw<ArgumentException>()
            .WithMessage("*Email is required*");
    }

    [Fact]
    public void Constructor_WithInvalidEmail_ShouldThrowArgumentException()
    {
        Action act = () => new Customer(FirstName, LastName, "invalid-email", PhoneNumber);

        act.Should().Throw<ArgumentException>()
            .WithMessage("*Email must be eligible*");
    }

    [Fact]
    public void Constructor_WithTooLongEmail_ShouldThrowArgumentException()
    {
        var longEmail = new string('a', 256) + "@example.com";

        Action act = () => new Customer(FirstName, LastName, longEmail, PhoneNumber);

        act.Should().Throw<ArgumentException>()
            .WithMessage("*Email must be under 255 characters*");
    }

    [Fact]
    public void Constructor_WithEmptyPhoneNumber_ShouldThrowArgumentException()
    {
        Action act = () => new Customer(FirstName, LastName, Email, string.Empty);

        act.Should().Throw<ArgumentException>()
            .WithMessage("*Phone number is required*");
    }
}
