
namespace DO;

public record Assignment
(
    int Id,
    int CallId,
    int VolunteerId,
    DateTime EntryTimeForTreatment ,
    DateTime? EndTimeOfTreatment=null,
    TYPEOFTREATMENT? TypeOfTreatment=null








)
{


    public override string ToString()
    {
        return $"Assignment Id: {Id}, " +
               $"Call Id: {CallId}, " +
               $"Volunteer Id: {VolunteerId}, " +
               $"Entry Time for Treatment: {EntryTimeForTreatment}, " +
               $"End Time of Treatment: {(EndTimeOfTreatment.HasValue ? EndTimeOfTreatment.Value.ToString() : "Not Set")}, " +
               $"Type of Treatment: {(TypeOfTreatment.HasValue ? TypeOfTreatment.Value.ToString() : "Not Specified")}";
    }


}
