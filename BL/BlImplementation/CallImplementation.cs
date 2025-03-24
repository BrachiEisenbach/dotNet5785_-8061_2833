
using BlApi;
using BO;
using DalApi;
using DO;
using Helpers;

namespace BlImplementation
{
    internal class CallImplementation : ICall
    {
        private readonly DalApi.IDal _dal = DalApi.Factory.Get;

        public IEnumerable<BO.CallInList> GetCallList(Enum? STATUS, object? valFilter, Enum? TYPEOFCALL)
        {
            //      var calls = _dal.Call.ReadAll();
            //      //  שלב 1: סינון לפי הפרמטר הראשון (אם הועבר)
            //      if (STATUS != null && valFilter != null)
            //      {
            //          calls = calls.Where(call =>
            //    STATUS.GetType() == typeof(BO.STATUS) && valFilter is BO.STATUS status && call.Status == status ||
            //    STATUS.GetType() == typeof(BO.TYPEOFCALL) && valFilter is BO.TYPEOFCALL type && call.TypeOfCall == type
            //);
            //      }
            //      //  שלב 2: השארת ההקצאה האחרונה של כל קריאה
            //      calls = calls.GroupBy(call => call.CallId)
            //                   .Select(group => group.OrderByDescending(c => c.OpenTime).First());

            //      //  שלב 3: מיון לפי הפרמטר השלישי (אם הועבר)
            //      calls = TYPEOFCALL switch
            //      {
            //          BO.STATUS => calls.OrderBy(call => call.Status),
            //          BO.TYPEOFCALL => calls.OrderBy(call => call.TypeOfCall),
            //          _ => calls.OrderBy(call => call.CallId) // ברירת מחדל: לפי מספר קריאה
            //      };

            //      return calls;


        }


        // done כמעט
        public BO.Call GetCallDetails(int callId)
        {
            var callDO = _dal.Call.Read(callId);
            if (callDO == null)
            {
                throw new BlDoesNotExistException($"קריאה עם מזהה {callId} לא נמצאה.");
            }
            var callAssignments = _dal.Assignment.ReadAll()
                          .Where(assign => assign.CallId == callId)
                          .Select(assign => new BO.CallAssignInList
                          {
                              VolunteerId = assign.VolunteerId,
                              VolunteerName = _dal.Volunteer.Read(assign.VolunteerId)?.FullName,
                              EntryTimeForTreatment = assign.EntryTimeForTreatment,
                              EndTimeOfTreatment = assign.EndTimeOfTreatment,
                              TypeOfTreatment = (BO.FINISHTYPE?)assign.TypeOfTreatment                           // הוסף עוד שדות לפי הצורך
                          }).ToList();
            return new BO.Call
            {
                Id = callDO.Id,
                //TypeOfCall = ConvertToDOType(callDO.TypeOfCall),
                TypeOfCall = (BO.TYPEOFCALL)callDO.TypeOfCall,
                VerbalDescription = callDO.VerbalDescription,
                FullAddress = callDO.FullAddress,
                Latitude = callDO.Latitude,
                Longitude = callDO.Longitude,
                OpenTime = callDO.OpenTime,
                MaxTimeToFinish = callDO.MaxTimeToFinish,
                //בעיה לא ברורה עם הסטטוס
                //Status = (BO.STATUS)callDO.Status,
                listOfCallAssign = callAssignments // שמירת רשימת ההקצאות
            };

        }

        // done
        public void AddCall(BO.Call call)
        {
            if (string.IsNullOrWhiteSpace(call.FullAddress))
                throw new BlArgumentException("כתובת לא יכולה להיות ריקה");

            if (call.Latitude is null || call.Longitude is null)
                throw new BlArgumentException("קואורדינטות גיאוגרפיות חייבות להיות תקינות");

            if (call.OpenTime == default)
                throw new BlArgumentException("זמן פתיחת הקריאה אינו תקין");

            if (call.MaxTimeToFinish is not null && call.MaxTimeToFinish <= call.OpenTime)
                throw new BlArgumentException("זמן סיום מקסימלי חייב להיות מאוחר מזמן פתיחת הקריאה");

            DO.Call newCall = new DO.Call(
                Id: call.Id,  // אם ה-ID מיוצר במסד הנתונים, אפשר לא להעביר אותו כאן
                TypeOfCall: (DO.TYPEOFCALL)call.TypeOfCall,
                VerbalDescription: call.VerbalDescription,
                FullAddress: call.FullAddress,
                Latitude: call.Latitude ?? 0,  // שימוש ב-0 אם הערך null
                Longitude: call.Longitude ?? 0,  // שימוש ב-0 אם הערך null
                OpenTime: call.OpenTime,
                MaxTimeToFinish: call.MaxTimeToFinish
            );
            try
            {
                _dal.Call.Create(newCall);
            }
            catch (Exception ex)
            {
                throw new BlInvalidOperationException("שגיאה בהוספת הקריאה למערכת", ex);
            }

        }

