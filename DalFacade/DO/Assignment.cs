
namespace DO;

/// <summary>
/// Represents an assignment that links a volunteer to a call and tracks the treatment process.
/// </summary>
/// <param name="Id">The unique identifier for the assignment.</param>
/// <param name="CallId">The ID of the related call being handled in this assignment.</param>
/// <param name="VolunteerId">The ID of the volunteer assigned to handle the call.</param>
/// <param name="EntryTimeForTreatment">The date and time the treatment or handling of the call began.</param>
/// <param name="EndTimeOfTreatment">
/// The date and time the treatment or handling of the call was completed. 
/// Optional; <c>null</c> if the treatment is ongoing or not completed.
/// </param>
/// <param name="TypeOfTreatment">
/// The type of treatment provided for the call. 
/// Optional; <c>null</c> if the type has not been determined.
/// </param>
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


    public Assignment() : this(0, 0, 0, DateTime.MinValue, null, null) { }



    /// <summary>
    /// Returns a string representation of the assignment, including its details.
    /// </summary>
    /// <returns>
    /// A string containing the assignment's ID, related call ID, volunteer ID, 
    /// entry time, end time, and type of treatment.
    /// </returns>

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
