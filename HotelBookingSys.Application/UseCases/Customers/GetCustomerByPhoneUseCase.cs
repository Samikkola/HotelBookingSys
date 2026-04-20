using HotelBookingSys.Application.Common.Result;
using HotelBookingSys.Application.DTOs.CustomerDtos;
using HotelBookingSys.Application.Mappings.Customers;
using HotelBookingSys.Domain.Interfaces;

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

        return Result<CustomerResponseDto>.Success(CustomerMapper.ToResponseDto(customer));
    }
}
