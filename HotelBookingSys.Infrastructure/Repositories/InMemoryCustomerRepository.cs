using HotelBookingSys.Application.Interfaces;
using HotelBookingSys.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelBookingSys.Infrastructure.Repositories;

public class InMemoryCustomerRepository : ICustomerRepository
{
    private readonly InMemoryDatabase _database;

    public InMemoryCustomerRepository(InMemoryDatabase database)
    {
        _database = database;
    }

    public Task AddAsync(Customer customer)
    {
        _database.Customers.Add(customer);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<Customer?> GetByIdAsync(Guid id)
    {
        var customer =  _database.Customers.FirstOrDefault(c => c.Id == id);

        return Task.FromResult(customer);
    }

    public Task<Customer> GetByNameAsync(string firstName, string lastName)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Customer customer)
    {
        throw new NotImplementedException();
    }
}
