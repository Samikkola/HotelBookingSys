namespace HotelBookingSys.Domain.Entities;

/// <summary>
/// Represents an authenticated staff user in the hotel booking system.
/// </summary>
public class User : BaseEntity
{
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string Role { get; private set; } = string.Empty;

    private User()
    {
    }

    /// <summary>
    /// Creates a new user with validated authentication fields.
    /// </summary>
    /// <param name="firstName"></param>
    /// <param name="lastName"></param>
    /// <param name="email"></param>
    /// <param name="passwordHash"></param>
    /// <param name="role"></param>
    public User(string firstName, string lastName, string email, string passwordHash, string role)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required.", nameof(email));

        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("Password hash is required.", nameof(passwordHash));

        if (string.IsNullOrWhiteSpace(role))
            throw new ArgumentException("Role is required.", nameof(role));

        FirstName = firstName?.Trim() ?? string.Empty;
        LastName = lastName?.Trim() ?? string.Empty;
        Email = email.Trim();
        PasswordHash = passwordHash;
        Role = role.Trim();
    }
}
