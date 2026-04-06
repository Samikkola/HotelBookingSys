using HotelBookingSys.Application.Common.Result;
using HotelBookingSys.Domain.Interfaces;

namespace HotelBookingSys.Application.UseCases.Customers;

public class DeleteCustomerUseCase
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IReservationRepository _reservationRepository;

    public DeleteCustomerUseCase(ICustomerRepository customerRepository, IReservationRepository reservationRepository)
    {
        _customerRepository = customerRepository;
        _reservationRepository = reservationRepository;
    }

    /// <summary>
    /// Deletes a customer when no active reservations exist.
    /// </summary>
    /// <param name="customerId"></param>
    /// <returns></returns>
    public async Task<Result> ExecuteAsync(Guid customerId)
    {
        var customer = await _customerRepository.GetByIdAsync(customerId);
        if (customer == null)
            return Result.Failure(ErrorCode.NotFound, $"Customer with ID {customerId} not found.");

        var hasActiveReservations = await _reservationRepository.HasActiveReservationsByCustomerIdAsync(customerId);
        if (hasActiveReservations)
        {
            return Result.Failure(
                ErrorCode.Conflict,
                "Customer cannot be deleted because they have active reservations. Cancel or complete those reservations first.");
        }

        await _customerRepository.DeleteAsync(customerId);
        return Result.Success();
    }
}
