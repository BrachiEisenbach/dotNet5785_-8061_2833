using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class CallAssignInList
    {
        public int? VolunteerId { get; init; }
        public string? VolunteerName { get; init; }
        public DateTime EntryTimeForTreatment { get; init; }
        public DateTime? EndTimeOfTreatment { get; init; }
        public FINISHTYPE? TypeOfTreatment { get; init; }

        public override string ToString()
        {
            return $"Volunteer ID: {VolunteerId?.ToString() ?? "N/A"}, " +
                   $"Name: {VolunteerName ?? "N/A"}, " +
                   $"Entry: {EntryTimeForTreatment:g}, " +
                   $"End: {(EndTimeOfTreatment.HasValue ? EndTimeOfTreatment.Value.ToString("g") : "Still in treatment")}, " +
                   $"Finish Type: {TypeOfTreatment?.ToString() ?? "N/A"}";
        }


    }
}
