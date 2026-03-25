using System;

namespace HotelBookingSys.Application.DTOs.ReservationDtos;

public class UpdateReservationDatesDto
{
    public DateOnly NewCheckInDate { get; set; }
    public DateOnly NewCheckOutDate { get; set; }
}