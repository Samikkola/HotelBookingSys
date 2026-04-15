using HotelBookingSys.Application.Common.Result;
using HotelBookingSys.Application.DTOs.CustomerDtos;
using HotelBookingSys.Domain.Interfaces;
using HotelBookingSys.Domain.Entities;

namespace HotelBookingSys.Application.UseCases.Customers;

public class UpdateCustomerUseCase
{
    private readonly ICustomerRepository _customerRepository;

    public UpdateCustomerUseCase(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    /// <summary>
    /// Updates customer details.
    /// </summary>
    /// <param name="customerId"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    public async Task<Result<CustomerResponseDto>> ExecuteAsync(Guid customerId, UpdateCustomerDto dto)
    {
        var customer = await _customerRepository.GetByIdAsync(customerId);
        if (customer == null)
            return Result<CustomerResponseDto>.Failure(ErrorCode.NotFound, $"Customer with ID {customerId} not found.");

        if (await _customerRepository.EmailExistsAsync(dto.Email, customerId))
            return Result<CustomerResponseDto>.Failure(ErrorCode.Conflict, "A customer with this email already exists.");

        if (await _customerRepository.PhoneExistsAsync(dto.PhoneNumber, customerId))
            return Result<CustomerResponseDto>.Failure(ErrorCode.Conflict, "A customer with this phone number already exists.");

        try
        {
            customer.UpdateDetails(dto.FirstName, dto.LastName, dto.Email, dto.PhoneNumber, dto.Notes);
        }
        catch (ArgumentException ex)
        {
            return Result<CustomerResponseDto>.Failure(ErrorCode.Validation, ex.Message);
        }

        await _customerRepository.UpdateAsync(customer);

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
