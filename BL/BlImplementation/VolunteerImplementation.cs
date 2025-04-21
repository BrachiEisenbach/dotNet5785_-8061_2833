


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

        //done
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

        //done
        public void UpdateVolunteerDetails(int id, BO.Volunteer boVolunteer)
        {
            //שליפת מתנדב
            var vol = _dal.Volunteer.Read(boVolunteer.Id) ??
            throw new BO.BlDoesNotExistException($"The Volunteer with ID={boVolunteer.Id} does not exist");

            // בדיקה אם המבקש הוא מנהל או שהוא עצמו
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

            // בדיקה אילו שדות השתנו ואילו מותרים לשינוי
            if (requester.Role.ToString() != BO.ROLE.ADMIN.ToString() && vol.Role.ToString() != boVolunteer.Role.ToString())
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

        //done
        public void DeleteVolunteerDetails(int id)
        {
            var vol = _dal.Volunteer.Read(id) ??
            throw new BO.BlDoesNotExistException($"The Volunteer with ID={id} does not exist");

            var assignments = _dal.Assignment.ReadAll(a => a.Id == id);
            // Check if the volunteer is currently handling a call or has ever handled one
            if (vol.Active == true)
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
            catch (Exception ex)
            {
                throw new BO.BlException("Error while deleting volunteer.", ex);
            }
        }

        //done
        public void AddVolunteer(BO.Volunteer boVolunteer)
        {
            if (boVolunteer == null)
                throw new ArgumentNullException("No volunteer entered.");
            Tools.ValidateVolunteerFormat(boVolunteer);
            Tools.ValidateVolunteerLogic(boVolunteer);



            (double latitude, double longitude) = VolunteerManager.FetchCoordinates(boVolunteer.FullAddress);

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
            };





        }

        //done
        public BO.Volunteer GetVolunteerDetails(int id)
        {
            try
            {
                // שליפת מתנדב
                if (id > 0)
                {
                    var vol = _dal.Volunteer.Read(id) ??
                        throw new BO.BlDoesNotExistException($"The Volunteer with ID={id} does not exist");

                    // המרה ל־BO באמצעות AutoMapper

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

        //done
        public IEnumerable<VolunteerInList> GetVolunteerInList(bool? isActive = null, VOLUNTEERFIELDSORT? sort = null)
        {
            try
            {
                //החזרת אוסף של DO.volunteer מסונן לפי ערך ה ACTIVE  אם קיים
                var DOvolunteers = VolunteerManager.GetIsActiveVolunteers(isActive);

                //המרת האובייקטים לBO.volunteer
                var BOvolunteers = DOvolunteers.Select(vol => VolunteerManager.GetVolunteerFromDO(vol));


                // סינון לפי ערך enum אם קיים, אחרת לפי תז
                IEnumerable<BO.Volunteer> sortedVolunteers = sort switch
                {
                    VOLUNTEERFIELDSORT.FULLNAME => BOvolunteers.OrderBy(v => v.FullName),
                    VOLUNTEERFIELDSORT.SUMTREATED => BOvolunteers.OrderByDescending(v => v.AllCallsThatTreated),
                    VOLUNTEERFIELDSORT.SUMCANCELED => BOvolunteers.OrderByDescending(v => v.AllCallsThatCanceled),
                    VOLUNTEERFIELDSORT.SUMEXPIRED => BOvolunteers.OrderByDescending(v => v.AllCallsThatHaveExpired),
                    _ => BOvolunteers.OrderBy(v => v.Id) // ברירת מחדל לפי ת"ז
                };


                //  המרה לישות הלוגית BO.VolunteerInList
                return sortedVolunteers.Select(vol => VolunteerManager.ConvertToBOVolunteerInList(vol));

            }
            catch (DalDoesNotExistException dalDoesNotExistException)
            {
                throw new BlDoesNotExistException("There are no volunteers. ", dalDoesNotExistException);
            }
            catch (Exception ex)
            {
                throw new BlException("Error while accepting volunteers on the list");


            }
        }
    }


   



}