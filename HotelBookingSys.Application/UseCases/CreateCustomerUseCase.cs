using HotelBookingSys.Application.Interfaces;
using HotelBookingSys.Domain.Entities;

namespace HotelBookingSys.Application.UseCases;

public class CreateCustomerUseCase
{
    private readonly ICustomerRepository _customerRepository;

    public CreateCustomerUseCase(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<Customer> ExecuteAsync(string firstName, string lastName, string email, string phoneNumber, string? notes)
    {
        var customer = new Customer(firstName, lastName, email, phoneNumber, notes);

        await _customerRepository.AddAsync(customer);

        return customer;
    }
}
