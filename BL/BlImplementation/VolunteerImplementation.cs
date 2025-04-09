


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
        public IEnumerable<VolunteerInList> GetVolunteerInList(bool? isActive = null, VOLUNTEERFILEDSORT? sort = null)
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
                    VOLUNTEERFILEDSORT.FULLNAME => BOvolunteers.OrderBy(v => v.FullName),
                    VOLUNTEERFILEDSORT.SUMTREATED => BOvolunteers.OrderByDescending(v => v.AllCallsThatTreated),
                    VOLUNTEERFILEDSORT.SUMCANCELED => BOvolunteers.OrderByDescending(v => v.AllCallsThatCanceled),
                    VOLUNTEERFILEDSORT.SUMEXPIRED => BOvolunteers.OrderByDescending(v => v.AllCallsThatHaveExpired),
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


    //yoaana code main



    //using DalTest;

    //namespace BlTest
    //    {
    //        class Program
    //        {
    //            static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    //            private static void MainMenu()
    //            {
    //                while (true)
    //                {
    //                    Console.WriteLine("Main Menu:");
    //                    Console.WriteLine("1. Admin Menu");
    //                    Console.WriteLine("2. Volunteers Menu");
    //                    Console.WriteLine("3. Calls Menu");
    //                    Console.WriteLine("4. Configuration Menu");
    //                    Console.WriteLine("5. Exit");

    //                    Console.Write("Enter your choice: ");
    //                    string choice = Console.ReadLine();

    //                    switch (choice)
    //                    {
    //                        case "1":
    //                            AdminMenu();
    //                            break;
    //                        case "2":
    //                            VolunteersMenu();
    //                            break;
    //                        case "3":
    //                            CallsMenu();
    //                            break;
    //                        case "4":
    //                            ConfigMenu();
    //                            break;
    //                        case "5":
    //                            return;
    //                        default:
    //                            Console.WriteLine("Invalid choice. Please try again.");
    //                            break;
    //                    }
    //                }
    //            }

    //            private static void AdminMenu()
    //            {
    //                while (true)
    //                {
    //                    Console.WriteLine("Admin Menu:");
    //                    Console.WriteLine("1. Reset Database");
    //                    Console.WriteLine("2. Initialize Database");
    //                    Console.WriteLine("3. Forward Clock");
    //                    Console.WriteLine("4. Get Clock");
    //                    Console.WriteLine("5. Back");

    //                    Console.Write("Enter your choice: ");
    //                    string choice = Console.ReadLine();

    //                    switch (choice)
    //                    {
    //                        case "1":
    //                            s_bl.Admin.ResetDB();
    //                            break;
    //                        case "2":
    //                            s_bl.Admin.InitializeDB();
    //                            break;
    //                        case "3":
    //                            Console.Write("Enter the time unit (HOUR, DAY, WEEK, MONTH): ");
    //                            string timeUnitStr = Console.ReadLine();
    //                            if (Enum.TryParse<BO.TimeUnit>(timeUnitStr, out BO.TimeUnit timeUnit))
    //                            {
    //                                s_bl.Admin.ForwardClock(timeUnit);
    //                            }
    //                            else
    //                            {
    //                                Console.WriteLine("Invalid time unit. Please try again.");
    //                            }
    //                            break;
    //                        case "4":
    //                            Console.WriteLine(s_bl.Admin.GetClock());
    //                            break;
    //                        case "5":
    //                            return;
    //                        default:
    //                            Console.WriteLine("Invalid choice. Please try again.");
    //                            break;
    //                    }
    //                }
    //            }

    //            private static void VolunteersMenu()
    //            {
    //                while (true)
    //                {
    //                    Console.WriteLine("Volunteers Menu:");
    //                    Console.WriteLine("1. Enter Volunteer");
    //                    Console.WriteLine("2. Read All Volunteers");
    //                    Console.WriteLine("3. Read Volunteer");
    //                    Console.WriteLine("4. Update Volunteer");
    //                    Console.WriteLine("5. Delete Volunteer");
    //                    Console.WriteLine("6. Create Volunteer");
    //                    Console.WriteLine("7. Back");

    //                    Console.Write("Enter your choice: ");
    //                    string choice = Console.ReadLine();

    //                    switch (choice)
    //                    {
    //                        case "1":
    //                            // Enter Volunteer logic
    //                            break;
    //                        case "2":
    //                            foreach (var volunteer in s_bl.Volunteer.ReadAll())
    //                            {
    //                                Console.WriteLine(volunteer);
    //                            }
    //                            break;
    //                        case "3":
    //                            Console.Write("Enter the volunteer ID: ");
    //                            if (int.TryParse(Console.ReadLine(), out int volunteerId))
    //                            {
    //                                try
    //                                {
    //                                    var volunteer = s_bl.Volunteer.Read(volunteerId);
    //                                    Console.WriteLine(volunteer);
    //                                }
    //                                catch (BO.BlDoesNotExistException ex)
    //                                {
    //                                    Console.WriteLine(ex);
    //                                }
    //                            }
    //                            else
    //                            {
    //                                Console.WriteLine("Invalid volunteer ID. Please try again.");
    //                            }
    //                            break;
    //                        case "4":
    //                            // Update Volunteer logic
    //                            break;
    //                        case "5":
    //                            // Delete Volunteer logic
    //                            break;
    //                        case "6":
    //                            // Create Volunteer logic
    //                            break;
    //                        case "7":
    //                            return;
    //                        default:
    //                            Console.WriteLine("Invalid choice. Please try again.");
    //                            break;
    //                    }
    //                }
    //            }

    //            private static void CallsMenu()
    //            {
    //                while (true)
    //                {
    //                    Console.WriteLine("Calls Menu:");
    //                    Console.WriteLine("1. Sum Calls");
    //                    Console.WriteLine("2. Update Open Calls");
    //                    Console.WriteLine("3. Read All Calls");
    //                    Console.WriteLine("4. Read Call");
    //                    Console.WriteLine("5. Update Call");
    //                    Console.WriteLine("6. Delete Call");
    //                    Console.WriteLine("7. Create Call");
    //                    Console.WriteLine("8. Read All Closed Calls");
    //                    Console.WriteLine("9. Read All Open Calls");
    //                    Console.WriteLine("10. End Of Treatment");
    //                    Console.WriteLine("11. Cancel");
    //                    Console.WriteLine("12. Call Choice");
    //                    Console.WriteLine("13. Back");

    //                    Console.Write("Enter your choice: ");
    //                    string choice = Console.ReadLine();

    //                    switch (choice)
    //                    {
    //                        case "1":
    //                            int[] sumCalls = s_bl.Call.SumCalls();
    //                            Console.WriteLine(string.Join(", ", sumCalls));
    //                            break;
    //                        //case "2":
    //                        //    s_bl.Call.UpdateOpenCalls();
    //                        //    break;
    //                        case "3":
    //                            foreach (var call in s_bl.Call.ReadAll())
    //                            {
    //                                Console.WriteLine(call);
    //                            }
    //                            break;
    //                        case "4":
    //                            Console.Write("Enter the call ID: ");
    //                            if (int.TryParse(Console.ReadLine(), out int callId))
    //                            {
    //                                try
    //                                {
    //                                    var call = s_bl.Call.Read(callId);
    //                                    Console.WriteLine(call);
    //                                }
    //                                catch (BO.BlDoesNotExistException ex)
    //                                {
    //                                    Console.WriteLine(ex);
    //                                }
    //                            }
    //                            else
    //                            {
    //                                Console.WriteLine("Invalid call ID. Please try again.");
    //                            }
    //                            break;
    //                        case "5":
    //                            // Update Call logic
    //                            break;
    //                        case "6":
    //                            // Delete Call logic
    //                            break;
    //                        case "7":
    //                            // Create Call logic
    //                            break;
    //                        case "8":
    //                            Console.Write("Enter the volunteer ID: ");
    //                            if (int.TryParse(Console.ReadLine(), out int volId))
    //                            {
    //                                Console.Write("Enter the call type (MEDICAL, FOOD, ERRAND): ");
    //                                string callTypeStr = Console.ReadLine();
    //                                if (Enum.TryParse<BO.CallType>(callTypeStr, out BO.CallType callType))
    //                                {
    //                                    foreach (var closedCall in s_bl.Call.ReadAllClosed(volId, callType))
    //                                    {
    //                                        Console.WriteLine(closedCall);
    //                                    }
    //                                }
    //                                else
    //                                {
    //                                    Console.WriteLine("Invalid call type. Please try again.");
    //                                }
    //                            }
    //                            else
    //                            {
    //                                Console.WriteLine("Invalid volunteer ID. Please try again.");
    //                            }
    //                            break;
    //                        case "9":
    //                            Console.Write("Enter the volunteer ID: ");
    //                            if (int.TryParse(Console.ReadLine(), out int volId2))
    //                            {
    //                                Console.Write("Enter the call type (MEDICAL, FOOD, ERRAND): ");
    //                                string callTypeStr = Console.ReadLine();
    //                                if (Enum.TryParse<BO.CallType>(callTypeStr, out BO.CallType callType))
    //                                {
    //                                    foreach (var openCall in s_bl.Call.ReadAllOpen(volId2, callType))
    //                                    {
    //                                        Console.WriteLine(openCall);
    //                                    }
    //                                }
    //                                else
    //                                {
    //                                    Console.WriteLine("Invalid call type. Please try again.");
    //                                }
    //                            }
    //                            else
    //                            {
    //                                Console.WriteLine("Invalid volunteer ID. Please try again.");
    //                            }
    //                            break;
    //                        case "10":
    //                            Console.Write("Enter the volunteer ID: ");
    //                            if (int.TryParse(Console.ReadLine(), out int volId3))
    //                            {
    //                                Console.Write("Enter the assignment ID: ");
    //                                if (int.TryParse(Console.ReadLine(), out int assignmentId))
    //                                {
    //                                    s_bl.Call.EndOfTretment(volId3, assignmentId);
    //                                }
    //                                else
    //                                {
    //                                    Console.WriteLine("Invalid assignment ID. Please try again.");
    //                                }
    //                            }
    //                            else
    //                            {
    //                                Console.WriteLine("Invalid volunteer ID. Please try again.");
    //                            }
    //                            break;
    //                        case "11":
    //                            Console.Write("Enter the patient ID: ");
    //                            if (int.TryParse(Console.ReadLine(), out int patientId))
    //                            {
    //                                Console.Write("Enter the assignment ID: ");
    //                                if (int.TryParse(Console.ReadLine(), out int assignmentId))
    //                                {
    //                                    s_bl.Call.Cancel(patientId, assignmentId);
    //                                }
    //                                else
    //                                {
    //                                    Console.WriteLine("Invalid assignment ID. Please try again.");
    //                                }
    //                            }
    //                            else
    //                            {
    //                                Console.WriteLine("Invalid patient ID. Please try again.");
    //                            }
    //                            break;
    //                        case "12":
    //                            Console.Write("Enter the volunteer ID: ");
    //                            if (int.TryParse(Console.ReadLine(), out int volId4))
    //                            {
    //                                Console.Write("Enter the assignment ID: ");
    //                                if (int.TryParse(Console.ReadLine(), out int assignmentId))
    //                                {
    //                                    s_bl.Call.CallChoice(volId4, assignmentId);
    //                                }
    //                                else
    //                                {
    //                                    Console.WriteLine("Invalid assignment ID. Please try again.");
    //                                }
    //                            }
    //                            else
    //                            {
    //                                Console.WriteLine("Invalid volunteer ID. Please try again.");
    //                            }
    //                            break;
    //                        case "13":
    //                            return;
    //                        default:
    //                            Console.WriteLine("Invalid choice. Please try again.");
    //                            break;
    //                    }
    //                }
    //            }

    //            private static void ConfigMenu()
    //            {
    //                while (true)
    //                {
    //                    Console.WriteLine("Configuration Menu:");
    //                    Console.WriteLine("1. Forward Clock");
    //                    Console.WriteLine("2. Set Risk Range");
    //                    Console.WriteLine("3. Back");

    //                    Console.Write("Enter your choice: ");
    //                    string choice = Console.ReadLine();

    //                    switch (choice)
    //                    {
    //                        case "1":
    //                            SetClock();
    //                            break;
    //                        case "2":
    //                            SetRiskRange();
    //                            break;
    //                        case "3":
    //                            return;
    //                        default:
    //                            Console.WriteLine("Invalid choice. Please try again.");
    //                            break;
    //                    }
    //                }
    //            }

    //            private static void SetClock()
    //            {
    //                Console.Write("Enter the time unit (HOUR, DAY, WEEK, MONTH): ");
    //                string timeUnitStr = Console.ReadLine();
    //                if (Enum.TryParse<BO.TimeUnit>(timeUnitStr, out BO.TimeUnit timeUnit))
    //                {
    //                    Console.Write("Enter the amount to forward the clock: ");
    //                    if (int.TryParse(Console.ReadLine(), out int amount))
    //                    {
    //                        for (int i = 0; i < amount; i++)
    //                        {
    //                            s_bl.Admin.ForwardClock(timeUnit);
    //                        }
    //                    }
    //                    else
    //                    {
    //                        Console.WriteLine("Invalid amount. Please try again.");
    //                    }
    //                }
    //                else
    //                {
    //                    Console.WriteLine("Invalid time unit. Please try again.");
    //                }
    //            }

    //            private static void SetRiskRange()
    //            {
    //                Console.Write("Enter the new maximum risk range (in hours): ");
    //                if (int.TryParse(Console.ReadLine(), out int maxRange))
    //                {
    //                    s_bl.Admin.SetMaxRange(TimeSpan.FromHours(maxRange));
    //                }
    //                else
    //                {
    //                    Console.WriteLine("Invalid maximum risk range. Please try again.");
    //                }
    //            }

    //            static void Main(string[] args)
    //            {
    //                try
    //                {
    //                    s_bl.Admin.InitializeDB();

    //                    MainMenu();
    //                }
    //                catch (Exception e)
    //                {
    //                    Console.WriteLine(e.Message);
    //                }
    //            }
    //        }
    //    }



}


