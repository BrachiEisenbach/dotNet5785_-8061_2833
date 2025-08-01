﻿
using BlApi;
using BO;
using DalApi;
using DO;
using Helpers;
using System.Diagnostics;


namespace BlImplementation
{
    internal class CallImplementation : BlApi.ICall
    {
        private readonly DalApi.IDal _dal = DalApi.Factory.Get;

        /// <summary>
        /// Retrieves a list of calls with optional filtering and sorting.
        /// </summary>
        /// <param name="statusFilter">An optional filter by status (e.g., Open, Closed).</param>
        /// <param name="valFilter">The value to filter by (based on the status or type of call).</param>
        /// <param name="typeOfCallSort">An optional parameter to sort by type of call.</param>
        /// <returns>A collection of filtered and sorted call details in a list.</returns>
        /// <exception cref="BlDoesNotExistException">Thrown when no calls are found in the system.</exception>

        public IEnumerable<BO.CallInList> GetCallList(Enum? filterBy, object? filterValue, Enum? sortBy)
        {
            try
            {
                IEnumerable<DO.Call> calls;
                IEnumerable<DO.Assignment> assignments;
                IEnumerable<DO.Volunteer> volunteers;
                lock (AdminManager.BlMutex)
                {

                    calls = _dal.Call.ReadAll().ToList();
                    assignments = _dal.Assignment.ReadAll().ToList();
                    volunteers = _dal.Volunteer.ReadAll().ToList();
                }
                var riskRange = AdminManager.RiskRange;

                // קריאה אחרונה לפי CallId
                var latestCalls = calls
                    .GroupBy(call => call.Id)
                    .Select(group => group.OrderByDescending(c => c.OpenTime).First())
                    .ToList();

                // הכנה לרשימה לוגית BO
                var result = latestCalls.Select(call =>
                {
                    var callAssignments = assignments.Where(a => a.CallId == call.Id).ToList();
                    var lastAssignment = callAssignments.OrderByDescending(a => a.EntryTimeForTreatment).FirstOrDefault();
                    var status = CallManager.CalculateStatus(call);

                    return new BO.CallInList
                    {

                        Id = lastAssignment?.Id,
                        CallId = lastAssignment?.CallId ?? call.Id,
                        TypeOfCall = CallManager.ConvertToBOType(call.TypeOfCall),
                        OpenTime = call.OpenTime,
                        TimeLeft = (status != BO.STATUS.Closed && call.MaxTimeToFinish is DateTime max)
        ? (max - DateTime.Now > TimeSpan.Zero ? max - DateTime.Now : TimeSpan.Zero)
        : (TimeSpan?)null,
                        NameOfLastVolunteer = lastAssignment != null
        ? volunteers.FirstOrDefault(v => v.Id == lastAssignment.VolunteerId)?.FullName
        : null,
                        TimeTaken = (lastAssignment?.EndTimeOfTreatment.HasValue == true)
        ? lastAssignment.EndTimeOfTreatment.Value - call.OpenTime
        : null,
                        Status = status,
                        SumOfAssigned = callAssignments.Count
                    };


                }).ToList();

                // סינון אם נדרש
                if (filterBy != null && filterValue != null)
                {
                    if (filterBy is BO.STATUS && filterValue is BO.STATUS statusVal)
                        result = result.Where(r => r.Status == statusVal).ToList();
                    else if (filterBy is BO.TYPEOFCALL && filterValue is BO.TYPEOFCALL typeVal)
                        result = result.Where(r => r.TypeOfCall == typeVal).ToList();
                }

                // מיון אם נדרש
                result = sortBy switch
                {
                    BO.STATUS => result.OrderBy(r => r.Status).ToList(),
                    BO.TYPEOFCALL => result.OrderBy(r => r.TypeOfCall).ToList(),
                    _ => result.OrderBy(r => r.CallId).ToList()
                };

                return result;
            }
            catch (DalDoesNotExistException ex)
            {
                throw new BlDoesNotExistException("There are no readings.", ex);
            }
            catch (Exception ex)
            {
                throw new BlException("Error while getting the readings in the list", ex);
            }
        }

