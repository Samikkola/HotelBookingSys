using System;
using System.Collections.Generic;
using System.Text;

namespace HotelBookingSys.Domain.Entities;

public class Customer
{
    public Guid Id { get; private set; }
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PhoneNumber { get; private set; } = string.Empty;    
    public string? Notes { get; private set; } = string.Empty;

    // Navigation property ?
    // public List<Reservation> Reservations { get; set; } = new List<Reservation>();

    //Constructor
    public Customer(string firstName, string lastName, string email, string phoneNumber, string? notes = null)
    {
        // Domain validation
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name is required.", nameof(firstName));

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name is required.", nameof(lastName));

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required.", nameof(email));
        //TODO: Check email format?

        if (string.IsNullOrWhiteSpace(phoneNumber))
            throw new ArgumentException("Phone number is required.", nameof(phoneNumber));

        Id = Guid.NewGuid();
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PhoneNumber = phoneNumber;
        Notes = notes;
    }

}
