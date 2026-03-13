using HotelBookingSys.Application.DTOs;
using HotelBookingSys.Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingSys.API.Controllers;


/// <summary>
/// Creates a new customer.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly CreateCustomerUseCase _createCustomerUseCase;
    private readonly GetCustomersUseCase _getCustomersUseCase;

    public CustomersController(CreateCustomerUseCase createCustomerUseCase, GetCustomersUseCase getCustomersUseCase)
    {
        _createCustomerUseCase = createCustomerUseCase;
        _getCustomersUseCase = getCustomersUseCase;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerResponseDto>>> GetCustomers()
    {
        var result = await _getCustomersUseCase.ExecuteAsync();
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<CustomerResponseDto>> CreateCustomer([FromBody]CreateCustomerDto request)
    {
        var result = await _createCustomerUseCase.ExecuteAsync(request);
          
        return Ok(result);
    }
}
