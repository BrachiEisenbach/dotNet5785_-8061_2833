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
            // 📱 Phone validation – must be 9 or 10 digits
            if (string.IsNullOrWhiteSpace(volunteer.Phone) || !Regex.IsMatch(volunteer.Phone, @"^\d{9,10}$"))
                throw new ArgumentException("Invalid phone number format. Must be 9 or 10 digits.");

            // 📧 Email validation – basic format
            if (!string.IsNullOrWhiteSpace(volunteer.Email) &&
                !Regex.IsMatch(volunteer.Email, @"^[^\s@]+@[^\s@]+\.[^\s@]+$"))
                throw new ArgumentException("Invalid email address format.");

            // 🆔 ID validation – must be exactly 9 digits
            if (volunteer.Id < 100000000 || volunteer.Id > 999999999)
                throw new ArgumentException("Invalid volunteer ID format. Must be 9 digits.");

            // 🧍‍♀️ Full name validation – minimum 3 characters
            if (string.IsNullOrWhiteSpace(volunteer.FullName) || volunteer.FullName.Length < 3)
                throw new ArgumentException("Full name must be at least 3 characters long.");

            // 📍 Latitude & Longitude validation – must exist and be within range
            if (!volunteer.Latitude.HasValue || !volunteer.Longitude.HasValue)
                throw new ArgumentException("Latitude and Longitude must be provided.");
            if (volunteer.Latitude is < -90 or > 90)
                throw new ArgumentException("Latitude must be between -90 and 90.");
            if (volunteer.Longitude is < -180 or > 180)
                throw new ArgumentException("Longitude must be between -180 and 180.");

            // 🔒 Password validation – not empty
            if (string.IsNullOrWhiteSpace(volunteer.Password))
                throw new ArgumentException("Password cannot be empty.");
        }


        public static void ValidateVolunteerLogic(BO.Volunteer volunteer)
        {
            // Max distance must be a positive number
            if (!volunteer.MaxDistance.HasValue || volunteer.MaxDistance <= 0)
                throw new ArgumentException("Max distance must be a positive number.");

            // Volunteer must be active if they have a call in progress
            if (volunteer.CallInTreate != null && !volunteer.Active)
                throw new ArgumentException("A volunteer with an active call must be marked as active.");

            // Location must be present and within valid range
            if (!volunteer.Latitude.HasValue || !volunteer.Longitude.HasValue)
                throw new ArgumentException("Volunteer must have a location.");
            if (volunteer.Latitude is < -90 or > 90 || volunteer.Longitude is < -180 or > 180)
                throw new ArgumentException("Latitude must be between -90 and 90, and longitude between -180 and 180.");

            // ID must be logically valid (with checksum)
            if (!IsValidIsraeliID(volunteer.Id))
                throw new ArgumentException("Invalid Israeli ID (checksum failed).");
        }

        //check if the id is valid
        public static bool IsValidIsraeliID(int id)
        {
            string idStr = id.ToString().PadLeft(9, '0'); // לוודא 9 ספרות עם אפסים מובילים

            if (!Regex.IsMatch(idStr, @"^\d{9}$"))
                return false;

            int sum = 0;
            for (int i = 0; i < 9; i++)
            {
                int digit = (idStr[i] - '0') * ((i % 2) + 1);
                if (digit > 9)
                    digit -= 9;
                sum += digit;
            }

            return sum % 10 == 0;
        }


    }
}
