using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Helpers
{
    internal static class Tools
    {
        public static void ValidateVolunteerFormat(BO.Volunteer volunteer)
        {
            // Validate phone number format (numbers only, length 9 or 10)
            if (string.IsNullOrWhiteSpace(volunteer.Phone) || !Regex.IsMatch(volunteer.Phone, @"^\d{9,10}$"))
                throw new ArgumentException("Invalid phone number format.");
            // Validate email format (simplified regex for email)
            if (!string.IsNullOrWhiteSpace(volunteer.Email) && !Regex.IsMatch(volunteer.Email, @"^[^\s@]+@[^\s@]+\.[^\s@]+$"))
                throw new ArgumentException("Invalid email address format.");
            // Validate ID format (simple check for numeric ID and length of 9 digits)
            if (string.IsNullOrWhiteSpace(volunteer.Id.ToString()) || !Regex.IsMatch(volunteer.Id.ToString(), @"^\d{9}$"))
                throw new ArgumentException("Invalid volunteer ID format (must be 9 digits).");
            // Validate full name length
            if (string.IsNullOrWhiteSpace(volunteer.FullName) || volunteer.FullName.Length < 3)
                throw new ArgumentException("Full name must be at least 3 characters long.");
            // Validate longitude and latitude format
            if ((volunteer.Latitude.HasValue && !double.IsNaN(volunteer.Latitude.Value) &&
                (volunteer.Longitude.HasValue && !double.IsNaN(volunteer.Longitude.Value))))
            {
                double lat = volunteer.Latitude.Value;
                double lon = volunteer.Longitude.Value;
            }
            else
            {
                throw new ArgumentException("Latitude and Longitude must be valid numeric values.");
            }
            // Check that password is not empty
            if (string.IsNullOrWhiteSpace(volunteer.Password))
                throw new ArgumentException("Password cannot be empty.");
        }

        public static void ValidateVolunteerLogic(BO.Volunteer volunteer)
        {
            // Check that max distance is not negative
            if (volunteer.MaxDistance <= 0)
                throw new ArgumentException("Max distance must be positive.");
            // Check that the volunteer is active if they have an active call
            if (volunteer?.Active == true && !volunteer.Active)
                throw new ArgumentException("A volunteer with an active call must be active.");
            // If there is location data, make sure it is not empty
            if (!(volunteer.Latitude.HasValue && !double.IsNaN(volunteer.Latitude.Value) &&
                !(volunteer.Longitude.HasValue && !double.IsNaN(volunteer.Longitude.Value))))
                throw new ArgumentException("Volunteer must have a valid location (latitude and longitude).");
        }
    }
}
