using HotelBookingSys.Application.Common.Result;
using HotelBookingSys.Application.DTOs.CustomerDtos;
using HotelBookingSys.Domain.Interfaces;
using HotelBookingSys.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        return Result<IEnumerable<CustomerResponseDto>>.Success(customers.Select(MapToDto).ToList());

    }

    private CustomerResponseDto MapToDto(Customer customer)
    {
        return new CustomerResponseDto
        {
            Id = customer.Id,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Email = customer.Email,
            Phone = customer.PhoneNumber,
            Notes = customer.Notes?.ToString() ?? string.Empty
        };
    }
}
