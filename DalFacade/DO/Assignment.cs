
namespace DO;

public record Assignment
(
    int Id,
    int CallId,
    int VolunteerId,
    DateTime EntryTimeForTreatment ,
    DateTime? EndTimeOfTreatment=null,
    TYPEOFTREATMENT? TypeOfTreatment=null





);