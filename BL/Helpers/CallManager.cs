using DalApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
    internal static class CallManager
    {
        internal static IDal s_dal = Factory.Get; //stage 4

        //המרת סוג ה ENUM מ BO לDO
        public static DO.TYPEOFCALL ConvertToDOType(BO.TYPEOFCALL boType)
        {
            return (DO.TYPEOFCALL)Enum.Parse(typeof(DO.TYPEOFCALL), boType.ToString());
        }

        //המרת סוג ה ENUM מ DO ל BO
        internal static BO.TYPEOFCALL ConvertToBOType(DO.TYPEOFCALL boType)
        {
            return (BO.TYPEOFCALL)Enum.Parse(typeof(DO.TYPEOFCALL), boType.ToString());
        }


        internal static BO.STATUS CalculateStatus(DO.Call call, TimeSpan riskRange)
        {

            DateTime currentTime = DateTime.Now;
            var assignments = s_dal.Assignment.ReadAll().ToList();

            var lastAssignment = assignments
                    .Where(a => a.CallId == call.Id)
                    .OrderByDescending(a => a.EntryTimeForTreatment)
                    .FirstOrDefault();

            if (lastAssignment == null || lastAssignment.EndTimeOfTreatment.HasValue)
            {
                if (call.MaxTimeToFinish < currentTime)
                    return BO.STATUS.Expired;

                if (call.MaxTimeToFinish - currentTime <= riskRange)
                    return BO.STATUS.OpenDangerZone;

                return BO.STATUS.Open;
            }

            if (lastAssignment.TypeOfTreatment.ToString() == BO.FINISHTYPE.TREATE.ToString())
                return BO.STATUS.Closed;

            if (call.MaxTimeToFinish < currentTime)
                return BO.STATUS.Expired;

            if (call.MaxTimeToFinish - currentTime <= riskRange)
                return BO.STATUS.InTreatmentDangerZone;

            return BO.STATUS.InTreatment;
        }

        internal static BO.FINISHTYPE? ConvertToBOFinishType(DO.TYPEOFTREATMENT? typeOfTreatment)
        {
            return typeOfTreatment switch
            {
                DO.TYPEOFTREATMENT.SELFCANCELLATION => BO.FINISHTYPE.SELFCANCELLATION,
                DO.TYPEOFTREATMENT.CANCALINGANADMINISTRATOR => BO.FINISHTYPE.CANCALINGANADMINISTRATOR,
                DO.TYPEOFTREATMENT.TREATE => BO.FINISHTYPE.TREATE,
                _ => null  // במקרה של ערך לא מוכר, מחזירים null
            };
        }

        //private double CalculateDistance(DO.Location volunteerLoc, DO.Location callLoc)
        //{
        //    double deltaX = volunteerLoc.X - callLoc.X;
        //    double deltaY = volunteerLoc.Y - callLoc.Y;
        //    return Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
        //}

    }
}
