using DO;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Call
    {
        public int Id { get; init; }
        public TYPEOFCALL TypeOfCall { get; init; }
        public string? VerbalDescription { get; init; }
        public string FullAddress { get; init; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public DateTime OpenTime { get; init; }
        public DateTime? MaxTimeToFinish { get; init; }
        public STATUS Status { get; set; }

        public List<BO.CallAssignInList>? listOfCallAssign { get; set; }
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"ID: {Id}");
            sb.AppendLine($"Type Of Call: {TypeOfCall}");
            sb.AppendLine($"Verbal Description {(VerbalDescription != null ? VerbalDescription : "None")}");
            sb.AppendLine($"Full Address: {FullAddress}");
            sb.AppendLine($"Location: Latitude = {Latitude?.ToString() ?? "N/A"}, Longitude = {Longitude?.ToString() ?? "N/A"}");
            sb.AppendLine($"Open Time: {OpenTime}");
            sb.AppendLine($"Max Time To Finish {(MaxTimeToFinish != null ? MaxTimeToFinish : "None")}");
            sb.AppendLine($"Status: {Status}");

            return sb.ToString();
        }
    }
}
