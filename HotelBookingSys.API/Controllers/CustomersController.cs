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

    public CustomersController(CreateCustomerUseCase createCustomerUseCase)
    {
        _createCustomerUseCase = createCustomerUseCase;
    }

    [HttpPost]
    public async Task<ActionResult<CustomerResponseDto>> CreateCustomer([FromBody]CreateCustomerDto request)
    {
        var result = await _createCustomerUseCase.ExecuteAsync(request);
          
        return Ok(result);
    }
}
