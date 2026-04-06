namespace HotelBookingSys.Application.DTOs.ReservationDtos;

public class UpdateReservationDto
{
    public Guid? RoomId { get; set; }
    public int? GuestCount { get; set; }
    public DateOnly? NewCheckInDate { get; set; }
    public DateOnly? NewCheckOutDate { get; set; }
}
