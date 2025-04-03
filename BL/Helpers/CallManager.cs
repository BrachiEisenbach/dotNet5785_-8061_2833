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
            return MappingProfile.ConvertToBO(doCall);
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
                DO.TYPEOFTREATMENT.CANCELINGANADMINISTRATOR => BO.FINISHTYPE.CANCALINGANADMINISTRATOR,
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


        private const string ApiKey = "67ebc190aaf5b144782334hkg4d1b14";
        private static readonly HttpClient Client = new HttpClient();

        /// <summary>
        /// מקבלת כתובת ומחזירה את הקואורדינטות (קו רוחב וקו אורך) שלה באמצעות API.
        /// </summary>
        /// <param name="address">כתובת לחיפוש</param>
        /// <returns>זוג ערכים: קו רוחב וקו אורך</returns>
        internal static (double Latitude, double Longitude) FetchCoordinates(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
                throw new BlArgumentException("הכתובת שסופקה אינה תקינה.");

            // יצירת כתובת ה-URL לשליפת הנתונים
            string requestUrl = $"https://geocode.maps.co/search?q={Uri.EscapeDataString(address)}&api_key={ApiKey}";

            try
            {
                // שליחת בקשה וקבלת תשובה
                var responseTask = Client.GetAsync(requestUrl);
                HttpResponseMessage response = responseTask.Result;
                response.EnsureSuccessStatusCode();

                // קריאת תוכן התגובה
                string jsonResult = response.Content.ReadAsStringAsync().Result;

                // המרת ה-JSON לאובייקטים
                var locationData = JsonSerializer.Deserialize<GeoResponse[]>(jsonResult, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (locationData == null || locationData.Length == 0)
                    throw new BlException("לא נמצאו נתוני מיקום לכתובת זו.");

                if (!double.TryParse(locationData[0].Lat, out double lat) ||
                    !double.TryParse(locationData[0].Lon, out double lon))
                {
                    throw new BlException("שגיאה בהמרת הנתונים המספריים.");
                }

                return (lat, lon);
            }
            catch (HttpRequestException httpEx)
            {
                throw new BlException("שגיאה בהתחברות לשרת המפות: " + httpEx.Message);
            }
            catch (JsonException jsonEx)
            {
                throw new BlException("שגיאה בעיבוד הנתונים: " + jsonEx.Message);
            }
            catch (Exception ex)
            {
                throw new BlException("שגיאה כללית בעת שליפת המיקום: " + ex.Message);
            }
        }

        // מחלקת עזר לפענוח התשובה מה-API
        private class GeoResponse
        {
            [JsonPropertyName("lat")]
            public string Lat { get; set; }

            [JsonPropertyName("lon")]
            public string Lon { get; set; }
        }



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
        private static double DegreesToRadians(double degrees)
        {
            return degrees * (Math.PI / 180);
        }
    }
}
