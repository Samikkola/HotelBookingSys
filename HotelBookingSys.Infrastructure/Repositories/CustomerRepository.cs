using HotelBookingSys.Domain.Interfaces;
using HotelBookingSys.Domain.Entities;
using HotelBookingSys.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSys.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CustomerRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Customer customer)
        {
            await _dbContext.Customers.AddAsync(customer);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var customer = await _dbContext.Customers.FindAsync(id);
            if (customer == null)
                return;

            _dbContext.Customers.Remove(customer);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await _dbContext.Customers.ToListAsync();
        }

        public async Task<Customer?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Customers.FindAsync(id);
        }

        public async Task<Customer?> GetByNameAsync(string firstName, string lastName)
        {
            return await _dbContext.Customers.FirstOrDefaultAsync(c => c.FirstName == firstName && c.LastName == lastName);
        }

        public async Task<Customer?> GetCustomerByEmailAsync(string email)
        {
            return await _dbContext.Customers.FirstOrDefaultAsync(c => c.Email == email);
        }

        public async Task<Customer?> GetCustomerByPhoneAsync(string phone)
        {
            return await _dbContext.Customers.FirstOrDefaultAsync(c => c.PhoneNumber == phone);
        }

        public async Task UpdateAsync(Customer customer)
        {
            _dbContext.Customers.Update(customer);
            await _dbContext.SaveChangesAsync();
        }
    }
}
