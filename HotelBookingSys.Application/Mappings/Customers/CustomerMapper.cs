using HotelBookingSys.Application.DTOs.CustomerDtos;
using HotelBookingSys.Domain.Entities;

namespace HotelBookingSys.Application.Mappings.Customers;

public static class CustomerMapper
{
    /// <summary>
    /// Maps customer domain entity to response DTO.
    /// </summary>
    /// <param name="customer"></param>
    /// <returns></returns>
    public static CustomerResponseDto ToResponseDto(Customer customer)
    {
        return new CustomerResponseDto
        {
            Id = customer.Id,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Email = customer.Email,
            Phone = customer.PhoneNumber,
            Notes = customer.Notes ?? string.Empty,
            CreatedAt = customer.CreatedAt,
            UpdatedAt = customer.UpdatedAt,
        };
    }

    /// <summary>
    /// Maps customer creation DTO to customer domain entity.
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public static Customer ToDomain(CreateCustomerDto dto)
    {
        return new Customer(dto.FirstName, dto.LastName, dto.Email, dto.PhoneNumber, dto.Notes);
    }
}
