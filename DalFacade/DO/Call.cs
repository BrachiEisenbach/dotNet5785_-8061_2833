

namespace DO;

/// <summary>
/// Represents a call with various properties such as ID, type, location, description, and timing details.
/// </summary>
/// <param name="Id">The unique identifier for the call.</param>
/// <param name="TypeOfCall">The type of call, indicating the nature of the event (e.g., emergency).</param>
/// <param name="VerbalDescription">An optional verbal description of the call.</param>
/// <param name="FullAddress">The full address where the call originated. Default is an empty string.</param>
/// <param name="Latitude">The geographical latitude of the call's location. Default is 0.</param>
/// <param name="Longitude">The geographical longitude of the call's location. Default is 0.</param>
/// <param name="OpenTime">The date and time when the call was opened. Default is <c>DateTime.MinValue</c>.</param>
/// <param name="MaxTimeToFinish">
/// The maximum time allowed to finish handling the call. Optional; <c>null</c> if not set.
/// </param>
public record Call
       
(
    int Id,
    TYPEOFCALL TypeOfCall,
    string? VerbalDescription=null,
    string FullAddress="",
    double Latitude=0,
    double Longitude=0,
    DateTime OpenTime=default(DateTime) ,
    DateTime? MaxTimeToFinish=null

)
{
    public Call() : this(0, TYPEOFCALL.REDRIVE, null,"", 0,0, default(DateTime), null) { }


    /// <summary>
    /// Returns a string representation of the call, including its details.
    /// </summary>
    /// <returns>
    /// A string containing the call's ID, type, description, location, and timing information.
    /// </returns>
    public override string ToString()
    {
        return $"Id: {Id}, Type of Call: {TypeOfCall}, " +
               $"Verbal Description: {(string.IsNullOrEmpty(VerbalDescription) ? "Not Provided" : VerbalDescription)}, " +
               $"Address: {FullAddress}, Latitude: {Latitude}, Longitude: {Longitude}, " +
               $"Open Time: {OpenTime}, Max Time To Finish: {(MaxTimeToFinish.HasValue ? MaxTimeToFinish.Value.ToString() : "Not Set")}";
    }
}