using System;
using System.Collections.Generic;
using System.Text;

namespace HotelBookingSys.Application.DTOs;

public class ReservationResponseDto
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public Guid RoomId { get; set; }
    public int RoomNumber { get; set; }
    public DateOnly CheckInDate { get; set; }
    public DateOnly CheckOutDate { get; set; }
    public int NumberOfGuests { get; set; }
    public decimal TotalPrice { get; set; }
    public string Status { get; set; } = string.Empty;
}
