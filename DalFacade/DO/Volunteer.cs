
namespace DO;

/// <summary>
/// Represents a user with various properties such as ID, full name, contact details, and additional settings.
/// </summary>
/// <param name="Id">The unique identifier for the user.</param>
/// <param name="FullName">The full name of the user.</param>
/// <param name="Phone">The phone number of the user.</param>
/// <param name="Email">The email address of the user.</param>
/// <param name="Password">The user's password.</param>
/// <param name="FullAddress">The full address of the user .</param>
/// <param name="Latitude">The geographical latitude of the user's location .</param>
/// <param name="Longitude">The geographical longitude of the user's location .</param>
/// <param name="Role">The role of the user, default is set to "VOLUNTEER".</param>
/// <param name="Active">Indicates whether the user is active (true/false).</param>
/// <param name="MaxDistance">The maximum distance , typically used to
/// define a search radius or allowed range.</param>
/// <param name="TypeOfDistance">Distance type, set to walking distance.</param>

public record Volunteer
(

   


    int Id,
    string FullName,
    string Phone,
    string Email,
    string? Password=null,
    string? FullAddress = null,
    double? Latitude=null,
    double? Longitude = null,
    ROLE Role=ROLE.VOLUNTEER,
    bool Active = false,
    double? MaxDistance = null,
    TYPEOFDISTSANCE TypeOfDistance=TYPEOFDISTSANCE.AERIALDISTANCE


)
{
    public override string ToString()
    {
        return $"Id: {Id}, Full Name: {FullName}, Phone: {Phone}, Email: {Email}, " +
               $"Password: {Password ?? "Not Set"}, Address: {FullAddress ?? "Not Set"}, " +
               $"Latitude: {Latitude?.ToString() ?? "Not Set"}, Longitude: {Longitude?.ToString() ?? "Not Set"}, " +
               $"Role: {Role}, Active: {Active}, Max Distance: {MaxDistance?.ToString() ?? "Not Set"}, " +
               $"Type of Distance: {TypeOfDistance}";
    }
}
