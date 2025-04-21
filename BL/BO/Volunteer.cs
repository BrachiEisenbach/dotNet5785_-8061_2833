using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Volunteer
    {
        public int Id { get; init; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string? Password { get; set; }
        public string? FullAddress { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public ROLE Role { get; set; }
        public bool Active { get; set; }
        public double? MaxDistance { get; set; }
        public TYPEOFDISTANCE TypeOfDistance { get; set; }
        public int AllCallsThatTreated { get; init; }
        public int AllCallsThatCanceled { get; init; }
        public int AllCallsThatHaveExpired { get; init; }
        public BO.CallInProgress? CallInTreate { get; init; }


        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Volunteer ID: {Id}");
            sb.AppendLine($"Name: {FullName}");
            sb.AppendLine($"Phone: {Phone}");
            sb.AppendLine($"Email: {Email}");
            sb.AppendLine($"Address: {FullAddress}");
            sb.AppendLine($"Location: Latitude = {Latitude?.ToString() ?? "N/A"}, Longitude = {Longitude?.ToString() ?? "N/A"}");
            sb.AppendLine($"Role: {Role}");
            sb.AppendLine($"Active: {Active}");
            sb.AppendLine($"Max Distance: {MaxDistance?.ToString() ?? "N/A"} ({TypeOfDistance})");
            sb.AppendLine($"Calls Treated: {AllCallsThatTreated}");
            sb.AppendLine($"Calls Canceled: {AllCallsThatCanceled}");
            sb.AppendLine($"Calls Expired: {AllCallsThatHaveExpired}");
            sb.AppendLine($"Current Call In Treatment: {(CallInTreate != null ? CallInTreate.ToString() : "None")}");
            return sb.ToString();
        }
    }
}
