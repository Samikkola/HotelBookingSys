namespace HotelBookingSys.Application.DTOs.ReservationDtos;

public class UpdateReservationDto
{
    public Guid? RoomId { get; set; }
    public int? GuestCount { get; set; }
    public DateOnly? CheckInDate { get; set; }
    public DateOnly? CheckOutDate { get; set; }
}
