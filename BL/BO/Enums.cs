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
    public enum TYPEOFDISTSANCE
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
        DangerZone

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
}