        /// <summary>
        /// Retrieves detailed information about a specific call by its ID.
        /// </summary>
        /// <param name="callId">The ID of the call to retrieve details for.</param>
        /// <returns>A detailed BO.Call object with assigned volunteers and related data.</returns>
        /// <exception cref="BlDoesNotExistException">Thrown when the call with the specified ID does not exist.</exception>

        public BO.Call GetCallDetails(int callId)
        {

            try
            {
                DO.Call? callDO;
                lock (AdminManager.BlMutex) //stage 7
                    callDO = _dal.Call.Read(callId);
                if (callDO == null)
                {
                    throw new BlDoesNotExistException($"Call with ID {callId} not found .");
                }
                List<BO.CallAssignInList> callAssignments;
                lock (AdminManager.BlMutex) //stage 7
                    callAssignments = _dal.Assignment.ReadAll()
                              .Where(assign => assign.CallId == callId)
                              .Select(assign => new BO.CallAssignInList
                              {
                                  VolunteerId = assign.VolunteerId,
                                  VolunteerName = _dal.Volunteer.Read(assign.VolunteerId)?.FullName,
                                  EntryTimeForTreatment = assign.EntryTimeForTreatment,
                                  EndTimeOfTreatment = assign.EndTimeOfTreatment,
                                  TypeOfTreatment = (BO.FINISHTYPE?)assign.TypeOfTreatment
                              }).ToList();


                return MappingProfile.ConvertToBO(callDO, callAssignments);
            }
            catch (DalDoesNotExistException dalDoesNotExistException)
            {
                throw new BlDoesNotExistException("There are no readings and/or risk time frame.", dalDoesNotExistException);
            }
            catch (Exception)
            {
                throw new BlException("Error while getting message details.");
            }
        }

        /// <summary>
        /// Adds a new call to the system after validating the data.
        /// </summary>
        /// <param name="call">The BO.Call object containing the details of the new call.</param>
        /// <exception cref="BlArgumentException">Thrown when invalid arguments (like empty address or invalid times) are provided.</exception>
        /// <exception cref="BlAlreadyExistException">Thrown when a call already exists in the system.</exception>
        /// <exception cref="BlInvalidOperationException">Thrown when an error occurs while adding the call.</exception>

        public void AddCall(BO.Call call)
        {
            AdminManager.ThrowOnSimulatorIsRunning(); //stage 7
            if (string.IsNullOrWhiteSpace(call.FullAddress))
                throw new BlArgumentException("Address cannot be empty.");


            call.Latitude = null;
            call.Longitude = null;
            if (call.MaxTimeToFinish is not null && call.MaxTimeToFinish <= call.OpenTime)
                throw new BlArgumentException("Maximum end time must be later than the call start time");

            DO.Call newCall = new DO.Call(
                Id: call.Id,
                TypeOfCall: (DO.TYPEOFCALL)call.TypeOfCall,
                VerbalDescription: call.VerbalDescription,
                FullAddress: call.FullAddress,
                Latitude: call.Latitude ?? 0,
                Longitude: call.Longitude ?? 0,
                OpenTime: AdminManager.Now,
                MaxTimeToFinish: call.MaxTimeToFinish
            );
            try
            {
                lock (AdminManager.BlMutex) //stage 7
                    _dal.Call.Create(newCall);
                CallManager.Observers.NotifyListUpdated();
            }
            catch (DalAlreadyExistException dalAlreadyExistException)
            {
                throw new BlAlreadyExistException("A call already exists.", dalAlreadyExistException);
            }
            catch (Exception ex)
            {
                throw new BlInvalidOperationException("Error adding the call to the system", ex);
            }
            try
            {
                _ = CallManager.UpdateCoordinatesAsync(call);
                if (!call.Latitude.HasValue || !call.Longitude.HasValue)
                    throw new BlArgumentException("Latitude and Longitude must be provided.");

                if (call.Latitude is < -90 or > 90)
                    throw new BlArgumentException("Latitude must be between -90 and 90.");
                if (call.Longitude is < -180 or > 180)
                    throw new BlArgumentException("Longitude must be between -180 and 180.");

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error updating coordinates for call {call.Id}: {ex.Message}");
            }
        }


