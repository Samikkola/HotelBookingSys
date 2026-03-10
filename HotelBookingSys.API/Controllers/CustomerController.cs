using HotelBookingSys.API.DTOs;
using HotelBookingSys.Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingSys.API.Controllers;

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
    public async Task<IActionResult> CreateCustomer(CreateCustomerRequestDto request)
    {
        var customer = await _createCustomerUseCase.ExecuteAsync(
            request.FirstName,
            request.LastName,
            request.Email,
            request.PhoneNumber,
            request.Notes
        );

        return Ok(customer);
    }
}
