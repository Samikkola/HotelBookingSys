using HotelBookingSys.Application.Common.Result;
using HotelBookingSys.Application.DTOs;
using HotelBookingSys.Application.Interfaces;
using HotelBookingSys.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace HotelBookingSys.Application.UseCases;

public class CreateCustomerUseCase
{
    private readonly ICustomerRepository _customerRepository;

    public CreateCustomerUseCase(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<Result<CustomerResponseDto>> ExecuteAsync(CreateCustomerDto dto)
    {
        Customer customer;
        try
        {
            customer = MapToDomain(dto);
        }
        catch (ArgumentException ex)
        {
            return Result<CustomerResponseDto>.Failure(ErrorCode.Validation, ex.Message);
        }

        await _customerRepository.AddAsync(customer);

        return Result<CustomerResponseDto>.Success(MapToDto(customer));
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

    private Customer MapToDomain(CreateCustomerDto dto)
    {
        return new Customer(dto.FirstName, dto.LastName, dto.Email, dto.PhoneNumber, dto.Notes);
    }
}
