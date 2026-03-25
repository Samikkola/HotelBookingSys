using HotelBookingSys.Application.Common.Result;
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
    private readonly GetCustomerByIdUseCase _getCustomerByIdUseCase;
    

    public CustomersController(
        CreateCustomerUseCase createCustomerUseCase,
        GetCustomersUseCase getCustomersUseCase,
        GetCustomerByIdUseCase getCustomerByIdUseCase)
        
    {
        _createCustomerUseCase = createCustomerUseCase;
        _getCustomersUseCase = getCustomersUseCase;
        _getCustomerByIdUseCase = getCustomerByIdUseCase;      
        
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

    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerResponseDto>> GetCustomerById(Guid id)
    {
        var result = await _getCustomerByIdUseCase.ExecuteAsync(id);
        return ToActionResult(result);
    }

   
}
