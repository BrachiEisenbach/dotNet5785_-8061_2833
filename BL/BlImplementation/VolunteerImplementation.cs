using BlApi;
using BO;
using DalApi;
using DO;
using Helpers;
using System.Collections;
using System.Diagnostics;

namespace BlImplementation
{
    internal class VolunteerImplementation : BlApi.IVolunteer
    {
        private readonly DalApi.IDal _dal = DalApi.Factory.Get;

        /// <summary>
        /// Returns the role of a volunteer based on their username.
        /// </summary>
        /// <param name="userName">The full name of the volunteer.</param>
        /// <returns>The role of the volunteer.</returns>
        public BO.ROLE GetUserRole(string userName, string password)
        {
            try
            {
                IEnumerable<DO.Volunteer> volunteers;
                lock (AdminManager.BlMutex) //stage 7
                    volunteers = _dal.Volunteer.ReadAll();
                var user = volunteers.FirstOrDefault(v => v.FullName == userName && v.Password == password);

                if (user == null)
                {
                    throw new BlDoesNotExistException("The user does not exist or password is incorrect.");
                }

                return VolunteerManager.ConvertToBORole(user.Role);
            }
            catch (DalDoesNotExistException dalDoesNotExistException)
            {
                throw new BlDoesNotExistException("The user does not exist.", dalDoesNotExistException);
            }
            catch (Exception ex)
            {
                throw new BlException("Error while getting user role.");
            }
        }

        /// <summary>
        /// Returns the role of a volunteer based on their id.
        /// </summary>
        /// <param id="id">The id of the volunteer.</param>
        /// <returns>The role of the volunteer.</returns>
        public BO.ROLE GetUserRoleById(int id)
        {
            try
            {
                IEnumerable<DO.Volunteer> volunteers;
                lock (AdminManager.BlMutex) //stage 7
                    volunteers = _dal.Volunteer.ReadAll().ToList();
                var user = volunteers.FirstOrDefault(v => v.Id == id);
                if (user == null)
                {
                    throw new BlDoesNotExistException("The user isn't exist.");
                }

                return VolunteerManager.ConvertToBORole(user.Role);
            }
            catch (DalDoesNotExistException dalDoesNotExistException)
            {
                throw new BlDoesNotExistException("The user isn't exist.", dalDoesNotExistException);
            }
            catch (Exception ex)
            {
                throw new BlException("Error while getting message details.");
            }
        }

        /// <summary>
        /// Updates the details of an existing volunteer.
        /// </summary>
        /// <param name="id">The ID of the requester (admin or volunteer themselves).</param>
        /// <param name="boVolunteer">The volunteer object containing updated details.</param>
        public void UpdateVolunteerDetails(int id, BO.Volunteer boVolunteer)
        {
            AdminManager.ThrowOnSimulatorIsRunning(); //stage 7

            DO.Volunteer? vol;
            lock (AdminManager.BlMutex) //stage 7
                vol = _dal.Volunteer.Read(boVolunteer.Id) ??
            throw new BO.BlDoesNotExistException($"The Volunteer with ID={boVolunteer.Id} does not exist");

            DO.Volunteer? requester;
            lock (AdminManager.BlMutex) //stage 7
                requester = _dal.Volunteer.Read(id) ??
            throw new BO.BlDoesNotExistException($"Requester with ID={id} does not exist");
            if (requester.Role != (DO.ROLE)BO.ROLE.ADMIN && requester.Id != boVolunteer.Id)
            {
                throw new BO.BlUnauthorizedException("Only an admin or the volunteer themselves can update the details.");
            }

            if (!boVolunteer.Active && boVolunteer.CallInTreate != null)
            {
                throw new BO.BlUnauthorizedException("Cannot deactivate a volunteer who is currently assigned to a call.");
            }

            Tools.ValidateVolunteerFormat(boVolunteer);
            Tools.ValidateVolunteerLogic(boVolunteer);


            boVolunteer.Latitude = null;
            boVolunteer.Longitude = null;

            if (requester.Role.ToString() != BO.ROLE.ADMIN.ToString() && vol.Role.ToString() != boVolunteer.Role.ToString())
            {
                throw new BO.BlUnauthorizedException("Only an admin can update the volunteer role.");
            }

            DO.Volunteer doVolunteer = new DO.Volunteer(
                boVolunteer.Id,
                boVolunteer.FullName,
                boVolunteer.Phone,
                boVolunteer.Email,
                boVolunteer.Password,
                boVolunteer.FullAddress,
                boVolunteer.Latitude,
                boVolunteer.Longitude,
                (DO.ROLE)boVolunteer.Role,
                boVolunteer.Active,
                boVolunteer.MaxDistance,
                (DO.TYPEOFDISTANCE)boVolunteer.TypeOfDistance
            );

            try
            {
                lock (AdminManager.BlMutex) //stage 7
                    _dal.Volunteer.Update(doVolunteer);
                VolunteerManager.Observers.NotifyItemUpdated(doVolunteer.Id);
                VolunteerManager.Observers.NotifyListUpdated();
            }
            catch (DO.DalDoesNotExistException ex)
            {
                throw new BO.BlDoesNotExistException($"Volunteer with ID={boVolunteer.Id} does not exist", ex);
            }
            catch (Exception ex)
            {
                throw new BlException("Error while updating volunteer", ex);
            }
            try
            {
                _ = VolunteerManager.UpdateVolunteerCoordinatesAsync(boVolunteer);
                if (!boVolunteer.Latitude.HasValue || !boVolunteer.Longitude.HasValue)
                    throw new BlArgumentException("Latitude and Longitude must be provided.");

                if (boVolunteer.Latitude is < -90 or > 90)
                    throw new BlArgumentException("Latitude must be between -90 and 90.");
                if (boVolunteer.Longitude is < -180 or > 180)
                    throw new BlArgumentException("Longitude must be between -180 and 180.");

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error updating coordinates for call {boVolunteer.Id}: {ex.Message}");
            }

        }

