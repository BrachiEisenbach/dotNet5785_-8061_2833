using BO;
using DalApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
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



    }


}
