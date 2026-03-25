namespace HotelBookingSys.Application.DTOs.ReservationDtos;

public class CreateReservationDto
{
    public Guid CustomerId { get; set; }
    public int RoomNumber { get; set; }
    public DateOnly CheckInDate { get; set; }
    public DateOnly CheckOutDate { get; set; }
    public int NumberOfGuests { get; set; }
}