        // done
        public void cancelTreat(int volId, int assiId)
        {
            try
            {
                var assignment = _dal.Assignment.Read(assiId);
                if (assignment == null)
                    throw new BlDoesNotExistException($"הקצאה עם מזהה {assiId} לא נמצאה.");

                // 2. שליפת המתנדב שמבקש לבטל
                var volunteerFromDAL = _dal.Volunteer.Read(volId);
                if (volunteerFromDAL == null)
                    throw new BlDoesNotExistException($"המשתמש עם מזהה {volId} לא נמצא.");

                // 3. בדיקת הרשאה - רק מנהל או המתנדב עצמו רשאים לבטל
                if (volunteerFromDAL.Role != DO.ROLE.ADMIN && volunteerFromDAL.Id != assignment.VolunteerId)
                    throw new BlUnauthorizedException("אין לך הרשאה לבטל את הטיפול.");

                // 4. בדיקה שההקצאה עדיין פתוחה
                var call = _dal.Call.Read(assignment.CallId);
                if (call == null)
                    throw new BlDoesNotExistException($"קריאה עם מזהה {assignment.CallId} לא נמצא.");

                if (assignment.EndTimeOfTreatment != null)
                    throw new BlInvalidOperationException("לא ניתן לבטל הקצאה שכבר הסתיימה.");

                if (call.MaxTimeToFinish != null && call.MaxTimeToFinish < DateTime.Now)
                    throw new BlInvalidOperationException("לא ניתן לבטל טיפול לאחר שפג תוקפה של הקריאה.");

                // 5. עדכון הנתונים
                var updatedAssignment = assignment with
                {
                    EndTimeOfTreatment = ClockManager.Now,
                    TypeOfTreatment = volunteerFromDAL.Id == assignment.VolunteerId
                                      ? TYPEOFTREATMENT.SELFCANCELLATION
                                      : TYPEOFTREATMENT.CANCALINGANADMINISTRATOR
                };

                // 6. שמירה לשכבת הנתונים
                _dal.Assignment.Update(updatedAssignment);
            }

            catch (BlDoesNotExistException)
            {
                throw;
            }
            catch (BlUnauthorizedException)
            {
                throw;
            }

            catch (Exception ex)
            {
                // חריגה לא צפויה – נעטוף בחריגה עסקית מתאימה
                throw new BlInvalidOperationException("שגיאה בעת ביטול הטיפול", ex);
            }

        }

        // done כמעט
        public void chooseCall(int volId, int callId)
        {
            try
            {
                var call = _dal.Call.Read(callId);
                if (call == null)
                    throw new BlDoesNotExistException($"קריאה עם מזהה {callId} לא נמצאה.");
                // 2. בדיקה שהקריאה לא טופלה ושאין הקצאה פתוחה
                var existingAssignments = _dal.Assignment.ReadAll(a => a.CallId == callId);

                if (existingAssignments.Any(a => a.EndTimeOfTreatment == null))
                    throw new BlInvalidOperationException("לא ניתן לבחור קריאה שכבר נמצאת בטיפול.");

                // 3. בדיקה שלא פג תוקפה של הקריאה
                if (call.MaxTimeToFinish != null && call.MaxTimeToFinish < ClockManager.Now)
                    throw new BlInvalidOperationException("לא ניתן לבחור קריאה שתוקפה פג.");

                // 4. שליפת המתנדב מה-DAL
                var volunteer = _dal.Volunteer.Read(volId);
                if (volunteer == null)
                    throw new BlDoesNotExistException($"המתנדב עם מזהה {volId} לא נמצא.");

                // 5. יצירת הקצאה חדשה
                var newAssignment = new DO.Assignment(
                    Id: 0, //?????????????? מה צריך להכניס פה?
                    //Id: GenerateNewAssignmentId(),  // פונקציה שתייצר מזהה חדש להקצאה
                    CallId: callId,
                    VolunteerId: volId,
                    EntryTimeForTreatment: ClockManager.Now,
                    EndTimeOfTreatment: null,
                    TypeOfTreatment: null
                );
                // 6. שמירת ההקצאה החדשה
                _dal.Assignment.Create(newAssignment);
            }
            catch (DalDoesNotExistException ex)
            {
                // תרגום חריגת DAL לחריגה של BL
                throw new BlDoesNotExistException("אחד מהפריטים המבוקשים אינו קיים.", ex);
            }
            catch (Exception ex)
            {
                throw new BlInvalidOperationException("שגיאה בעת בחירת קריאה לטיפול", ex);
            }
        }

        public void DeleteCall(int callId)
        {
            throw new NotImplementedException();
        }

        // done
        public int[] GetCallCountsByStatus()
        {
            var calls = GetCallList(null, null, null);
            return calls
                        .GroupBy(call => (int)call.Status)  // מקבץ לפי הסטטוס (מותאם לאינדקס)
                        .ToDictionary(g => g.Key, g => g.Count()) // יוצר מילון {סטטוס -> כמות קריאות}
                        .Select(kvp => kvp.Value) // שולף את הכמויות
                        .ToArray(); // ממיר למערך
        }



        public IEnumerable<ClosedCallInList> GetClosedCallInList(int volId, BO.TYPEOFCALL? tOfCall)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<OpenCallInList> GetOpenCallInList(int volId, BO.TYPEOFCALL? tOfCall, BO.TYPEOFCALL? tOfCall2)
        {
            throw new NotImplementedException();
        }

        public void UpdateCallDetails(BO.Call call)
        {

        }

        public void UpdateCallStatus()
        {
            throw new NotImplementedException();
        }

        public void updateFinishTreat(int volId, int callId)
        {
            throw new NotImplementedException();
        }
    }
}