        /// <summary>
        /// Cancels the treatment of an assigned volunteer by updating the assignment status.
        /// </summary>
        /// <param name="volId">The volunteer ID requesting to cancel the treatment.</param>
        /// <param name="assiId">The assignment ID to be canceled.</param>
        /// <exception cref="BlDoesNotExistException">Thrown when the assignment or volunteer cannot be found.</exception>
        /// <exception cref="BlUnauthorizedException">Thrown when the volunteer doesn't have permission to cancel the treatment.</exception>
        /// <exception cref="BlInvalidOperationException">Thrown when it's not possible to cancel the treatment (e.g., already completed or expired).</exception>

        public void CancelTreat(int volId, int assiId)
        {
            AdminManager.ThrowOnSimulatorIsRunning(); //stage 7
            try
            {
                DO.Assignment? assignment;
                lock (AdminManager.BlMutex) //stage 7
                    assignment = _dal.Assignment.Read(assiId);
                if (assignment == null)
                    throw new BlDoesNotExistException($"Assignment with ID {assiId} .");

                //  שליפת המתנדב שמבקש לבטל
                DO.Volunteer? volunteerFromDAL;
                lock (AdminManager.BlMutex) //stage 7
                    volunteerFromDAL = _dal.Volunteer.Read(volId);
                if (volunteerFromDAL == null)
                    throw new BlDoesNotExistException($"The user with ID {volId} not found.");

                //  בדיקת הרשאה - רק מנהל או המתנדב עצמו רשאים לבטל
                if (volunteerFromDAL.Role != DO.ROLE.ADMIN && volunteerFromDAL.Id != assignment.VolunteerId)
                    throw new BlUnauthorizedException("You do not have permission to cancel the treatment.");

                //  בדיקה שההקצאה עדיין פתוחה
                DO.Call? call;
                lock (AdminManager.BlMutex) //stage 7
                    call = _dal.Call.Read(assignment.CallId);
                if (call == null)
                    throw new BlDoesNotExistException($"Assignment with ID {assignment.CallId} not found.");

                if (assignment.EndTimeOfTreatment != null)
                    throw new BlInvalidOperationException("It is not possible to cancel an assignment that has already been completed..");

                if (call.MaxTimeToFinish != null && call.MaxTimeToFinish < DateTime.Now)
                    throw new BlInvalidOperationException("Treatment cannot be canceled after the call has expired.");

                //  עדכון הנתונים
                var updatedAssignment = assignment with
                {
                    EndTimeOfTreatment = AdminManager.Now,
                    TypeOfTreatment = volunteerFromDAL.Id == assignment.VolunteerId
                                      ? TYPEOFTREATMENT.SELFCANCELLATION
                                      : TYPEOFTREATMENT.CANCELINGANADMINISTRATOR
                };

                //  שמירה לשכבת הנתונים
                lock (AdminManager.BlMutex) //stage 7
                    _dal.Assignment.Update(updatedAssignment);
            }

            catch (DalDoesNotExistException dalDoesNotExistException)
            {
                throw new BlDoesNotExistException($"No Assignment with ID exists.{assiId}", dalDoesNotExistException);
            }

            catch (Exception ex)
            {
                throw new BlInvalidOperationException("Error while canceling treatment", ex);
            }

        }

