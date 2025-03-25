
using BlApi;
using System.Collections;
using DalApi;
using BO;
using Helpers;
using DO;

namespace BlImplementation
{
    internal class VolunteerImplementation : IVolunteer
    {
        private readonly DalApi.IDal _dal = DalApi.Factory.Get;

        // done  needs to ask about the new function
        public void AddVolunteer(BO.Volunteer boVolunteer)
        {
            if (boVolunteer == null) throw new ArgumentNullException("No volunteer entered.");
            Tools.ValidateVolunteerFormat(boVolunteer);
            Tools.ValidateVolunteerLogic(boVolunteer);



            (double latitude, double longitude) = VolunteerManager.GetCoordinatesFromAddress(boVolunteer.FullAddress);

            boVolunteer.Latitude = latitude;
            boVolunteer.Longitude = longitude;



            // יצירת אובייקט נתונים
            DO.Volunteer doVolunteer = new DO.Volunteer(
                boVolunteer.Id,
                // לא משתנה
                boVolunteer.FullName,
                boVolunteer.Phone,
                boVolunteer.Email,
                boVolunteer.FullAddress,
                boVolunteer.Password,
               latitude,
              longitude,
             (DO.ROLE)boVolunteer.Role,
                boVolunteer.Active,
                boVolunteer.MaxDistance,
               (DO.TYPEOFDISTSANCE)boVolunteer.TypeOfDistance
            );

            try
            {
                _dal.Volunteer.Create(doVolunteer);
            }
            catch (DO.DalDoesNotExistException ex)
            {
                throw new BO.BlDoesNotExistException($"Volunteer with ID={boVolunteer.Id} does not exist", ex);
            }





        }




        //done
        public BO.ROLE GetUserRole(string userName)
        {
            var volunteer = _dal.Volunteer.ReadAll();
            var user = volunteer.FirstOrDefault(v => v.FullName == userName);
            if (user == null)
            {
                throw new BlDoesNotExistException("המשתמש לא קיים.");
            }

            return VolunteerManager.ConvertToBOType(user.Role);
        }

      

        public IEnumerable<VolunteerInList> GetVolunteerInList(bool? active, BO.ROLE? ROLE)
        {
            throw new NotImplementedException();
        }

        //almost done there are errors...admin...
        public void UpdateVolunteerDetails(int id, BO.Volunteer boVolunteer)
        {
            //שליפת מתנדב
            var vol = _dal.Volunteer.Read(boVolunteer.Id) ??
            throw new BO.BlDoesNotExistException($"The Volunteer with ID={boVolunteer.Id} does not exist");

            // בדיקה אם המבקש הוא מנהל או שהוא עצמו
            var requester = _dal.Volunteer.Read(id) ??
            throw new BO.BlDoesNotExistException($"Requester with ID={id} does not exist");
            if (requester.Role != (DO.ROLE)BO.ROLE.Admin && requester.Id != boVolunteer.Id)
            {
                throw new BO.BlUnauthorizedException("Only an admin or the volunteer themselves can update the details.");
            }

            Tools.ValidateVolunteerFormat(boVolunteer);
            Tools.ValidateVolunteerLogic(boVolunteer);



            (double latitude, double longitude) = VolunteerManager.GetCoordinatesFromAddress(boVolunteer.FullAddress);

            boVolunteer.Latitude = latitude;
            boVolunteer.Longitude = longitude;

            // בדיקה אילו שדות השתנו ואילו מותרים לשינוי
            if (requester.Role.ToString() != BO.ROLE.Admin.ToString() && vol.Role.ToString() != boVolunteer.Role.ToString())
            {
                throw new BO.BlUnauthorizedException("Only an admin can update the volunteer role.");
            }

            // יצירת אובייקט נתונים
            DO.Volunteer doVolunteer = new DO.Volunteer(
                boVolunteer.Id,
                // לא משתנה
                boVolunteer.FullName,
                boVolunteer.Phone,
                boVolunteer.Email,
                boVolunteer.FullAddress,
                boVolunteer.Password,
               latitude,
              longitude,
             (DO.ROLE)boVolunteer.Role,
                boVolunteer.Active,
                boVolunteer.MaxDistance,
               (DO.TYPEOFDISTSANCE)boVolunteer.TypeOfDistance
            );

            try
            {
                _dal.Volunteer.Update(doVolunteer);
            }
            catch (DO.DalDoesNotExistException ex)
            {
                throw new BO.BlDoesNotExistException($"Volunteer with ID={boVolunteer.Id} does not exist", ex);
            }



        }
        //I think it is done
        public void DeleteVolunteerDetails(int id)
        {
            var vol = _dal.Volunteer.Read(id) ??
            throw new BO.BlDoesNotExistException($"The Volunteer with ID={id} does not exist");

            var assignments = _dal.Assignment.ReadAll(a => a.Id == id);
            // Check if the volunteer is currently handling a call or has ever handled one
            if (vol.Active==true)
            {
                // If the volunteer is currently handling a call, throw an exception
                throw new BO.BlVolunteerInProgressException($"Volunteer with ID={id} is currently handling a call and cannot be deleted.");
            }
            // Check if the volunteer has ever handled any assignment
            if (assignments.Any())
            {
                // If the volunteer has handled any assignment, throw an exception
                throw new BO.BlVolunteerInProgressException($"Volunteer with ID={id} has handled assignments and cannot be deleted.");
            }
            // If no active call is found, proceed with the deletion
            try
            {
                _dal.Volunteer.Delete(id);
            }
            catch (DO.DalDoesNotExistException ex)
            {
                // If the deletion fails due to a missing volunteer in the data layer, throw a custom exception
                throw new BO.BlDoesNotExistException($"Volunteer with ID={id} does not exist", ex);
            }

        }