        /// <summary>
        /// Deletes a volunteer's details if they are not currently handling an active call or assignment.
        /// </summary>
        /// <param name="id">The ID of the volunteer to delete.</param>
        public void DeleteVolunteerDetails(int id)
        {
            AdminManager.ThrowOnSimulatorIsRunning(); //stage 7

            DO.Volunteer? vol;
            lock (AdminManager.BlMutex) //stage 7
                vol = _dal.Volunteer.Read(id) ??
            throw new BO.BlDoesNotExistException($"The Volunteer with ID={id} does not exist");

            IEnumerable<Assignment?> assignments;
            lock (AdminManager.BlMutex) //stage 7
                assignments = _dal.Assignment.ReadAll(a => a.Id == id);
            if (vol.Active == true)
            {
                System.Diagnostics.Debug.WriteLine("נזרקת החריגה מסוג BlVolunteerInProgressException");
                throw new BlVolunteerInProgressException($"Volunteer with ID={id} is currently handling a call and cannot be deleted.");
            }
            // בדיקה אם יש משימות פתוחות לולנטר
            foreach (var a in assignments)
            {
                System.Diagnostics.Debug.WriteLine($"Assignment found: Id={a.Id}, VolunteerId={a.VolunteerId}");
            }

            if (assignments.Any())
            {
                throw new BO.BlVolunteerInProgressException($"Volunteer with ID={id} has handled assignments and cannot be deleted.");
            }

            try
            {
                lock (AdminManager.BlMutex) //stage 7
                    _dal.Volunteer.Delete(id);
                VolunteerManager.Observers.NotifyListUpdated();
            }
            catch (DO.DalDoesNotExistException ex)
            {
                throw new BO.BlDoesNotExistException($"Volunteer with ID={id} does not exist", ex);
            }
            catch (Exception ex)
            {
                throw new BO.BlException("Error while deleting volunteer.", ex);
            }
        }

        /// <summary>
        /// Adds a new volunteer to the system.
        /// </summary>
        /// <param name="boVolunteer">The volunteer object to add.</param>
        public void AddVolunteer(BO.Volunteer boVolunteer)
        {
            AdminManager.ThrowOnSimulatorIsRunning(); //stage 7

            if (boVolunteer == null)
                throw new ArgumentNullException("No volunteer entered.");
            Tools.ValidateVolunteerFormat(boVolunteer);
            Tools.ValidateVolunteerLogic(boVolunteer);


            boVolunteer.Latitude = null;
            boVolunteer.Longitude = null;

            DO.Volunteer doVolunteer = new DO.Volunteer(
                boVolunteer.Id,
                boVolunteer.FullName,
                boVolunteer.Phone,
                boVolunteer.Email,
                boVolunteer.Password,
                boVolunteer.FullAddress,
                boVolunteer.Latitude ?? 0,
                boVolunteer.Longitude = 0,
                (DO.ROLE)boVolunteer.Role,
                boVolunteer.Active,
                boVolunteer.MaxDistance,
                (DO.TYPEOFDISTANCE)boVolunteer.TypeOfDistance
            );

            try
            {
                lock (AdminManager.BlMutex) //stage 7
                    _dal.Volunteer.Create(doVolunteer);
                VolunteerManager.Observers.NotifyListUpdated();
            }
            catch (DO.DalDoesNotExistException ex)
            {
                throw new BO.BlDoesNotExistException($"Volunteer with ID={boVolunteer.Id} does not exist", ex);
            }
            catch (Exception ex)
            {
                throw new BlException("Error while adding volunteer", ex);
            }
            try
            {
                _ = VolunteerManager.UpdateVolunteerCoordinatesAsync(boVolunteer);
                if (!boVolunteer.Latitude.HasValue || !boVolunteer.Longitude.HasValue)
                    throw new BlArgumentException("Latitude and Longitude must be provided.");

                if (boVolunteer.Latitude is < -90 or > 90)
                    throw new BlArgumentException("Latitude must be between -90 and 90.");
                if (boVolunteer.Longitude is < -180 or > 180)
                    throw new BlArgumentException("Longitude must be between -180 and 180.");

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error updating coordinates for call {boVolunteer.Id}: {ex.Message}");
            }

        }