        /// <summary>
        /// Validates if the volunteer can take the selected call and assigns the volunteer to it.
        /// </summary>
        /// <param name="volId">The ID of the volunteer who is being assigned to the call.</param>
        /// <param name="callId">The ID of the call to be assigned to the volunteer.</param>
        public void ChooseCall(int volId, int callId)
        {
            AdminManager.ThrowOnSimulatorIsRunning(); //stage 7
            try
            {
                DO.Call? call;
                lock (AdminManager.BlMutex) //stage 7
                    call = _dal.Call.Read(callId);
                if (call == null)
                    throw new BlDoesNotExistException($"Call with ID {callId} not found.");
                // 2. בדיקה שהקריאה לא טופלה ושאין הקצאה פתוחה
                IEnumerable<DO.Assignment> existingAssignments;
                lock (AdminManager.BlMutex) //stage 7
                    existingAssignments = _dal.Assignment.ReadAll(a => a.CallId == callId);

                if (existingAssignments.Any(a => a.EndTimeOfTreatment == null))
                    throw new BlInvalidOperationException("You cannot select a call that is already in progress.");

                // 3. בדיקה שלא פג תוקפה של הקריאה
                if (call.MaxTimeToFinish != null && call.MaxTimeToFinish < AdminManager.Now)
                    throw new BlInvalidOperationException("Cannot select an expired call.");

                // 4. שליפת המתנדב מה-DAL
                DO.Volunteer? volunteer;
                lock (AdminManager.BlMutex) //stage 7
                    volunteer = _dal.Volunteer.Read(volId);
                if (volunteer == null)
                    throw new BlDoesNotExistException($"The user with ID {volId} not found.");

                // 5. יצירת הקצאה חדשה
                var newAssignment = new DO.Assignment(
                    Id: 0,
                    CallId: callId,
                    VolunteerId: volId,
                    EntryTimeForTreatment: AdminManager.Now,
                    EndTimeOfTreatment: null,
                    TypeOfTreatment: null
                );
                // 6. שמירת ההקצאה החדשה
                lock (AdminManager.BlMutex) //stage 7
                    _dal.Assignment.Create(newAssignment);
                System.Diagnostics.Debug.WriteLine(newAssignment);
                CallManager.Observers.NotifyListUpdated();

            }
            catch (DalDoesNotExistException dalDoesNotExistException)
            {
                // תרגום חריגת DAL לחריגה של BL
                throw new BlDoesNotExistException("One of the requested items does not exist.", dalDoesNotExistException);
            }
            catch (DalAlreadyExistException dalAlreadyExistException)
            {
                // תרגום חריגת DAL לחריגה של BL
                throw new BlAlreadyExistException($" An assignment with a call ID exists {callId}.", dalAlreadyExistException);
            }

            catch (Exception ex)
            {
                throw new BlInvalidOperationException("Error when selecting a call for treatment.", ex);
            }
        }
        /// <summary>
        /// Deletes a call from the system if it is open and has no existing assignments.
        /// </summary>
        /// <param name="callId">The ID of the call to be deleted.</param>

        public void DeleteCall(int callId)
        {
            AdminManager.ThrowOnSimulatorIsRunning(); //stage 7
            try
            {
                //  שליפת הקריאה מהמאגר
                var call = GetCallDetails(callId);
                if (call == null)
                    throw new BlDoesNotExistException($"Call with ID {callId} not found.");
                if (call.Status != STATUS.Open)
                    throw new BlInvalidOperationException("A call that is not in open status cannot be deleted.");

                //בדיקה שלא קיימות הקצאות לקריאה זו
                IEnumerable<Assignment> assignments;
                lock (AdminManager.BlMutex) //stage 7
                    assignments = _dal.Assignment.ReadAll().Where(a => a.CallId == callId);
                if (assignments.Any())
                    throw new BlInvalidOperationException("A call previously assigned to volunteers cannot be deleted.");

                lock (AdminManager.BlMutex) //stage 7
                    _dal.Call.Delete(callId);

                CallManager.Observers.NotifyListUpdated();
            }
            catch (DalDoesNotExistException dalDoesNotExistException)
            {
                // תרגום חריגת DAL לחריגה של BL
                throw new BlDoesNotExistException($" No call with ID exists {callId}.", dalDoesNotExistException);
            }

            catch (Exception ex)
            {
                throw new BlInvalidOperationException("Error deleting call", ex);
            }
        }

        /// <summary>
        /// Returns the count of calls based on their status.
        /// </summary>

