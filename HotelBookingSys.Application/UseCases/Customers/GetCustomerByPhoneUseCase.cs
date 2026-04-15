using HotelBookingSys.Application.Common.Result;
using HotelBookingSys.Application.DTOs.CustomerDtos;
using HotelBookingSys.Domain.Interfaces;
using HotelBookingSys.Domain.Entities;

namespace HotelBookingSys.Application.UseCases.Customers;

public class GetCustomerByPhoneUseCase
{
    private readonly ICustomerRepository _customerRepository;

    public GetCustomerByPhoneUseCase(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    /// <summary>
    /// Retrieves a customer by phone number.
    /// </summary>
    /// <param name="phone"></param>
    /// <returns></returns>
    public async Task<Result<CustomerResponseDto>> ExecuteAsync(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            return Result<CustomerResponseDto>.Failure(ErrorCode.Validation, "Phone is required.");

        var customer = await _customerRepository.GetCustomerByPhoneAsync(phone);
        if (customer == null)
            return Result<CustomerResponseDto>.Failure(ErrorCode.NotFound, $"Customer with phone '{phone}' not found.");

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
