using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    /// Defining a role in the system
    /// </summary>

    public enum ROLE
    {
        ADMIN = 0,
        DISTRICTMANAGER,
        VOLUNTEER
    }
    /// <summary>
    /// A distance type in generating a call
    /// </summary>
    public enum TYPEOFDISTANCE
    {
        AERIALDISTANCE,
        WALKINGDISTANCE,
        DRIVINGDISTANCE
    }
    public enum TYPEOFCALL
    {
        FLATTIRE,
        CARBURGLARY,
        REDRIVE,
    }
    public enum STATUS
    {
        InTreatment,
        InTreatmentDangerZone,
        Open,
        Closed,
        Expired,
        OpenDangerZone

    }
    /// <summary>
    /// type of treatment
    /// </summary>
    public enum FINISHTYPE
    {
        TREATE,
        SELFCANCELLATION,
        CANCALINGANADMINISTRATOR,
        CANCELLATIONHASEXPIRED

    }


    public enum VOLUNTEERFIELDSORT
    {
        FULLNAME,
        SUMTREATED,
        SUMCANCELED,
        SUMEXPIRED
    }

    public enum ClosedCallInListField
    {
        Id,
        FullAddress,
        TypeOfCall,
        OpenTime,
        EntryTimeForTreatment,
        EndTimeOfTreatment,
        TypeOfTreatment
    }
   
public enum OpenCallInListField
    {
        Id,
        FullAddress,
        TypeOfCall,
        OpenTime,
        MaxTimeToFinish,
        Distance
    }
  

public enum TIMEUNIT
    {
        MINUTE,
        HOUR,
        DAY,
        MONTH,
        YEAR
    }

}
