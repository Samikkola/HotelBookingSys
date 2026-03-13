using HotelBookingSys.Application.DTOs;
using HotelBookingSys.Application.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBookingSys.Application.UseCases;

public class GetCustomersUseCase
{
    private readonly ICustomerRepository _customerRepository;

    public GetCustomersUseCase(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<IEnumerable<CustomerResponseDto>> ExecuteAsync()
    {
        var customers = await _customerRepository.GetAllAsync();

        return customers.Select(c => new CustomerResponseDto
        {
            Id = c.Id,
            FirstName = c.FirstName,
            LastName = c.LastName,
            Email = c.Email,
            Phone = c.PhoneNumber,
            Notes = c.Notes?.ToString() ?? string.Empty
        });
    }
}
