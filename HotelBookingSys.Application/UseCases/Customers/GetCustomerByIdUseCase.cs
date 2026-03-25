using HotelBookingSys.Application.Common.Result;
using HotelBookingSys.Application.DTOs.CustomerDtos;
using HotelBookingSys.Application.Interfaces;
using HotelBookingSys.Domain.Entities;
using System.Threading.Tasks;

namespace HotelBookingSys.Application.UseCases.Customers;

public class GetCustomerByIdUseCase
{
    private readonly ICustomerRepository _customerRepository;

    public GetCustomerByIdUseCase(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<Result<CustomerResponseDto>> ExecuteAsync(Guid customerId)
    {
        var customer = await _customerRepository.GetByIdAsync(customerId);
        if (customer == null)
            return Result<CustomerResponseDto>.Failure(ErrorCode.NotFound, $"Customer with ID {customerId} not found.");

        return Result<CustomerResponseDto>.Success(MapToDto(customer));
    }

    private static CustomerResponseDto MapToDto(Customer customer)
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
