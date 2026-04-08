using FluentAssertions;
using HotelBookingSys.Application.Common.Result;
using HotelBookingSys.Application.DTOs.CustomerDtos;
using HotelBookingSys.Application.UseCases.Customers;
using HotelBookingSys.Domain.Entities;
using HotelBookingSys.Domain.Interfaces;
using Moq;

namespace HotelBookingSys.Tests.Application.Customers;

public class CreateCustomerUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_WhenEmailAlreadyExists_ReturnsConflict()
    {
        var repository = new Mock<ICustomerRepository>();
        repository.Setup(r => r.EmailExistsAsync("jane@example.com", null)).ReturnsAsync(true);

        var useCase = new CreateCustomerUseCase(repository.Object);

        var result = await useCase.ExecuteAsync(new CreateCustomerDto
        {
            FirstName = "Jane",
            LastName = "Doe",
            Email = "jane@example.com",
            PhoneNumber = "123456",
            Notes = ""
        });

        result.IsFailure.Should().BeTrue();
        result.ErrorCode.Should().Be(ErrorCode.Conflict);
        result.ErrorMessage.Should().Be("A customer with this email already exists.");
        repository.Verify(r => r.AddAsync(It.IsAny<Customer>()), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_WhenPhoneAlreadyExists_ReturnsConflict()
    {
        var repository = new Mock<ICustomerRepository>();
        repository.Setup(r => r.EmailExistsAsync("jane@example.com", null)).ReturnsAsync(false);
        repository.Setup(r => r.PhoneExistsAsync("123456", null)).ReturnsAsync(true);

        var useCase = new CreateCustomerUseCase(repository.Object);

        var result = await useCase.ExecuteAsync(new CreateCustomerDto
        {
            FirstName = "Jane",
            LastName = "Doe",
            Email = "jane@example.com",
            PhoneNumber = "123456",
            Notes = ""
        });

        result.IsFailure.Should().BeTrue();
        result.ErrorCode.Should().Be(ErrorCode.Conflict);
        result.ErrorMessage.Should().Be("A customer with this phone number already exists.");
        repository.Verify(r => r.AddAsync(It.IsAny<Customer>()), Times.Never);
    }
}
