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
        internal static ObserverManager Observers = new(); //stage 5 
        public static BO.Call GetCallFromDO(DO.Call doCall)
        {
            return MappingProfile.ConvertToBO(doCall, s_dal.Config.RiskRange);
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




        /// <summary>
        /// מתודה המעדכנת את כל הקריאות הפתוחות שפג תוקפן.
        /// יש לזמן אותה מ-ClockManager בכל עדכון שעון.
        /// </summary>
        internal static void UpdateExpiredOpenCalls()
        {
            // מאחזר את רדיוס הסיכון כדי לחשב סטטוס נוכחי
            var riskRange = s_dal.Config.RiskRange;
            DateTime now = AdminManager.Now; // קבל את השעה הנוכחית מה-AdminManager שלך

            // עובר על כל הקריאות ב-DAL
            var allCalls = s_dal.Call.ReadAll().ToList(); // ToList כדי שלא תהיה בעיה עם שינוי תוך כדי איטרציה

            foreach (var call in allCalls)
            {
                // חשב את הסטטוס הנוכחי של הקריאה
                var currentStatus = CalculateStatus(call, riskRange);

                // תנאי: זמן הסיום המקסימלי עבר (MaxTimeToFinish אינו null ובעבר)
                // וגם הקריאה עדיין בסטטוס 'פתוחה' או 'פתוחה בסיכון' (כלומר, לא סיימו לטפל בה עדיין)
                if (call.MaxTimeToFinish.HasValue && call.MaxTimeToFinish.Value < now &&
                    (currentStatus == BO.STATUS.Open || currentStatus == BO.STATUS.OpenDangerZone))
                {
                    DO.Assignment oldAssignment = null; // נשנה את השם כדי להבהיר

                    // נסה למצוא הקצאה קיימת עבור הקריאה שזמן הטיפול שלה עדיין לא הסתיים
                    var existingAssignments = s_dal.Assignment.ReadAll(a => a.CallId == call.Id && a.EndTimeOfTreatment == null).ToList();

                    if (existingAssignments.Any())
                    {
                        // קיימת הקצאה פתוחה - יש לעדכן אותה.
                        // מכיוון שהמאפיינים הם init-only, ניצור אובייקט חדש עם הנתונים המעודכנים.
                        oldAssignment = existingAssignments.First(); // קח את ההקצאה הפתוחה הראשונה

                        // *** התיקון כאן: צור אובייקט חדש עם הנתונים המעודכנים ***
                        DO.Assignment newAssignment = oldAssignment with // השתמש ב-`with` expression אם BO.Assignment הוא record
                        {
                            EndTimeOfTreatment = now,
                            TypeOfTreatment = DO.TYPEOFTREATMENT.CANCELLATIONHASEXPIRED
                        };
                        s_dal.Assignment.Update(newAssignment); // עדכן את ה-DAL עם האובייקט החדש
                    }
                    else
                    {
                        // אין הקצאה פתוחה - צור הקצאה חדשה מסוג "פג תוקף"
                        DO.Assignment newAssignment = new DO.Assignment(
                            Id: 0, // DAL אמור להקצות ID חדש
                            CallId: call.Id,
                            VolunteerId: 0, // ת.ז מתנדב שהוא 0 כפי שנדרש
                            EntryTimeForTreatment: now,
                            EndTimeOfTreatment: now,
                            TypeOfTreatment: DO.TYPEOFTREATMENT.CANCELLATIONHASEXPIRED
                        );
                        s_dal.Assignment.Create(newAssignment);
                    }
                    // שלב 5 (א): שליחת הודעה למשקיפים על עדכון הקריאה הספציפית
                    CallManager.Observers.NotifyListUpdated();

                }
            }

            // שלב 5 (ב): לאחר השלמת המעבר על כל הקריאות, "תישלח הודעה" למשקיפים על עדכון רשימת הקריאות
            Observers.NotifyListUpdated(); // זה כבר קיים אצלך, מפעיל את Observer שמפעיל את GetOpenCallInList
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

    // כאן היא עדיין בטיפול פעיל
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


        //public static void PrintLocationDebugInfo(DO.Volunteer vol, DO.Call call)
        //{
        //    Console.WriteLine($"--- DEBUG DISTANCE CHECK ---");
        //    Console.WriteLine($"Volunteer Address: {vol.FullAddress}");
        //    Console.WriteLine($"→ Latitude: {vol.Latitude}, Longitude: {vol.Longitude}");

        //    Console.WriteLine($"Call Address: {call.FullAddress}");
        //    Console.WriteLine($"→ Latitude: {call.Latitude}, Longitude: {call.Longitude}");

        //    var distance = GetDistance(vol, call);
        //    Console.WriteLine($"Calculated Distance: {distance} km");
        //}



        private const string ApiKey = "67ebc190aaf5b144782334hkg4d1b14";
        private static readonly HttpClient Client = new HttpClient();




        /// <summary>
        /// מחשב את המרחק בין מתנדב לקריאה על פי הקואורדינטות של שניהם.
        /// </summary>
        /// <param name="volunteer">המתנדב</param>
        /// <param name="call">הקריאה</param>
        /// <returns>המרחק בקילומטרים בין המתנדב לקריאה</returns>
        // ב-CallManager.cs

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

            // חישוב ההפרש בקווי רוחב ובאורך (כעת שניהם כבר ברדיאנים)
            double dLat = lat2 - lat1;
            double dLon = lon2 - lon1;

            // חישוב המרחק בעזרת נוסחת Haversine
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                               Math.Cos(lat1) * Math.Cos(lat2) *
                               Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            // חישוב המרחק (רדיוס כדור הארץ * זווית הקשת)
            double distance = EarthRadius * c;
            //if (distance > 500) // נניח שמרחק מעל 500 ק"מ חשוד
            //    PrintLocationDebugInfo(volunteer, call);
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