        public BO.Volunteer GetVolunteerDetails(int id)
        {
            //שליפת מתנדב
            var vol = _dal.Volunteer.Read(id) ??
            throw new BO.BlDoesNotExistException($"The Volunteer with ID={id} does not exist");

        }
        //public BO.Volunteer? Read(int id)
        //{
        //    var doVolunteer = _dal.Volunteer.Read(id) ??
        //        throw new BO.BlDoesNotExistException($"Volunteer with ID={id} does not exist");
        //    var treatedAssignments = from a in _dal.Assignment.ReadAll()
        //                             where a.MyFinishType.ToString() == BO.FinishType.Treated.ToString()
        //                             group a by a.VolId into g
        //                             select new { VolId = g.Key, Count = g.Count() };

        //    var canceledAssignments = from a in _dal.Assignment.ReadAll()
        //                              where a.MyFinishType.ToString() == BO.FinishType.SelfCanclled.ToString()
        //                              group a by a.VolId into g
        //                              select new { VolId = g.Key, Count = g.Count() };

        //    var expiredAssignments = from a in _dal.Assignment.ReadAll()
        //                             where a.MyFinishType.ToString() == BO.FinishType.Expired.ToString()
        //                             group a by a.VolId into g
        //                             select new { VolId = g.Key, Count = g.Count() };

        //    // המרה למילון
        //    var treatedDict = treatedAssignments.ToDictionary(x => x.VolId, x => x.Count);
        //    var canceledDict = canceledAssignments.ToDictionary(x => x.VolId, x => x.Count);
        //    var expiredDict = expiredAssignments.ToDictionary(x => x.VolId, x => x.Count);

        //    BO.CallInProgress? callInProgress = VolunteerManager.CheckCallInProgress(id);


        //    return new BO.Volunteer
        //    {
        //        Id = id,
        //        FullName = doVolunteer.FullName, // התאם את השם
        //        Phone = doVolunteer.Phone,   // אם קיים ב-DataObject
        //        Email = doVolunteer.Email,   // אם קיים ב-DataObject
        //        Address = doVolunteer.CrntAdress, // אם קיים ב-DataObject
        //        Latitutde = doVolunteer.Latitude, // תוודא שהשם נכון (ייתכן שיש שגיאת כתיב)
        //        Longitude = doVolunteer.Longitude, // אם קיים ב-DataObject
        //        MyRole = (BO.Role)doVolunteer.MyRole, // תוודא ששדה זה קיים ב-DataObject
        //        Active = doVolunteer.Active, // התאם את השם
        //        MaxDistance = doVolunteer.MaxDistance, // אם קיים ב-DataObject
        //        MyDisType = (BO.DisType)doVolunteer.MyDisType, // אם קיים ב-DataObject
        //        SumTreated = treatedDict.ContainsKey(id) ? treatedDict[id] : 0,
        //        SumCanceled = canceledDict.ContainsKey(id) ? canceledDict[id] : 0,
        //        SumExpired = expiredDict.ContainsKey(id) ? expiredDict[id] : 0,
        //        MyCallInProgress = callInProgress ?? null

        //    };
        //}

    }
}

