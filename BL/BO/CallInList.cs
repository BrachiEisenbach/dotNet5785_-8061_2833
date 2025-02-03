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
    }
}
