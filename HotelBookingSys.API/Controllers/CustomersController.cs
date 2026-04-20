using HotelBookingSys.Application.Common.Result;
using HotelBookingSys.Application.DTOs.CustomerDtos;
using HotelBookingSys.Application.UseCases.Customers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingSys.API.Controllers;


[ApiController]
[Route("api/[controller]")]
public class CustomersController : BaseController
{
    private readonly CreateCustomerUseCase _createCustomerUseCase;
    private readonly GetCustomersUseCase _getCustomersUseCase;
    private readonly GetCustomerByIdUseCase _getCustomerByIdUseCase;
    private readonly GetCustomerByEmailUseCase _getCustomerByEmailUseCase;
    private readonly GetCustomerByPhoneUseCase _getCustomerByPhoneUseCase;
    private readonly UpdateCustomerUseCase _updateCustomerUseCase;
    private readonly DeleteCustomerUseCase _deleteCustomerUseCase;
    

    public CustomersController(
        CreateCustomerUseCase createCustomerUseCase,
        GetCustomersUseCase getCustomersUseCase,
        GetCustomerByIdUseCase getCustomerByIdUseCase,
        GetCustomerByEmailUseCase getCustomerByEmailUseCase,
        GetCustomerByPhoneUseCase getCustomerByPhoneUseCase,
        UpdateCustomerUseCase updateCustomerUseCase,
        DeleteCustomerUseCase deleteCustomerUseCase)
        
    {
        _createCustomerUseCase = createCustomerUseCase;
        _getCustomersUseCase = getCustomersUseCase;
        _getCustomerByIdUseCase = getCustomerByIdUseCase;
        _getCustomerByEmailUseCase = getCustomerByEmailUseCase;
        _getCustomerByPhoneUseCase = getCustomerByPhoneUseCase;
        _updateCustomerUseCase = updateCustomerUseCase;
        _deleteCustomerUseCase = deleteCustomerUseCase;
        
    }

    /// <summary>
    /// Returns all customers.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerResponseDto>>> GetCustomers()
    {
        var result = await _getCustomersUseCase.ExecuteAsync();
        return ToActionResult(result);
    }

    /// <summary>
    /// Creates a customer.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<CustomerResponseDto>> CreateCustomer([FromBody]CreateCustomerDto request)
    {
        var result = await _createCustomerUseCase.ExecuteAsync(request);

        if (result.IsSuccess)
            return CreatedAtAction(nameof(GetCustomerById), new { id = result.Value!.Id }, result.Value);

        return ToActionResult(result);
    }

    /// <summary>
    /// Returns a customer by identifier.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerResponseDto>> GetCustomerById(Guid id)
    {
        var result = await _getCustomerByIdUseCase.ExecuteAsync(id);
        return ToActionResult(result);
    }

    /// <summary>
    /// Returns a customer by email.
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    [HttpGet("by-email")]
    public async Task<ActionResult<CustomerResponseDto>> GetCustomerByEmail([FromQuery] string email)
    {
        var result = await _getCustomerByEmailUseCase.ExecuteAsync(email);
        return ToActionResult(result);
    }

    /// <summary>
    /// Returns a customer by phone number.
    /// </summary>
    /// <param name="phone"></param>
    /// <returns></returns>
    [HttpGet("by-phone")]
    public async Task<ActionResult<CustomerResponseDto>> GetCustomerByPhone([FromQuery] string phone)
    {
        var result = await _getCustomerByPhoneUseCase.ExecuteAsync(phone);
        return ToActionResult(result);
    }

    /// <summary>
    /// Updates a customer.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<CustomerResponseDto>> UpdateCustomer(Guid id, [FromBody] UpdateCustomerDto request)
    {
        var result = await _updateCustomerUseCase.ExecuteAsync(id, request);
        return ToActionResult(result);
    }

    /// <summary>
    /// Deletes a customer.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCustomer(Guid id)
    {
        var result = await _deleteCustomerUseCase.ExecuteAsync(id);
        return ToActionResult(result);
    }
}
