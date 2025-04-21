using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class CallInProgress
    {
        public int Id { get; init; }
        public int CallId { get; init; }
        public TYPEOFCALL TypeOfCall { get; init; }
        public string? VerbalDescription { get; init; }
        public string? FullAddress { get; init; }
        public DateTime OpenTime { get; init; }
        public DateTime? MaxTimeToFinish { get; init; }
        public DateTime EnterTime { get; init; }
        public double Distance { get; init; }
        public STATUS Status { get; init; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Call In Progress ID: {Id}");
            sb.AppendLine($"Original Call ID: {CallId}");
            sb.AppendLine($"Type of Call: {TypeOfCall}");
            sb.AppendLine($"Description: {VerbalDescription ?? "N/A"}");
            sb.AppendLine($"Address: {FullAddress ?? "N/A"}");
            sb.AppendLine($"Opened At: {OpenTime}");
            sb.AppendLine($"Entered At: {EnterTime}");
            sb.AppendLine($"Max Time to Finish: {(MaxTimeToFinish.HasValue ? MaxTimeToFinish.ToString() : "N/A")}");
            sb.AppendLine($"Distance: {Distance} km");
            sb.AppendLine($"Status: {Status}");
            return sb.ToString();
        }


    }
}
