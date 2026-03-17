using System;

namespace HotelBookingSys.Application.DTOs;

public class UpdateReservationDatesDto
{
    public DateOnly NewCheckInDate { get; set; }
    public DateOnly NewCheckOutDate { get; set; }
}