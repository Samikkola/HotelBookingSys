namespace HotelBookingSys.API.DTOs;

public class CreateReservationRequestDto
{
    public Guid CustomerId { get; set; }
    public int RoomNumber { get; set; }
    public DateOnly CheckInDate { get; set; }
    public DateOnly CheckOutDate { get; set; }
}