        public Dictionary<BO.STATUS, int> GetCallCountsByStatus()
        {
            var calls = GetCallList(null, null, null);

            // מקבל ספירה של כל סטטוס שקיים ברשימה
            var counts = calls
                .GroupBy(call => call.Status)
                .ToDictionary(g => g.Key, g => g.Count());

            // לוודא שכל סטטוס שמעניין אותנו קיים במילון, גם אם הערך 0
            foreach (BO.STATUS status in Enum.GetValues(typeof(BO.STATUS)))
            {
                if (!counts.ContainsKey(status))
                {
                    counts[status] = 0;
                }
            }

            return counts;
        }



        /// <summary>
        /// Returns the list of closed calls handled by a volunteer, with options to filter and sort by call type.
        /// </summary>
        /// <param name="volId">The ID of the volunteer for whom the closed calls are being retrieved.</param>
        /// <param name="tOfCall">The type of call to filter by (optional).</param>
        /// <param name="sortBy">The field to sort the results by (optional).</param>

        public IEnumerable<ClosedCallInList> GetClosedCallInList(int volId, BO.TYPEOFCALL? tOfCall, BO.ClosedCallInListField? sortBy)
        {
            try
            {
                var riskRange = AdminManager.RiskRange;

                IEnumerable<BO.ClosedCallInList?> closedCalls;
                lock (AdminManager.BlMutex) //stage 7
                    closedCalls = _dal.Assignment.ReadAll()
                    .Where(a => a.VolunteerId == volId && a.EndTimeOfTreatment != null) // רק קריאות שטופלו
                    .Select(a =>
                    {
                        var call = GetCallDetails(a.CallId); // שליפת הקריאה המתאימה
                        if (call == null) return null;

                        // המרת DO.Call ל-BO.ClosedCallInList
                        return new BO.ClosedCallInList
                        {
                            Id = call.Id,
                            TypeOfCall = (BO.TYPEOFCALL)(int)call.TypeOfCall,
                            FullAddress = call.FullAddress,
                            OpenTime = call.OpenTime,
                            EntryTimeForTreatment = a.EntryTimeForTreatment,
                            EndTimeOfTreatment = a.EndTimeOfTreatment,
                            TypeOfTreatment = CallManager.ConvertToBOFinishType(a.TypeOfTreatment)
                        };
                    })
                    .Where(c => c != null) // סינון קריאות ריקות
                    .ToList();
                if (closedCalls == null)
                    throw new BlDoesNotExistException($"No closed calls found for volunteer with ID {volId}.");
                // סינון לפי סוג קריאה אם נדרש
                if (tOfCall.HasValue)
                {
                    closedCalls = closedCalls
                        .Where(c => c.TypeOfCall == tOfCall.Value)
                        .ToList();
                }

                // מיון לפי השדה שנבחר, ברירת מחדל לפי מזהה קריאה
                closedCalls = sortBy switch
                {
                    BO.ClosedCallInListField.OpenTime => closedCalls.OrderBy(c => c.OpenTime).ToList(),
                    BO.ClosedCallInListField.EntryTimeForTreatment => closedCalls.OrderBy(c => c.EntryTimeForTreatment).ToList(),
                    BO.ClosedCallInListField.EndTimeOfTreatment => closedCalls.OrderBy(c => c.EndTimeOfTreatment).ToList(),
                    _ => closedCalls.OrderBy(c => c.Id).ToList()
                };

                return closedCalls!;
            }
            catch (DalDoesNotExistException dalEx)
            {
                throw new BlDoesNotExistException("There are no allocations.", dalEx);
            }
            catch (Exception ex)
            {
                throw new BlInvalidOperationException("Error fetching closed calls.", ex);
            }
        }


        /// <summary>
        /// Returns a list of open calls for a volunteer, with options to filter and sort by call type or other parameters.
        /// </summary>
        /// <param name="volId">The ID of the volunteer for whom the open calls are being retrieved.</param>
        /// <param name="tOfCall">The type of call to filter by (optional).</param>
        /// <param name="sortBy">The field to sort the results by (optional).</param>


