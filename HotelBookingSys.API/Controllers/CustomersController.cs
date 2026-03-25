using HotelBookingSys.Application.DTOs.CustomerDtos;
using HotelBookingSys.Application.UseCases.Customers;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingSys.API.Controllers;


/// <summary>
/// Creates a new customer.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CustomersController : BaseController
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
        return ToActionResult(result);
    }

    [HttpPost]
    public async Task<ActionResult<CustomerResponseDto>> CreateCustomer([FromBody]CreateCustomerDto request)
    {
        var result = await _createCustomerUseCase.ExecuteAsync(request);

        if (result.IsSuccess)
            return CreatedAtAction(nameof(GetCustomers), result.Value);

        return ToActionResult(result);
    }
}
