using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class VolunteerInList
    {
        public int Id { get; init; }
        public string FullName { get; init; }
        public bool Active { get; init; }
        public int AllCallsThatTreated { get; init; }
        public int AllCallsThatCanceled { get; init; }
        public int AllCallsThatHaveExpired { get; init; }
        public int? CallId { get; init; }
        public BO.TYPEOFCALL TypeOfCall { get; init; }


        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Volunteer ID: {Id}");
            sb.AppendLine($"Name: {FullName}");
            sb.AppendLine($"Active: {Active}");
            sb.AppendLine($"AllCallsThatTreated: {AllCallsThatTreated}");
            sb.AppendLine($"AllCallsThatCanceled: {AllCallsThatCanceled}");
            sb.AppendLine($"AllCallsThatHaveExpired: {AllCallsThatHaveExpired}");
            sb.AppendLine($"CallId: {(CallId != null ? CallId: "None")}");
            return sb.ToString();
        }
    }
}




