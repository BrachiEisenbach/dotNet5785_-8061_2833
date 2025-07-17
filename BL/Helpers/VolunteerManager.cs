using BO;
using DO;
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
    public static class VolunteerManager
    {
        private static IDal s_dal = Factory.Get; //stage 4

        internal static ObserverManager Observers = new(); //stage 5 

        public static BO.Volunteer GetVolunteerFromDO(DO.Volunteer doVolunteer)
        {
            var boVolunteer=  MappingProfile.ConvertToBO(doVolunteer);
            if (boVolunteer == null)
            {
                System.Diagnostics.Debug.WriteLine("boVolunteer is null");
            }
            // בונים אובייקט חדש, ומעתיקים את כל הערכים, ורק CallInTreate מאתחלים ידנית
            return new BO.Volunteer
            {
                Id = boVolunteer.Id,
                FullName = boVolunteer.FullName,
                Phone = boVolunteer.Phone,
                Email = boVolunteer.Email,
                Password = boVolunteer.Password,
                FullAddress = boVolunteer.FullAddress,
                Latitude = boVolunteer.Latitude,
                Longitude = boVolunteer.Longitude,
                Role = boVolunteer.Role,
                Active = boVolunteer.Active,
                MaxDistance = boVolunteer.MaxDistance,
                TypeOfDistance = boVolunteer.TypeOfDistance,
                AllCallsThatTreated = GetAllCallsThatTreated(boVolunteer.Id),
                AllCallsThatCanceled = GetAllCallsThatCanceled(boVolunteer.Id),
                AllCallsThatHaveExpired = GetAllCallsThatHaveExpired(boVolunteer.Id),
                CallInTreate = GetCallInTreatment(doVolunteer.Id) // כאן מאתחלת את הערך
              
            };
        }

        public static DO.Volunteer GetVolunteerFromBO(BO.Volunteer boVolunteer)
        {
            return MappingProfile.ConvertToDO(boVolunteer);
        }

        //המרת סוג ה ENUM מ BO לDO
        internal static DO.ROLE ConvertToDORole(BO.ROLE boType)
        {
            if (Enum.TryParse<DO.ROLE>(boType.ToString(), out var doRole))
                return doRole;
            throw new ArgumentException($"No matching DO.ROLE for {boType}");
        }


        //המרת סוג ה ENUM מ DO ל BO
        internal static BO.ROLE ConvertToBORole(DO.ROLE doType)
        {
            if (Enum.TryParse<BO.ROLE>(doType.ToString(), ignoreCase: true, out var boRole))
            {
                System.Diagnostics.Debug.WriteLine($"success to parse role: {doType}");
                return boRole;
            }

            
            System.Diagnostics.Debug.WriteLine($"Failed to parse role: {doType}");
            throw new ArgumentException($"No matching BO.ROLE for {doType}");
        }


        // המרת ENUM של סוג מרחק מ-DO ל-BO 
        internal static BO.TYPEOFDISTANCE ConvertToBOType(DO.TYPEOFDISTANCE doType)
        {
            if (Enum.TryParse<BO.TYPEOFDISTANCE>(doType.ToString(), out var boType))
            {
                System.Diagnostics.Debug.WriteLine($"success to parse type d: {doType}");
                return boType;
            }


            System.Diagnostics.Debug.WriteLine($"Failed to parse type d: {doType}");
            throw new ArgumentException($"No matching BO.TYPEOFDISTANCE for {doType}");
        }

        // המרת ENUM של סוג מרחק מ-BO ל-DO
        internal static DO.TYPEOFDISTANCE ConvertToDOType(BO.TYPEOFDISTANCE boType)
        {
            if (Enum.TryParse<DO.TYPEOFDISTANCE>(boType.ToString(), out var doType))
                return doType;
            throw new ArgumentException($"No matching DO.TYPEOFDISTANCE for {boType}");
        }


        // המרת FINISHTYPE מ-BO ל-DO
        internal static DO.TYPEOFTREATMENT ConvertToDOTypeFinish(BO.FINISHTYPE finishType)
        {
            if (Enum.TryParse(finishType.ToString(), out DO.TYPEOFTREATMENT result))
            {
                System.Diagnostics.Debug.WriteLine($"success to parse type f: {result}");
                return result;
            }


            System.Diagnostics.Debug.WriteLine($"Failed to parse type f: {result}");
            throw new ArgumentException($"No matching DO.TYPEOFTREATMENT for {finishType}");
        }

        // המרת FINISHTYPE מ-DO ל-BO
        internal static BO.FINISHTYPE ConvertToBOTypeFinish(DO.TYPEOFTREATMENT finishType)
        {
            if (Enum.TryParse(finishType.ToString(), out BO.FINISHTYPE result))
                return result;

            throw new ArgumentException($"No matching BO.FINISHTYPE for {finishType}");
        }





        // פונקציה לחישוב מספר הקריאות שטופלו על ידי המתנדב
        public static int GetAllCallsThatTreated(int volunteerId)
        {
            var finishType = ConvertToDOTypeFinish(BO.FINISHTYPE.TREATE);
            return s_dal.Assignment.ReadAll().Count(a => a.VolunteerId == volunteerId && a.TypeOfTreatment == finishType);
        }


        // פונקציה לחישוב מספר הקריאות שבוטלו על ידי המתנדב
        public static int GetAllCallsThatCanceled(int volunteerId)
        {
            var finishType = ConvertToDOTypeFinish(BO.FINISHTYPE.SELFCANCELLATION); // או כל אחד שמתאים לך
            return s_dal.Assignment.ReadAll().Count(a => a.VolunteerId == volunteerId && a.TypeOfTreatment == finishType);
        }

        // פונקציה לחישוב מספר הקריאות שפג תוקפן
        public static int GetAllCallsThatHaveExpired(int volunteerId)
        {
            var finishType = ConvertToDOTypeFinish(BO.FINISHTYPE.CANCELLATIONHASEXPIRED);
            return s_dal.Assignment.ReadAll().Count(a => a.VolunteerId == volunteerId && a.TypeOfTreatment == finishType);
        }

        // פונקציה שמחזירה את הקריאה האחרונה שנמצאת בטיפול על ידי מתנדב, כאובייקט מסוג CallInProgress
        public static BO.CallInProgress? GetCallInTreatment(int volunteerId)
        {
            try
            {
                var assignments = s_dal.Assignment.ReadAll()
                    .Where(a => a.VolunteerId == volunteerId && !a.EndTimeOfTreatment.HasValue)
                    .OrderByDescending(a => a.EntryTimeForTreatment);

                foreach (var assignment in assignments)
                {
                    var call = s_dal.Call.Read(assignment.CallId);
                    if (call == null)
                        continue;

                    var riskRange = s_dal.Config.RiskRange;
                    var status = CallManager.CalculateStatus(call, riskRange);

                    // בודק שהסטטוס הוא בדיוק InTreatment (או סטטוס שרוצים)
                    if (status == STATUS.InTreatment)  // נניח שזה enum עם ערכים
                    {
                        var vol = s_dal.Volunteer.Read(volunteerId) ?? throw new BlDoesNotExistException($"The Volunteer with ID={volunteerId} does not exist");
                        var volLatitude = vol.Latitude ?? throw new BlArgumentException($"The Volunteer with ID={volunteerId} havn't Latitude");
                        var volLongitude = vol.Longitude ?? throw new BlArgumentException($"The Volunteer with ID={volunteerId} havn't Longitude");

                        return new BO.CallInProgress
                        {
                            Id = assignment.Id,
                            CallId = assignment.CallId,
                            TypeOfCall = CallManager.ConvertToBOType(call.TypeOfCall),
                            VerbalDescription = call.VerbalDescription,
                            FullAddress = call.FullAddress,
                            OpenTime = call.OpenTime,
                            MaxTimeToFinish = call.MaxTimeToFinish,
                            EnterTime = assignment.EntryTimeForTreatment,
                            Distance = CalculateHaversineDistance(call.Latitude, call.Longitude, volLatitude, volLongitude),
                            Status = status
                        };
                    }
                }

                // אם לא נמצאה קריאה במצב InTreatment
                return null;
            }
            catch (DalDoesNotExistException dalDoesNotExistException)
            {
                throw new BlDoesNotExistException($"The Volunteer with ID={volunteerId} does not exist", dalDoesNotExistException);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("GetCallInTreatment error: " + ex);
                throw new BlException("Error while getting message details.", ex);
            }
        }


        //פונקציה להחזיר רשימה של מתנדבים על פי סטטוס פעילות

        public static IEnumerable<DO.Volunteer> GetIsActiveVolunteers(bool? Act)
        {
            if (Act.HasValue)
            {
                var volunteers = s_dal.Volunteer.ReadAll().Where(v => (v.Active == Act.Value));
                return volunteers;
            }
            else
            {
                var volunteers = s_dal.Volunteer.ReadAll();
                return volunteers;
            }
        }
        //פו הממירה BO.volunteer לאובייקט BO.VolunteerInList
        internal static BO.VolunteerInList ConvertToBOVolunteerInList(BO.Volunteer volunteer)
        {
            return new BO.VolunteerInList
            {
                Id = volunteer.Id,
                FullName = volunteer.FullName,
                Active = volunteer.Active,
                AllCallsThatTreated = volunteer.AllCallsThatTreated,
                AllCallsThatCanceled = volunteer.AllCallsThatCanceled,
                AllCallsThatHaveExpired = volunteer.AllCallsThatHaveExpired,
                CallId = volunteer.CallInTreate?.CallId,
                TypeOfCall = volunteer.CallInTreate?.TypeOfCall ?? BO.TYPEOFCALL.NONE
            };
        }

        //פו המקבלת שתי כתובות ומחשבת להן אורך ורוחב ואז שולחת אותן לחישוב המרחק בינהן
        public static double GetDistanceBetweenAddresses(string address1, string address2)
        {
            System.Diagnostics.Debug.WriteLine($"Trying to fetch coordinates for:'{{address1}}' and '{{address2}}'");
            var coord1 = FetchCoordinates(address1);

            var coord2 = FetchCoordinates(address2);

            return CalculateHaversineDistance(coord1.Latitude, coord1.Longitude, coord2.Latitude, coord2.Longitude);
        }




        //פו המקבלת קווי אורך ורוחב של 2 כתובות ומחזירה מרחק בינהן

        private static double CalculateHaversineDistance(double lat1, double lon1, double lat2, double lon2)
        {
            //if (lat2 == null || lon2 == null)
            //    throw new BlArgumentException("The coordinates provided are invalid.");
            const double EarthRadius = 6371;

            double dLat = CallManager.DegreesToRadians(lat2 - lat1);
            double dLon = CallManager.DegreesToRadians(lon2 - lon1);

            lat1 = CallManager.DegreesToRadians(lat1);
            lat2 = CallManager.DegreesToRadians(lat2);

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(lat1) * Math.Cos(lat2) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return EarthRadius * c;
        }




        //private const string ApiKey = "6873bc416eea5543811152dmhaff213";
        //private static readonly HttpClient Client = new HttpClient();

        /// <summary>
        /// מקבלת כתובת ומחזירה את הקואורדינטות (קו רוחב וקו אורך) שלה באמצעות API.
        /// </summary>
        /// <param name="address">כתובת לחיפוש</param>
        /// <returns>זוג ערכים: קו רוחב וקו אורך</returns>
        //    public static (double Latitude, double Longitude) FetchCoordinates(string address)
        //    {
        //        System.Diagnostics.Debug.WriteLine(address); 

        //        if (string.IsNullOrWhiteSpace(address))
        //            throw new BlArgumentException("The address provided is invalid.");

        //        // יצירת כתובת ה-URL לשליפת הנתונים
        //        string requestUrl = $"https://geocode.maps.co/search?q={Uri.EscapeDataString(address)}&api_key={ApiKey}";
        //        System.Diagnostics.Debug.WriteLine(requestUrl);

        //        try
        //        {
        //            // שליחת בקשה וקבלת תשובה
        //            var responseTask = Client.GetAsync(requestUrl);
        //            HttpResponseMessage response = responseTask.Result;
        //            response.EnsureSuccessStatusCode();

        //            // קריאת תוכן התגובה
        //            string jsonResult = response.Content.ReadAsStringAsync().Result;
        //            System.Diagnostics.Debug.WriteLine(jsonResult); 

        //            // המרת ה-JSON לאובייקטים
        //            var locationData = JsonSerializer.Deserialize<GeoResponse[]>(jsonResult, new JsonSerializerOptions
        //            {
        //                PropertyNameCaseInsensitive = true
        //            });
        //            System.Diagnostics.Debug.WriteLine(locationData);

        //            if (locationData == null || locationData.Length == 0)
        //            {
        //                Console.WriteLine(jsonResult); // הדפס את התוכן כדי לבדוק
        //                throw new BlException("No location data found for this address.");
        //            }

        //            if (!double.TryParse(locationData[0].Lat, out double lat) ||
        //                !double.TryParse(locationData[0].Lon, out double lon))
        //            {
        //                throw new BlException("Error converting numeric data.");
        //            }

        //            return (lat, lon);
        //        }
        //        catch (HttpRequestException httpEx)
        //        {
        //            throw new BlException("Error connecting to the map server: " + httpEx.Message);
        //        }
        //        catch (JsonException jsonEx)
        //        {
        //            throw new BlException("Data processing error: " + jsonEx.Message);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw new BlException("General error while retrieving location: " + ex.Message);
        //        }
        //    }

        //    // מחלקת עזר לפענוח התשובה מה-API
        //    private class GeoResponse
        //    {
        //        [JsonPropertyName("lat")]
        //        public string Lat { get; set; }

        //        [JsonPropertyName("lon")]
        //        public string Lon { get; set; }
        //    }

        public static (double Latitude, double Longitude) FetchCoordinates(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
                throw new ArgumentException("The address provided is invalid.");

            string requestUrl = $"https://nominatim.openstreetmap.org/search?q={Uri.EscapeDataString(address)}&format=json&limit=1";
            System.Diagnostics.Debug.WriteLine("Requesting: " + requestUrl);

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "MyGeoApp/1.0 (youremail@example.com)");

            try
            {
                var response = client.GetAsync(requestUrl).GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode();

                var jsonResult = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                System.Diagnostics.Debug.WriteLine("Response: " + jsonResult);

                var locationData = JsonSerializer.Deserialize<List<NominatimResponse>>(jsonResult, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (locationData == null || locationData.Count == 0)
                    throw new Exception("No location data found for this address.");

                if (!double.TryParse(locationData[0].Lat, out double lat) ||
                    !double.TryParse(locationData[0].Lon, out double lon))
                {
                    throw new Exception("Error converting coordinates to double.");
                }

                return (lat, lon);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error fetching coordinates: {ex.Message}");
                throw;
            }
        }

        private class NominatimResponse
        {
            [JsonPropertyName("lat")]
            public string Lat { get; set; }

            [JsonPropertyName("lon")]
            public string Lon { get; set; }
        }


    }


}