        /// <summary>
        /// Returns the details of a volunteer by their ID.
        /// </summary>
        /// <param name="id">The ID of the volunteer.</param>
        /// <returns>A BO.Volunteer object with the volunteer's details.</returns>
        public BO.Volunteer GetVolunteerDetails(int id)
        {
            try
            {
                if (id > 0)
                {
                    DO.Volunteer? vol;
                    lock (AdminManager.BlMutex) //stage 7
                        vol = _dal.Volunteer.Read(id) ??
                        throw new BO.BlDoesNotExistException($"The Volunteer with ID={id} does not exist");

                    return VolunteerManager.GetVolunteerFromDO(vol);
                }
                else
                {
                    throw new BO.BlDoesNotExistException("Invalid ID card value");
                }
            }
            catch (DalDoesNotExistException dalDoesNotExistException)
            {
                throw new BlDoesNotExistException($"The Volunteer with ID={id} does not exist", dalDoesNotExistException);
            }
            catch (Exception ex)
            {
                throw new BlException("Error while getting message details.");
            }
        }

        /// <summary>
        /// Returns a list of volunteers, optionally filtered by active status and sorted by various fields.
        /// </summary>
        /// <param name="isActive">Optional filter for volunteer active status.</param>
        /// <param name="sort">Optional field by which to sort the list of volunteers.</param>
        /// <returns>A list of volunteers, sorted and filtered as specified.</returns>
        public IEnumerable<VolunteerInList> GetVolunteerInList(bool? isActive = null, VOLUNTEERFIELDSORT? sort = null)
        {
            try
            {
                var DOvolunteers = VolunteerManager.GetIsActiveVolunteers(isActive);
                System.Diagnostics.Debug.WriteLine("done GetIsActiveVolunteers");

                var BOvolunteers = DOvolunteers.Select(vol => VolunteerManager.GetVolunteerFromDO(vol)).ToList();
                System.Diagnostics.Debug.WriteLine("done GetVolunteerFromDO");
                var VolunteersInList = BOvolunteers.Select(vol => VolunteerManager.ConvertToBOVolunteerInList(vol));
                System.Diagnostics.Debug.WriteLine("done ConvertToBOVolunteerInList");


                IEnumerable<BO.VolunteerInList> sortedVolunteersInList = sort switch
                {
                    VOLUNTEERFIELDSORT.CALLTYPE => VolunteersInList.OrderBy(v => v.TypeOfCall),
                    VOLUNTEERFIELDSORT.FULLNAME => VolunteersInList.OrderBy(v => v.FullName),
                    VOLUNTEERFIELDSORT.SUMTREATED => VolunteersInList.OrderByDescending(v => v.AllCallsThatTreated),
                    VOLUNTEERFIELDSORT.SUMCANCELED => VolunteersInList.OrderByDescending(v => v.AllCallsThatCanceled),
                    VOLUNTEERFIELDSORT.SUMEXPIRED => VolunteersInList.OrderByDescending(v => v.AllCallsThatHaveExpired),
                    VOLUNTEERFIELDSORT.NONE or null => VolunteersInList, // בלי מיון
                    _ => VolunteersInList.OrderBy(v => v.Id)
                };

                return sortedVolunteersInList;
            }
            catch (DalDoesNotExistException dalDoesNotExistException)
            {
                throw new BlDoesNotExistException("There are no volunteers.", dalDoesNotExistException);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in GetVolunteerInList: {ex.Message}");
                throw new BlException("Error while accepting volunteers on the list", ex);
            }
        }
        /// <summary>
        /// מוסיף מתבונן כללי לרשימת המתבוננים במערכת המתנדבים.
        /// </summary>
        /// <param name="listObserver">פעולה שתופעל כאשר יתרחש עדכון כללי במערכת המתנדבים.</param>
        public void AddObserver(Action listObserver)
        {
            VolunteerManager.Observers.AddListObserver(listObserver); //stage 5
        }

        /// <summary>
        /// מוסיף מתבונן ספציפי למתנדב לפי מזהה.
        /// </summary>
        /// <param name="id">מזהה המתנדב שאליו המתבונן קשור.</param>
        /// <param name="observer">הפעולה שתופעל בעת עדכון המתנדב הספציפי.</param>
        public void AddObserver(int id, Action observer)
        {
            VolunteerManager.Observers.AddObserver(id, observer);
        }

        /// <summary>
        /// מסיר מתבונן כללי מהרשימה במערכת המתנדבים.
        /// </summary>
        /// <param name="listObserver">המתבונן הכללי שיש להסיר.</param>
        public void RemoveObserver(Action listObserver)
        {
            VolunteerManager.Observers.RemoveListObserver(listObserver);
        }

        /// <summary>
        /// מסיר מתבונן ספציפי למתנדב לפי מזהה.
        /// </summary>
        /// <param name="id">מזהה המתנדב שאליו המתבונן קשור.</param>
        /// <param name="observer">המתבונן שיש להסיר.</param>
        public void RemoveObserver(int id, Action observer)
        {
            VolunteerManager.Observers.RemoveObserver(id, observer);
        }

    }
}