using HotelBookingSys.Application.Common.Result;
using HotelBookingSys.Application.DTOs.CustomerDtos;
using HotelBookingSys.Application.Mappings.Customers;
using HotelBookingSys.Domain.Interfaces;
using HotelBookingSys.Domain.Entities;

namespace HotelBookingSys.Application.UseCases.Customers;

public class CreateCustomerUseCase
{
    private readonly ICustomerRepository _customerRepository;

    public CreateCustomerUseCase(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }
    /// <summary>
    /// Creates a new customer based on the provided DTO. 
    /// Validates the input and returns a Result containing either the created CustomerResponseDto or error information if the operation fails.
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public async Task<Result<CustomerResponseDto>> ExecuteAsync(CreateCustomerDto dto)
    {
        if (await _customerRepository.EmailExistsAsync(dto.Email))
            return Result<CustomerResponseDto>.Failure(ErrorCode.Conflict, "A customer with this email already exists.");

        if (await _customerRepository.PhoneExistsAsync(dto.PhoneNumber))
            return Result<CustomerResponseDto>.Failure(ErrorCode.Conflict, "A customer with this phone number already exists.");

        Customer customer;
        try
        {
            customer = CustomerMapper.ToDomain(dto);
        }
        catch (ArgumentException ex)
        {
            return Result<CustomerResponseDto>.Failure(ErrorCode.Validation, ex.Message);
        }

        await _customerRepository.AddAsync(customer);

        return Result<CustomerResponseDto>.Success(CustomerMapper.ToResponseDto(customer));
    }
}
