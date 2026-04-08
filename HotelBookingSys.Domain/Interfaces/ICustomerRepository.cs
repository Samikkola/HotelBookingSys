using HotelBookingSys.Domain.Entities;
namespace HotelBookingSys.Domain.Interfaces;

public interface ICustomerRepository
{
    /// <summary>
    /// Retrieves a customer by identifier.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Customer?> GetByIdAsync(Guid id);

    /// <summary>
    /// Retrieves a customer by full name.
    /// </summary>
    /// <param name="firstName"></param>
    /// <param name="lastName"></param>
    /// <returns></returns>
    Task<Customer?> GetByNameAsync(string firstName, string lastName);

    /// <summary>
    /// Retrieves a customer by email address.
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    Task<Customer?> GetCustomerByEmailAsync(string email);

    /// <summary>
    /// Retrieves a customer by phone number.
    /// </summary>
    /// <param name="phone"></param>
    /// <returns></returns>
    Task<Customer?> GetCustomerByPhoneAsync(string phone);

    /// <summary>
    /// Checks whether an email is already used by another customer.
    /// </summary>
    /// <param name="email"></param>
    /// <param name="excludeCustomerId"></param>
    /// <returns></returns>
    Task<bool> EmailExistsAsync(string email, Guid? excludeCustomerId = null);

    /// <summary>
    /// Checks whether a phone number is already used by another customer.
    /// </summary>
    /// <param name="phone"></param>
    /// <param name="excludeCustomerId"></param>
    /// <returns></returns>
    Task<bool> PhoneExistsAsync(string phone, Guid? excludeCustomerId = null);
   
    /// <summary>
    /// Retrieves all customers.
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<Customer>> GetAllAsync();

    /// <summary>
    /// Adds a customer.
    /// </summary>
    /// <param name="customer"></param>
    /// <returns></returns>
    Task AddAsync(Customer customer);

    /// <summary>
    /// Updates a customer.
    /// </summary>
    /// <param name="customer"></param>
    /// <returns></returns>
    Task UpdateAsync(Customer customer);

    /// <summary>
    /// Deletes a customer.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task DeleteAsync(Guid id);

}
