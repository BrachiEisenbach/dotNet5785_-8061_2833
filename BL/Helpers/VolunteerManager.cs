using DalApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
    internal static class VolunteerManager
    {
        private static IDal s_dal = Factory.Get; //stage 4


        //המרת סוג ה ENUM מ BO לDO
        internal static DO.ROLE ConvertToDOType(BO.ROLE boType)
        {
            return (DO.ROLE)Enum.Parse(typeof(DO.ROLE), boType.ToString());
        }

        //המרת סוג ה ENUM מ DO ל BO
        internal static BO.ROLE ConvertToBOType(DO.ROLE boType)
        {
            return (BO.ROLE)Enum.Parse(typeof(DO.ROLE), boType.ToString());
        }

        //internal static BO.VolunteerInList ConvertToBOVolunteerInList(DO.Volunteer volunteer)
        //{
        //    return new BO.VolunteerInList
        //    {
        //        Id = volunteer.Id,
        //        FullName = volunteer.FullName,
        //        Active = volunteer.Active,
        //        AllCallsThatTreated = s_dal.Assignment.ReadAll().Count(a => a.VolunteerId == volunteer.Id && a.TypeOfTreatment == DO.FINISHTYPE.TREATE),
        //        AllCallsThatCanceled = s_dal.Assignment.ReadAll().Count(a => a.VolunteerId == volunteer.Id && a.TypeOfTreatment == DO.FINISHTYPE.CANCEL),
        //        AllCallsThatHaveExpired = s_dal.Assignment.ReadAll().Count(a => a.VolunteerId == volunteer.Id && a.TypeOfTreatment == DO.FINISHTYPE.EXPIRE),
        //        CallId = s_dal.Assignment.ReadAll().Where(a => a.VolunteerId == volunteer.Id).OrderByDescending(a => a.EntryTimeForTreatment).FirstOrDefault()?.CallId
        //    };
        //}


        // פונקציה המחזירה כתובת אפס
        internal static (double, double) GetCoordinatesFromAddress(string address)
        {
            return (0.0, 0.0);
        }
    }

  
}
