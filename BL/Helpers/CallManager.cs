using DalApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using BO;
using AutoMapper;

namespace Helpers
{
    internal static class CallManager
    {
        public static BO.Call GetCallFromDO(DO.Call doCall)
        {
            return MappingProfile.ConvertToBO(doCall);   //error because there was'nt send riskRange
        }

        public static DO.Call GetCallFromBO(BO.Call boCall)
        {
            return MappingProfile.ConvertToDO(boCall);
        }

        internal static IDal s_dal = Factory.Get; //stage 4

        //המרת סוג ה ENUM מ BO לDO
        public static DO.TYPEOFCALL ConvertToDOType(BO.TYPEOFCALL boType)
        {
            return (DO.TYPEOFCALL)Enum.Parse(typeof(DO.TYPEOFCALL), boType.ToString());
        }

        //המרת סוג ה ENUM מ DO ל BO
        public static BO.TYPEOFCALL ConvertToBOType(DO.TYPEOFCALL boType)
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
                DO.TYPEOFTREATMENT.CANCELINGANADMINISTRATOR => BO.FINISHTYPE.CANCALINGANADMINISTRATOR,
                DO.TYPEOFTREATMENT.TREATE => BO.FINISHTYPE.TREATE,
                _ => null  // במקרה של ערך לא מוכר, מחזירים null
            };
        }





        private const string ApiKey = "67ebc190aaf5b144782334hkg4d1b14";
        private static readonly HttpClient Client = new HttpClient();
       



        /// <summary>
        /// מחשב את המרחק בין מתנדב לקריאה על פי הקואורדינטות של שניהם.
        /// </summary>
        /// <param name="volunteer">המתנדב</param>
        /// <param name="call">הקריאה</param>
        /// <returns>המרחק בקילומטרים בין המתנדב לקריאה</returns>
        internal static double GetDistance(DO.Volunteer volunteer, DO.Call call)
        {
            // רדיוס כדור הארץ בקילומטרים
            const double EarthRadius = 6371;

            // בדיקת תקינות קואורדינטות המתנדב
            if (volunteer.Latitude == null || volunteer.Longitude == null)
            {
                throw new ArgumentNullException("Volunteer latitude or longitude cannot be null.");
            }

            double lat1 = DegreesToRadians(volunteer.Latitude.Value);
            double lon1 = DegreesToRadians(volunteer.Longitude.Value);
            double lat2 = DegreesToRadians(call.Latitude);
            double lon2 = DegreesToRadians(call.Longitude);

            // חישוב ההפרש בקווי רוחב ובאורך
            double dLat = lat2 - lat1;
            double dLon = lon2 - lon1;

            // חישוב המרחק בעזרת נוסחת Haversine
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(lat1) * Math.Cos(lat2) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            // חישוב המרחק (רדיוס כדור הארץ * זווית הקשת)
            double distance = EarthRadius * c;

            return distance;
        }

        /// <summary>
        /// פונקציה להמרת מעלות לרדיאנים.
        /// </summary>
        /// <param name="degrees">הזווית במעלות</param>
        /// <returns>הזווית ברדיאנים</returns>
        internal static double DegreesToRadians(double degrees)
        {
            return degrees * (Math.PI / 180);
        }
    }
}
