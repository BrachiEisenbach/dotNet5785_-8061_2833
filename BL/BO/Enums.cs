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
        NONE
    }
    public enum STATUS
    {
        InTreatment,
        InTreatmentDangerZone,
        Open,
        Closed,
        Expired,
        OpenDangerZone,
        none

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
        NONE = -1,
        CALLTYPE = 0,
        FULLNAME = 1,
        SUMTREATED = 2,
        SUMCANCELED = 3,
        SUMEXPIRED = 4
    }

    public enum CallFieldSort
    {
        CallType,
        Status,
        SumOfAssigned
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
