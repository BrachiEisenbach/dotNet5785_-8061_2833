

namespace DO;

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
    public override string ToString()
    {
        return $"Id: {Id}, Type of Call: {TypeOfCall}, " +
               $"Verbal Description: {(string.IsNullOrEmpty(VerbalDescription) ? "Not Provided" : VerbalDescription)}, " +
               $"Address: {FullAddress}, Latitude: {Latitude}, Longitude: {Longitude}, " +
               $"Open Time: {OpenTime}, Max Time To Finish: {(MaxTimeToFinish.HasValue ? MaxTimeToFinish.Value.ToString() : "Not Set")}";
    }
}