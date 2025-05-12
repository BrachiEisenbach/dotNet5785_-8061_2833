using BlApi;
using System.Collections;
using DalApi;
using BO;
using Helpers;
using DO;

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
        public BO.ROLE GetUserRole(string userName)
        {
            try
            {
                var volunteer = _dal.Volunteer.ReadAll();
                var user = volunteer.FirstOrDefault(v => v.FullName == userName);
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
            var vol = _dal.Volunteer.Read(boVolunteer.Id) ??
            throw new BO.BlDoesNotExistException($"The Volunteer with ID={boVolunteer.Id} does not exist");

            var requester = _dal.Volunteer.Read(id) ??
            throw new BO.BlDoesNotExistException($"Requester with ID={id} does not exist");
            if (requester.Role != (DO.ROLE)BO.ROLE.ADMIN && requester.Id != boVolunteer.Id)
            {
                throw new BO.BlUnauthorizedException("Only an admin or the volunteer themselves can update the details.");
            }

            Tools.ValidateVolunteerFormat(boVolunteer);
            Tools.ValidateVolunteerLogic(boVolunteer);

            (double latitude, double longitude) = VolunteerManager.FetchCoordinates(boVolunteer.FullAddress);

            boVolunteer.Latitude = latitude;
            boVolunteer.Longitude = longitude;

            if (requester.Role.ToString() != BO.ROLE.ADMIN.ToString() && vol.Role.ToString() != boVolunteer.Role.ToString())
            {
                throw new BO.BlUnauthorizedException("Only an admin can update the volunteer role.");
            }

            DO.Volunteer doVolunteer = new DO.Volunteer(
                boVolunteer.Id,
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
                (DO.TYPEOFDISTANCE)boVolunteer.TypeOfDistance
            );

            try
            {
                _dal.Volunteer.Update(doVolunteer);
            }
            catch (DO.DalDoesNotExistException ex)
            {
                throw new BO.BlDoesNotExistException($"Volunteer with ID={boVolunteer.Id} does not exist", ex);
            }
            catch (Exception ex)
            {
                throw new BlException("Error while updating volunteer", ex);
            }
        }

        /// <summary>
        /// Deletes a volunteer's details if they are not currently handling an active call or assignment.
        /// </summary>
        /// <param name="id">The ID of the volunteer to delete.</param>
        public void DeleteVolunteerDetails(int id)
        {
            var vol = _dal.Volunteer.Read(id) ??
            throw new BO.BlDoesNotExistException($"The Volunteer with ID={id} does not exist");

            var assignments = _dal.Assignment.ReadAll(a => a.Id == id);
            if (vol.Active == true)
            {
                throw new BO.BlVolunteerInProgressException($"Volunteer with ID={id} is currently handling a call and cannot be deleted.");
            }
            if (assignments.Any())
            {
                throw new BO.BlVolunteerInProgressException($"Volunteer with ID={id} has handled assignments and cannot be deleted.");
            }

            try
            {
                _dal.Volunteer.Delete(id);
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
            if (boVolunteer == null)
                throw new ArgumentNullException("No volunteer entered.");
            Tools.ValidateVolunteerFormat(boVolunteer);
            Tools.ValidateVolunteerLogic(boVolunteer);

            (double latitude, double longitude) = VolunteerManager.FetchCoordinates(boVolunteer.FullAddress);

            boVolunteer.Latitude = latitude;
            boVolunteer.Longitude = longitude;
            // 📍 Latitude & Longitude validation – must exist and be within range
            if (!boVolunteer.Latitude.HasValue || !boVolunteer.Longitude.HasValue)
                throw new BlArgumentException("Latitude and Longitude must be provided.");

            if (boVolunteer.Latitude is < -90 or > 90)
                throw new BlArgumentException("Latitude must be between -90 and 90.");
            if (boVolunteer.Longitude is < -180 or > 180)
                throw new BlArgumentException("Longitude must be between -180 and 180.");

            DO.Volunteer doVolunteer = new DO.Volunteer(
                boVolunteer.Id,
                boVolunteer.FullName,
                boVolunteer.Phone,
                boVolunteer.Email,
                boVolunteer.Password,
                boVolunteer.FullAddress,
                latitude,
                longitude,
                (DO.ROLE)boVolunteer.Role,
                boVolunteer.Active,
                boVolunteer.MaxDistance,
                (DO.TYPEOFDISTANCE)boVolunteer.TypeOfDistance
            );

            try
            {
                _dal.Volunteer.Create(doVolunteer);
            }
            catch (DO.DalDoesNotExistException ex)
            {
                throw new BO.BlDoesNotExistException($"Volunteer with ID={boVolunteer.Id} does not exist", ex);
            }
            catch (Exception ex)
            {
                throw new BlException("Error while adding volunteer", ex);
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
                    var vol = _dal.Volunteer.Read(id) ??
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
                var BOvolunteers = DOvolunteers.Select(vol => VolunteerManager.GetVolunteerFromDO(vol));

                IEnumerable<BO.Volunteer> sortedVolunteers = sort switch
                {
                    VOLUNTEERFIELDSORT.FULLNAME => BOvolunteers.OrderBy(v => v.FullName),
                    VOLUNTEERFIELDSORT.SUMTREATED => BOvolunteers.OrderByDescending(v => v.AllCallsThatTreated),
                    VOLUNTEERFIELDSORT.SUMCANCELED => BOvolunteers.OrderByDescending(v => v.AllCallsThatCanceled),
                    VOLUNTEERFIELDSORT.SUMEXPIRED => BOvolunteers.OrderByDescending(v => v.AllCallsThatHaveExpired),
                    _ => BOvolunteers.OrderBy(v => v.Id)
                };

                return sortedVolunteers.Select(vol => VolunteerManager.ConvertToBOVolunteerInList(vol));
            }
            catch (DalDoesNotExistException dalDoesNotExistException)
            {
                throw new BlDoesNotExistException("There are no volunteers.", dalDoesNotExistException);
            }
            catch (Exception ex)
            {
                throw new BlException("Error while accepting volunteers on the list");
            }
        }
    }
}