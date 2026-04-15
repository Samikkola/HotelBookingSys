using HotelBookingSys.Application.Common.Result;
using HotelBookingSys.Application.DTOs.CustomerDtos;
using HotelBookingSys.Domain.Interfaces;
using HotelBookingSys.Domain.Entities;

namespace HotelBookingSys.Application.UseCases.Customers;

public class GetCustomerByEmailUseCase
{
    private readonly ICustomerRepository _customerRepository;

    public GetCustomerByEmailUseCase(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    /// <summary>
    /// Retrieves a customer by email address.
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    public async Task<Result<CustomerResponseDto>> ExecuteAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return Result<CustomerResponseDto>.Failure(ErrorCode.Validation, "Email is required.");

        var customer = await _customerRepository.GetCustomerByEmailAsync(email);
        if (customer == null)
            return Result<CustomerResponseDto>.Failure(ErrorCode.NotFound, $"Customer with email '{email}' not found.");

        return Result<CustomerResponseDto>.Success(MapToDto(customer));
    }

    private static CustomerResponseDto MapToDto(Customer customer)
    {
        return new CustomerResponseDto
        {
            Id = customer.Id,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Email = customer.Email,
            Phone = customer.PhoneNumber,
            Notes = customer.Notes ?? string.Empty,
            CreatedAt = customer.CreatedAt,
            UpdatedAt = customer.UpdatedAt,
        };
    }
}
