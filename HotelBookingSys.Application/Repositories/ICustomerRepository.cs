
using HotelBookingSys.Domain.Entities;
namespace HotelBookingSys.Domain.Repositories;

public interface ICustomerRepository
{
    Task<Customer> GetByIdAsync(Guid id);

    Task<Customer> GetByNameAsync(string firstName, string lastName);
   
    Task AddAsync(Customer customer);
    Task UpdateAsync(Customer customer);
    Task DeleteAsync(Guid id);

}
