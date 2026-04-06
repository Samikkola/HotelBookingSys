using HotelBookingSys.Domain.Enums;

namespace HotelBookingSys.Application.DTOs.ReservationDtos;

public class ReservationFilterDto
{
    public Guid? CustomerId { get; set; }
    public Guid? RoomId { get; set; }
    public ReservationStatus? Status { get; set; }
    public DateOnly? FromDate { get; set; }
    public DateOnly? ToDate { get; set; }
}
