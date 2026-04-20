using HotelBookingSys.Application.Common.Result;
using HotelBookingSys.Application.DTOs.CustomerDtos;
using HotelBookingSys.Application.Mappings.Customers;
using HotelBookingSys.Domain.Interfaces;

namespace HotelBookingSys.Application.UseCases.Customers;

public class GetCustomerByIdUseCase
{
    private readonly ICustomerRepository _customerRepository;

    public GetCustomerByIdUseCase(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }
    /// <summary>
    /// Retrieves a customer by their unique identifier.
    /// If the customer is found, returns a Result containing the CustomerResponseDto.
    /// </summary>
    /// <param name="customerId"></param>
    /// <returns></returns>
    public async Task<Result<CustomerResponseDto>> ExecuteAsync(Guid customerId)
    {
        var customer = await _customerRepository.GetByIdAsync(customerId);
        if (customer == null)
            return Result<CustomerResponseDto>.Failure(ErrorCode.NotFound, $"Customer with ID {customerId} not found.");

        return Result<CustomerResponseDto>.Success(CustomerMapper.ToResponseDto(customer));
    }
}
