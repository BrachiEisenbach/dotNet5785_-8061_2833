

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

);