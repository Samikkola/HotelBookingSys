using FluentAssertions;
using HotelBookingSys.Application.Common.Result;
using HotelBookingSys.Application.DTOs.CustomerDtos;
using HotelBookingSys.Application.UseCases.Customers;
using HotelBookingSys.Domain.Entities;
using HotelBookingSys.Domain.Interfaces;
using Moq;

namespace HotelBookingSys.Tests.Application.Customers;

public class UpdateCustomerUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_WhenEmailAlreadyExistsForDifferentCustomer_ReturnsConflict()
    {
        var repository = new Mock<ICustomerRepository>();
        var customerId = Guid.NewGuid();
        var existingCustomer = new Customer("Jane", "Doe", "jane@example.com", "123456");

        repository.Setup(r => r.GetByIdAsync(customerId)).ReturnsAsync(existingCustomer);
        repository.Setup(r => r.EmailExistsAsync("new@example.com", customerId)).ReturnsAsync(true);

        var useCase = new UpdateCustomerUseCase(repository.Object);

        var result = await useCase.ExecuteAsync(customerId, new UpdateCustomerDto
        {
            FirstName = "Jane",
            LastName = "Doe",
            Email = "new@example.com",
            PhoneNumber = "999999",
            Notes = "updated"
        });

        result.IsFailure.Should().BeTrue();
        result.ErrorCode.Should().Be(ErrorCode.Conflict);
        result.ErrorMessage.Should().Be("A customer with this email already exists.");
        repository.Verify(r => r.UpdateAsync(It.IsAny<Customer>()), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_WhenPhoneAlreadyExistsForDifferentCustomer_ReturnsConflict()
    {
        var repository = new Mock<ICustomerRepository>();
        var customerId = Guid.NewGuid();
        var existingCustomer = new Customer("Jane", "Doe", "jane@example.com", "123456");

        repository.Setup(r => r.GetByIdAsync(customerId)).ReturnsAsync(existingCustomer);
        repository.Setup(r => r.EmailExistsAsync("new@example.com", customerId)).ReturnsAsync(false);
        repository.Setup(r => r.PhoneExistsAsync("999999", customerId)).ReturnsAsync(true);

        var useCase = new UpdateCustomerUseCase(repository.Object);

        var result = await useCase.ExecuteAsync(customerId, new UpdateCustomerDto
        {
            FirstName = "Jane",
            LastName = "Doe",
            Email = "new@example.com",
            PhoneNumber = "999999",
            Notes = "updated"
        });

        result.IsFailure.Should().BeTrue();
        result.ErrorCode.Should().Be(ErrorCode.Conflict);
        result.ErrorMessage.Should().Be("A customer with this phone number already exists.");
        repository.Verify(r => r.UpdateAsync(It.IsAny<Customer>()), Times.Never);
    }
}
