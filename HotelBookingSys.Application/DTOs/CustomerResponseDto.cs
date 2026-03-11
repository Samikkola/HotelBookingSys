using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace HotelBookingSys.Application.DTOs;

public class CustomerResponseDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty; // TODO: use default! ?
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}