        public IEnumerable<OpenCallInList> GetOpenCallInList(int volId, BO.TYPEOFCALL? tOfCall, BO.OpenCallInListField? sortBy)
        {
            try
            {
                var riskRange = AdminManager.RiskRange;
                DO.Volunteer? volunteer;
                lock (AdminManager.BlMutex) //stage 7
                    volunteer = _dal.Volunteer.Read(volId)
                    ?? throw new BlDoesNotExistException($"Volunteer with ID {volId} not found.");

                IEnumerable<DO.Call> openCalls;
                lock (AdminManager.BlMutex) //stage 7
                    openCalls = _dal.Call.ReadAll()
                    .Where(call =>
                    {
                        var status = CallManager.CalculateStatus(call);
                        return status == BO.STATUS.Open || status == BO.STATUS.OpenDangerZone;
                    })
                    .Where(call => !tOfCall.HasValue || CallManager.ConvertToBOType(call.TypeOfCall) == tOfCall.Value);

                var openCallsList =
                    from call in openCalls
                    select new BO.OpenCallInList
                    {
                        Id = call.Id,
                        TypeOfCall = CallManager.ConvertToBOType(call.TypeOfCall),
                        VerbalDescription = call.VerbalDescription,
                        FullAddress = call.FullAddress,
                        OpenTime = call.OpenTime,
                        MaxTimeToFinish = call.MaxTimeToFinish,
                        Distance = CallManager.GetDistance(volunteer, call)
                    };
                openCallsList = openCallsList.Where(call => call.Distance <= volunteer.MaxDistance);
                openCallsList = sortBy.HasValue
                    ? sortBy.Value switch
                    {
                        BO.OpenCallInListField.TypeOfCall => openCallsList.OrderBy(c => c.TypeOfCall),
                        BO.OpenCallInListField.OpenTime => openCallsList.OrderBy(c => c.OpenTime),
                        BO.OpenCallInListField.Distance => openCallsList.OrderBy(c => c.Distance),
                        _ => openCallsList.OrderBy(c => c.Id)
                    }
                    : openCallsList.OrderBy(c => c.Id);

                return openCallsList;
            }
            catch (DalDoesNotExistException dalEx)
            {
                throw new BlDoesNotExistException("One of the requested items does not exist.", dalEx);
            }
            catch (Exception ex)
            {
                throw new BlInvalidOperationException("Error retrieving open calls.", ex);
            }
        }

        /// <summary>
        /// Updates the details of an existing call, ensuring data validity and saving changes to the system.
        /// </summary>
        /// <param name="call">The BO.Call object containing the updated call details.</param>

        public void UpdateCallDetails(BO.Call call)
        {
            AdminManager.ThrowOnSimulatorIsRunning(); //stage 7
            try
            {
                // 1️⃣ בדיקות תקינות נתונים
                if (call == null)
                    throw new BlException("The reading cannot be null");

                if (call.MaxTimeToFinish <= call.OpenTime)
                    throw new BlException("Maximum end time must be after the start time");

                if (string.IsNullOrWhiteSpace(call.FullAddress))
                    throw new ArgumentException("Invalid or empty address");

                // 3️⃣ חיפוש הקריאה בשכבת הנתונים
                DO.Call? existingCall;
                lock (AdminManager.BlMutex) //stage 7
                    existingCall = _dal.Call.Read(call.Id);
                if (existingCall == null)
                    throw new BlException($"No call found with ID {call.Id}");

                // 4️⃣ המרת אובייקט BO לקריאה ב-DO
                var riskRange = _dal.Config.RiskRange;

                var updatedCall = MappingProfile.ConvertToDO(call);
                lock (AdminManager.BlMutex) //stage 7
                    _dal.Call.Update(updatedCall);
                CallManager.Observers.NotifyItemUpdated(updatedCall.Id);  //stage 5
                CallManager.Observers.NotifyListUpdated();  //stage 5

            }
            catch (Exception ex)
            {
                throw new BlException("Error while updating reading", ex);
            }
            try
            {
                _ = CallManager.UpdateCoordinatesAsync(call);
                if (!call.Latitude.HasValue || !call.Longitude.HasValue)
                    throw new BlArgumentException("Latitude and Longitude must be provided.");

                if (call.Latitude is < -90 or > 90)
                    throw new BlArgumentException("Latitude must be between -90 and 90.");
                if (call.Longitude is < -180 or > 180)
                    throw new BlArgumentException("Longitude must be between -180 and 180.");

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error updating coordinates for call {call.Id}: {ex.Message}");
            }
        }


