using HotelBookingSys.Application.Common.Result;
using HotelBookingSys.Application.DTOs.CustomerDtos;
using HotelBookingSys.Application.Mappings.Customers;
using HotelBookingSys.Domain.Interfaces;

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

        return Result<CustomerResponseDto>.Success(CustomerMapper.ToResponseDto(customer));
    }
}
