using HotelBookingSys.Domain.Entities;
namespace HotelBookingSys.Application.Interfaces;

public interface ICustomerRepository
{
    Task<Customer> GetByIdAsync(Guid id);

    Task<Customer> GetByNameAsync(string firstName, string lastName);
   
    Task<IEnumerable<Customer>> GetAllAsync();
    Task AddAsync(Customer customer);
    Task UpdateAsync(Customer customer);
    Task DeleteAsync(Guid id);

}