        /// <summary>
        /// Updates the status of a volunteer's call treatment to "finished", ensuring proper assignment and status checks.
        /// </summary>
        /// <param name="volId">The ID of the volunteer who is finishing the treatment.</param>
        /// <param name="callId">The ID of the call being treated.</param>

        public void UpdateFinishTreat(int volId, int callId)
        {
            AdminManager.ThrowOnSimulatorIsRunning(); // שלב 7

            try
            {
                // שלב 1: שליפת ההקצאה המתאימה
                DO.Assignment? assignment;
                lock (AdminManager.BlMutex)
                {
                    assignment = _dal.Assignment
                        .ReadAll()
                        .FirstOrDefault(a => a.CallId == callId && a.VolunteerId == volId)
                        ?? throw new BO.BlDoesNotExistException("No assignment found for this volunteer and call.");
                }

                // שלב 2: וידוא שהמתנדב מתאים
                if (assignment.VolunteerId != volId)
                    throw new BO.BlUnauthorizedException("You do not have permission to end processing on this call.");

                // שלב 3: בדיקה אם כבר הסתיים
                if (assignment.EndTimeOfTreatment != null)
                    throw new BO.BlInvalidOperationException("Cannot finish handling a call that has already been closed.");

                // שלב 4: שליפת הקריאה הרלוונטית
                var call = _dal.Call.Read(callId)
                    ?? throw new BO.BlDoesNotExistException("Call not found for status calculation.");

                var now = AdminManager.Now;

                // שלב 5: קביעת סוג הטיפול לפי הזמן
                var typeOfTreatment = now <= call.MaxTimeToFinish
                    ? TYPEOFTREATMENT.TREATE
                    : TYPEOFTREATMENT.CANCELLATIONHASEXPIRED;

                // שלב 6: עדכון ההקצאה
                var updatedAssignment = assignment with
                {
                    EndTimeOfTreatment = now,
                    TypeOfTreatment = typeOfTreatment
                };

                lock (AdminManager.BlMutex)
                {
                    _dal.Assignment.Update(updatedAssignment);
                }
            }
            catch (DalDoesNotExistException dalDoesNotExistException)
            {
                throw new BlDoesNotExistException("One of the requested items does not exist.", dalDoesNotExistException);
            }
            catch (Exception ex)
            {
                throw new BlException("Error when finishing treatment: " + ex.Message, ex);
            }
        }


        /// <summary>
        /// מוסיף מתבונן כללי לרשימת המתבוננים במערכת.
        /// </summary>
        /// <param name="listObserver">פעולה שתופעל כאשר יתרחש עדכון כללי.</param>
        public void AddObserver(Action listObserver)
        {
            CallManager.Observers.AddListObserver(listObserver);
        }

        /// <summary>
        /// מוסיף מתבונן ספציפי לקריאה מסוימת לפי מזהה.
        /// </summary>
        /// <param name="id">מזהה הקריאה (Call ID) שאליה מתבונן זה קשור.</param>
        /// <param name="observer">הפעולה שתופעל כאשר יתרחש עדכון בקריאה זו.</param>
        public void AddObserver(int id, Action observer)
        {
            CallManager.Observers.AddObserver(id, observer);
        }

        /// <summary>
        /// מסיר מתבונן כללי מהרשימה.
        /// </summary>
        /// <param name="listObserver">המתבונן שיש להסיר.</param>
        public void RemoveObserver(Action listObserver)
        {
            CallManager.Observers.RemoveListObserver(listObserver);
        }

        /// <summary>
        /// מסיר מתבונן ספציפי מקריאה מסוימת לפי מזהה.
        /// </summary>
        /// <param name="id">מזהה הקריאה (Call ID) שאליה המתבונן קשור.</param>
        /// <param name="observer">המתבונן שיש להסיר.</param>
        public void RemoveObserver(int id, Action observer)
        {
            CallManager.Observers.RemoveObserver(id, observer);
        }
    }
}

