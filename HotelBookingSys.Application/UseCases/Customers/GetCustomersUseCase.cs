using HotelBookingSys.Application.Common.Result;
using HotelBookingSys.Application.DTOs.CustomerDtos;
using HotelBookingSys.Application.Mappings.Customers;
using HotelBookingSys.Domain.Interfaces;

namespace HotelBookingSys.Application.UseCases.Customers;

public class GetCustomersUseCase
{
    private readonly ICustomerRepository _customerRepository;

    public GetCustomersUseCase(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }
    /// <summary>
    /// Retrieves all customers from the repository. 
    /// Maps the domain entities to CustomerResponseDto and returns a Result containing the list of customers.
    /// </summary>
    public async Task<Result<IEnumerable<CustomerResponseDto>>> ExecuteAsync()
    {
        var customers = await _customerRepository.GetAllAsync();

        // Map to dto and return the list of customers
        return Result<IEnumerable<CustomerResponseDto>>.Success(customers.Select(CustomerMapper.ToResponseDto).ToList());

    }
}
