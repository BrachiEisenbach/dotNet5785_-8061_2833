using DO;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class CallInList
    {
        public int Id { get; init; }
        public int CallId { get; init; }
        public TYPEOFCALL TypeOfCall { get; init; }
        public DateTime OpenTime { get; init; }
        public TimeSpan? TimeLeft { get; init; }
        public string? NameOfLastVolunteer { get; init; }
        public TimeSpan? TimeTaken { get; init; }
        public STATUS Status { get; init; }
        public int SumOfAssigned { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"ID: {Id}");
            sb.AppendLine($"Call ID: {CallId}");
            sb.AppendLine($"Type Of Call: {TypeOfCall}");
            sb.AppendLine($"Open Time: {OpenTime}");
            sb.AppendLine($"Time Left: {(TimeLeft != null ? TimeLeft : "None")}");
            sb.AppendLine($"Name Of Last Volunteer: {(NameOfLastVolunteer != null ? NameOfLastVolunteer : "None")}");
            sb.AppendLine($"Time Taken: {(TimeTaken != null ? TimeTaken : "None")}");
            sb.AppendLine($"Status: {Status}");
            sb.AppendLine($"Sum Of Assigned: {SumOfAssigned}");
            return sb.ToString();
        }
    }
   
}
