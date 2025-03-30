
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

        // done כמעט. צריך לעבור לבדוק את הלוגיקה.
        public IEnumerable<BO.CallInList> GetCallList(Enum? statusFilter, object? valFilter, Enum? typeOfCallSort)
        {
            try { 
            var calls = _dal.Call.ReadAll().ToList();
            var riskRange = _dal.Config.RiskRange;

            if (statusFilter is BO.STATUS status && valFilter is BO.STATUS)
            {
                calls = calls.Where(call => CallManager.CalculateStatus(call, riskRange) == status).ToList();
            }

            if (statusFilter is BO.TYPEOFCALL type && valFilter is BO.TYPEOFCALL)
            {
                calls = calls.Where(call => CallManager.ConvertToBOType(call.TypeOfCall) == type).ToList();
            }

            calls = calls.GroupBy(call => call.Id)
                .Select(group => group.OrderByDescending(c => c.OpenTime).First())
                .ToList();

            calls = typeOfCallSort switch
            {
                BO.STATUS => calls.OrderBy(call => CallManager.CalculateStatus(call, riskRange)).ToList(),
                BO.TYPEOFCALL => calls.OrderBy(call => call.TypeOfCall).ToList(),
                _ => calls.OrderBy(call => call.Id).ToList() // ברירת מחדל: לפי מספר קריאה
            };

            return calls.Select(call => new BO.CallInList
            {
                //Id = call.Id,
                CallId = call.Id,
                TypeOfCall = (BO.TYPEOFCALL)call.TypeOfCall,
                Status = CallManager.CalculateStatus(call, riskRange),
                OpenTime = call.OpenTime
            });
            }
            catch (Exception ex) { throw new BlException("שגיאה בעת קבלת הקריאות ברשימה"); };

        }


        // done כמעט
        public BO.Call GetCallDetails(int callId)
        {
            try { 
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
                              TypeOfTreatment = (BO.FINISHTYPE?)assign.TypeOfTreatment                      
                          }).ToList();

            var riskRange = _dal.Config.RiskRange;

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
                Status = CallManager.CalculateStatus(callDO, riskRange),
                listOfCallAssign = callAssignments // שמירת רשימת ההקצאות
            };
            }
            catch (BlDoesNotExistException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BlException("שגיאה בעת קבלת פרטי הודעה.");
            }
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

        // done
        public void DeleteCall(int callId)
        {
            try
            {
                //  שליפת הקריאה מהמאגר
                var call = GetCallDetails(callId);
                if (call.Status != STATUS.Open)
                    throw new BlInvalidOperationException("לא ניתן למחוק קריאה שאינה בסטטוס פתוח.");

                //  בדיקה שלא קיימות הקצאות לקריאה זו
                var assignments = _dal.Assignment.ReadAll().Where(a => a.CallId == callId);
                if (assignments.Any())
                    throw new BlInvalidOperationException("לא ניתן למחוק קריאה שהוקצתה בעבר למתנדבים.");

                _dal.Call.Delete(callId);
            }
            catch (Exception ex)
            {
                throw new BlInvalidOperationException("שגיאה במחיקת הקריאה", ex);
            }
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

        // done...
        public IEnumerable<ClosedCallInList> GetClosedCallInList(int volId, BO.TYPEOFCALL? tOfCall, BO.ClosedCallInListField? sortBy)
        {
            var closedCalls = _dal.Assignment.ReadAll()
                .Where(a => a.VolunteerId == volId && a.EndTimeOfTreatment != null) // רק קריאות שטופלו
                .Select(a =>
                    {
                        var call = GetCallDetails(a.CallId); // שליפת הקריאה המתאימה
                        if (call == null) return null;
                        //}
                        return new BO.ClosedCallInList
                        {
                            Id = call.Id,
                            TypeOfCall = (BO.TYPEOFCALL)(int)call.TypeOfCall, // המרת ENUM
                            FullAddress = call.FullAddress,
                            OpenTime = call.OpenTime,
                            EntryTimeForTreatment = a.EntryTimeForTreatment,
                            EndTimeOfTreatment = a.EndTimeOfTreatment,
                            TypeOfTreatment = CallManager.ConvertToBOFinishType(a.TypeOfTreatment)
                        };
                    })
                .Where(c => c != null) // סינון קריאות ריקות
                .ToList();

            // סינון לפי סוג קריאה אם הועבר ערך
            if (tOfCall.HasValue)
            {
                closedCalls = closedCalls.Where(c => c.TypeOfCall == tOfCall.Value).ToList();
            }

            // מיון לפי הפרמטר שנבחר, ברירת מחדל לפי מספר קריאה
            closedCalls = sortBy switch
            {
                BO.ClosedCallInListField.OpenTime => closedCalls.OrderBy(c => c.OpenTime).ToList(),
                BO.ClosedCallInListField.EntryTimeForTreatment => closedCalls.OrderBy(c => c.EntryTimeForTreatment).ToList(),
                BO.ClosedCallInListField.EndTimeOfTreatment => closedCalls.OrderBy(c => c.EndTimeOfTreatment).ToList(),
                _ => closedCalls.OrderBy(c => c.Id).ToList() // ברירת מחדל: לפי מזהה קריאה
            };

            return closedCalls;
        }


        // done...
        public IEnumerable<OpenCallInList> GetOpenCallInList(int volId, BO.TYPEOFCALL? tOfCall, BO.OpenCallInList? sortBy)
        {
            //var calls = GetCallList();
            var riskRange = _dal.Config.RiskRange;
            //var volunteer = GetVolunteerDetails(volId) ?? throw new BlDoesNotExistException($"מתנדב עם מזהה {volId} לא נמצא.");
            var volunteer = _dal.Volunteer.Read(volId) ?? throw new BlDoesNotExistException($"מתנדב עם מזהה {volId} לא נמצא.");

            var openCalls = _dal.Call.ReadAll()
                    .Where(call =>
                    {
                        var status = CallManager.CalculateStatus(call, riskRange);
                        return status == BO.STATUS.Open || status == BO.STATUS.OpenDangerZone;
                    })
                    .Where(call => !tOfCall.HasValue || CallManager.ConvertToBOType(call.TypeOfCall) == tOfCall.Value); // סינון לפי סוג הקריאה (אם נדרש)
            var openCallsList =
                from call in openCalls
                    //let distance = CalculateDistance(volunteer.Location, call.Location) // חישוב מרחק (let)
                group call by call.TypeOfCall into callGroups // קיבוץ לפי סוג קריאה
                from call in callGroups // ביטול הקיבוץ כדי שנוכל להמשיך עם הנתונים
                select new BO.OpenCallInList // select new
                {
                    Id = call.Id,
                    TypeOfCall = CallManager.ConvertToBOType(call.TypeOfCall),
                    VerbalDescription = call.VerbalDescription,
                    FullAddress = call.FullAddress,
                    OpenTime = call.OpenTime,
                    MaxTimeToFinish = call.MaxTimeToFinish,
                    //Distance = distance
                };

            openCallsList = sortBy switch
            {
                BO.OpenCallInList list when list.TypeOfCall != default => openCallsList.OrderBy(c => c.TypeOfCall),
                BO.OpenCallInList list when list.OpenTime != default => openCallsList.OrderBy(c => c.OpenTime),
                BO.OpenCallInList list when list.Distance != default => openCallsList.OrderBy(c => c.Distance),
                _ => openCallsList.OrderBy(c => c.Id) // ברירת מחדל: מיון לפי מזהה קריאה
            };

            return openCallsList;

        }

        public void UpdateCallDetails(BO.Call call)
        {

        }

        //public void UpdateCallStatus()
        //{
        //    throw new NotImplementedException();
        //}

        public void updateFinishTreat(int volId, int callId)
        {
            try
            {
                // שלב 1: שליפת ההקצאה המתאימה משכבת הנתונים
                var assignment = _dal.Assignment.Read(callId)
                    ?? throw new BO.BlDoesNotExistException("לא נמצאה הקצאה עם המזהה הנתון.");

                // שלב 2: בדיקה שהמתנדב אכן מוקצה לקריאה זו
                if (assignment.VolunteerId != volId)
                    throw new BO.BlUnauthorizedException("אין לך הרשאה לסיים טיפול בקריאה זו.");

                // שלב 3: בדיקה שהקריאה עדיין פתוחה (לא טופלה, לא בוטלה, לא פג תוקפה)
                if (assignment.EndTimeOfTreatment != null)
                    throw new BO.BlInvalidOperationException("לא ניתן לסיים טיפול בקריאה שכבר נסגרה.");

                // שלב 4: עדכון הנתונים
                assignment.EndTimeOfTreatment = ClockManager.Now;
                assignment.TypeOfTreatment = DO.TYPEOFTREATMENT.Handled; // עדכון סוג הסיום

                // שלב 5: ניסיון עדכון בשכבת הנתונים
                _dal.Assignment.Update(assignment);
            }
            catch (DO.DOException ex)
            {
                // אם שכבת הנתונים זרקה חריגה, נעטוף ונשלח חריגה ברמת ה-BO
                throw new BO.BOException("שגיאה בעת סיום הטיפול: " + ex.Message, ex);
            }
        }
    }
}

