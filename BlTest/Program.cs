
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using DalTest;
using BO;
using DO;
using System.Data;
namespace BlTest
{

    internal class Program
    {

        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        private static void MainMenu()
        {
            var services = new ServiceCollection();
            services.AddAutoMapper(typeof(MappingProfile));
            while (true)
            {
                Console.WriteLine("Main Menu:");
                Console.WriteLine("1. Admin Menu");
                Console.WriteLine("2. Volunteers Menu");
                Console.WriteLine("3. Calls Menu");
                Console.WriteLine("4. Configuration Menu");
                Console.WriteLine("5. Exit");

                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AdminMenu();
                        break;
                    case "2":
                        VolunteersMenu();
                        break;
                    case "3":
                        CallsMenu();
                        break;
                    case "4":
                        ConfigMenu();
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }
     
        private static void AdminMenu()
        {
            while (true)
            {
                Console.WriteLine("Admin Menu:");
                Console.WriteLine("1. Reset Database");
                Console.WriteLine("2. Initialize Database");
                Console.WriteLine("3. Forward Clock");
                Console.WriteLine("4. Get Clock");
                Console.WriteLine("5. Back");

                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        s_bl.Admin.ResetDB();
                        break;
                    case "2":
                        s_bl.Admin.initializeDB();
                        break;
                    case "3":
                        Console.Write("Enter the time unit (MINUTE,HOUR, DAY, MONTH,YEAR): ");
                        string timeUnitStr = Console.ReadLine();
                        if (Enum.TryParse<BO.TIMEUNIT>(timeUnitStr, out BO.TIMEUNIT timeUnit))
                        {
                            s_bl.Admin.ClockPromotion(timeUnit);
                        }
                        else
                        {
                            Console.WriteLine("Invalid time unit. Please try again.");
                        }
                        break;
                    case "4":
                        Console.WriteLine(s_bl.Admin.GetTime());
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }
       

        private static void VolunteersMenu()
        {
            while (true)
            {
                Console.WriteLine("Volunteers Menu:");
                Console.WriteLine("1. Get Volunteer Role");
                Console.WriteLine("2. Get All Volunteers");
                Console.WriteLine("3. Get Volunteer By ID");
                Console.WriteLine("4. Update Volunteer");
                Console.WriteLine("5. Delete Volunteer");
                Console.WriteLine("6. Add Volunteer");
                Console.WriteLine("7. Back");

                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Write("Enter the volunteer userName: ");
                        string volunteerUN = Console.ReadLine();
                        if (volunteerUN != null)
                            Console.WriteLine(s_bl.Volunteer.GetUserRole(volunteerUN));
                        break;
                    case "2":
                        Console.WriteLine("If you want the list of volunteers to be filtered by those who are active, enter true, and if by those who are inactive, enter false:");
                        string isActiveStr = Console.ReadLine();
                        bool isActive;
                        if (!bool.TryParse(isActiveStr, out isActive))
                        {
                            Console.WriteLine("Invalid input for active status. Please enter 'true' or 'false'.");
                            return;
                        }

                        Console.WriteLine("Enter a field by which you would like the list to be sorted\nFrom the following fields:\n\n        FULLNAME,\n        SUMTREATED,\n        SUMCANCELED,\n        SUMEXPIRED");
                        string fieldStr = Console.ReadLine();
                        VOLUNTEERFIELDSORT field;
                        if (!Enum.TryParse<VOLUNTEERFIELDSORT>(fieldStr, true, out field))
                        {
                            Console.WriteLine("Invalid field input. Please enter one of the following: FULLNAME, SUMTREATED, SUMCANCELED, SUMEXPIRED.");
                            return;
                        }

                        foreach (var vol in s_bl.Volunteer.GetVolunteerInList(isActive, field)) { 
                            Console.WriteLine(vol);
                        }
                        
                        break;
                    case "3":
                        Console.Write("Enter the volunteer ID: ");
                        if (int.TryParse(Console.ReadLine(), out int volunteerId))
                        {
                            try
                            {
                                var volunteer = s_bl.Volunteer.GetVolunteerDetails(volunteerId);
                                Console.WriteLine(volunteer);
                            }
                            catch (BO.BlDoesNotExistException ex)
                            {
                                Console.WriteLine(ex);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid volunteer ID. Please try again.");
                        }
                        break;
                    case "4":
                    
                        try
                        {
                            Console.Write("Enter your ID (the requester): ");
                            int requesterId = int.Parse(Console.ReadLine());

                            Console.Write("Enter the volunteer ID to update: ");
                            int volUpId = int.Parse(Console.ReadLine());

                            Console.Write("Enter full name: ");
                            string fullName = Console.ReadLine();

                            Console.Write("Enter phone: ");
                            string phone = Console.ReadLine();

                            Console.Write("Enter email: ");
                            string email = Console.ReadLine();

                            Console.Write("Enter password (or leave empty): ");
                            string password = Console.ReadLine();
                            password = string.IsNullOrWhiteSpace(password) ? null : password;

                            Console.Write("Enter full address: ");
                            string fullAddress = Console.ReadLine();


                            Console.Write("Is active? (true/false): ");
                            if (!bool.TryParse(Console.ReadLine(), out bool active))
                            {
                                Console.WriteLine("Invalid input for active.");
                                break;
                            }

                            Console.Write("Enter max distance (or leave empty): ");
                            string maxDistStr = Console.ReadLine();
                            double? maxDistance = string.IsNullOrWhiteSpace(maxDistStr) ? null : double.Parse(maxDistStr);

                            Console.Write("Enter type of distance (AERIALDISTANCE, WALKINGDISTANCE, DRIVINGDISTANCE): ");
                            string typeOfDistanceStr = Console.ReadLine();
                            if (!Enum.TryParse<BO.TYPEOFDISTANCE>(typeOfDistanceStr, true, out BO.TYPEOFDISTANCE typeOfDistance))
                            {
                                Console.WriteLine("Invalid distance type.");
                                break;
                            }

                            // יצירת אובייקט BO.Volunteer (שדות readonly כמו CallInTreate, AllCallsThatTreated וכו' לא מוזנים כאן)
                            BO.Volunteer updatedVolunteer = new BO.Volunteer
                            {
                                Id = volUpId,
                                FullName = fullName,
                                Phone = phone,
                                Email = email,
                                Password = password,
                                FullAddress = fullAddress,
                                Role = s_bl.Volunteer.GetVolunteerDetails(requesterId).Role,
                                Active = active,
                                MaxDistance = maxDistance,
                                TypeOfDistance = typeOfDistance
                            };

                            s_bl.Volunteer.UpdateVolunteerDetails(requesterId, updatedVolunteer);
                            Console.WriteLine("Volunteer updated successfully.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                        break;
                    case "5":
                        Console.Write("Enter the volunteer ID to delete: ");
                        int volDelId = int.Parse(Console.ReadLine());
                        s_bl.Volunteer.DeleteVolunteerDetails(volDelId);

                        break;
                    case "6":
                        try
                        {
                            Console.Write("Enter volunteer ID: ");
                            int volunteerAddId = int.Parse(Console.ReadLine());

                            Console.Write("Enter full name: ");
                            string fullName = Console.ReadLine();

                            Console.Write("Enter phone: ");
                            string phone = Console.ReadLine();

                            Console.Write("Enter email: ");
                            string email = Console.ReadLine();

                            Console.Write("Enter password (or leave empty): ");
                            string password = Console.ReadLine();
                            password = string.IsNullOrWhiteSpace(password) ? null : password;

                            Console.Write("Enter full address: ");
                            string fullAddress = Console.ReadLine();

                            Console.Write("Is active? (true/false): ");
                            if (!bool.TryParse(Console.ReadLine(), out bool active))
                            {
                                Console.WriteLine("Invalid input for active.");
                                break;
                            }

                            Console.Write("Enter max distance (or leave empty): ");
                            string maxDistStr = Console.ReadLine();
                            double? maxDistance = string.IsNullOrWhiteSpace(maxDistStr) ? null : double.Parse(maxDistStr);

                            Console.Write("Enter type of distance (AERIALDISTANCE, WALKINGDISTANCE, DRIVINGDISTANCE): ");
                            string typeOfDistanceStr = Console.ReadLine();
                            if (!Enum.TryParse<BO.TYPEOFDISTANCE>(typeOfDistanceStr, true, out BO.TYPEOFDISTANCE typeOfDistance))
                            {
                                Console.WriteLine("Invalid distance type.");
                                break;
                            }

                            // יצירת אובייקט BO.Volunteer
                            BO.Volunteer newVolunteer = new BO.Volunteer
                            {
                                Id = volunteerAddId,
                                FullName = fullName,
                                Phone = phone,
                                Email = email,
                                Password = password,
                                FullAddress = fullAddress,
                                Role = s_bl.Volunteer.GetVolunteerDetails(volunteerAddId).Role,
                                Active = active,
                                MaxDistance = maxDistance,
                                TypeOfDistance = typeOfDistance
                            };

                            s_bl.Volunteer.AddVolunteer(newVolunteer);
                            Console.WriteLine("Volunteer added successfully.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                        break;

                    case "7":
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }
        //public int[] GetCallCountsByStatus();
        ////public void UpdateCallStatus();
        //public IEnumerable<BO.CallInList> GetCallList(Enum? STATUS, object? valFilter, Enum? TYPEOFCALL);
        //public BO.Call GetCallDetails(int callId);
        //public void UpdateCallDetails(BO.Call call);
        //public void DeleteCall(int callId);
        //public void AddCall(BO.Call call);
        //public IEnumerable<ClosedCallInList> GetClosedCallInList(int volId, BO.TYPEOFCALL? tOfCall, BO.ClosedCallInListField? sortBy);
        //public IEnumerable<BO.OpenCallInList> GetOpenCallInList(int volId, BO.TYPEOFCALL? tOfCall, BO.OpenCallInList? sortBy);
        //public void updateFinishTreat(int volId, int callId);
        //public void cancelTreat(int volId, int assiId);
        //public void chooseCall(int volId, int callId);
        private static void CallsMenu()
        {
            while (true)
            {
                Console.WriteLine("Calls Menu:");
                Console.WriteLine("1. Sum Calls");
                Console.WriteLine("2. Update Open Calls");
                Console.WriteLine("3. Read All Calls");
                Console.WriteLine("4. Read Call");
                Console.WriteLine("5. Update Call");
                Console.WriteLine("6. Delete Call");
                Console.WriteLine("7. Create Call");
                Console.WriteLine("8. Read All Closed Calls");
                Console.WriteLine("9. Read All Open Calls");
                Console.WriteLine("10. End Of Treatment");
                Console.WriteLine("11. Cancel");
                Console.WriteLine("12. Call Choice");
                Console.WriteLine("13. Back");

                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        int[] sumCalls = s_bl.Call.SumCalls();
                        Console.WriteLine(string.Join(", ", sumCalls));
                        break;
                    //case "2":
                    //    s_bl.Call.UpdateOpenCalls();
                    //    break;
                    case "3":
                        try
                        {
                            Console.WriteLine("Enter filter for status (optional):");
                            Console.WriteLine("Options: OPEN, IN_PROGRESS, CLOSED");
                            string statusFilterStr = Console.ReadLine();
                            BO.STATUS? statusFilter = null;

                            if (!string.IsNullOrEmpty(statusFilterStr))
                            {
                                if (Enum.TryParse<BO.STATUS>(statusFilterStr, true, out BO.STATUS status))
                                {
                                    statusFilter = status;
                                }
                                else
                                {
                                    Console.WriteLine("Invalid status entered.");
                                    break;
                                }
                            }

                            Console.WriteLine("Enter filter value (optional):");
                            Console.WriteLine("If filter is status, enter the corresponding status (OPEN, IN_PROGRESS, CLOSED),");
                            Console.WriteLine("If filter is type of call, enter the corresponding type of call (URGENT, NORMAL, etc.)");
                            string valFilterStr = Console.ReadLine();
                            object? valFilter = null;

                            if (!string.IsNullOrEmpty(valFilterStr))
                            {
                                if (statusFilter.HasValue && Enum.TryParse<BO.STATUS>(valFilterStr, true, out BO.STATUS statusValue))
                                {
                                    valFilter = statusValue;
                                }
                                else if (Enum.TryParse<BO.TYPEOFCALL>(valFilterStr, true, out BO.TYPEOFCALL typeOfCall))
                                {
                                    valFilter = typeOfCall;
                                }
                                else
                                {
                                    Console.WriteLine("Invalid filter value entered.");
                                    break;
                                }
                            }

                            Console.WriteLine("Enter type of call sorting (optional):");
                            Console.WriteLine("Options: STATUS, TYPEOFCALL");
                            string typeOfCallSortStr = Console.ReadLine();
                            Enum? typeOfCallSort = null;

                            if (!string.IsNullOrEmpty(typeOfCallSortStr))
                            {
                                if (Enum.TryParse<BO.STATUS>(typeOfCallSortStr, true, out BO.STATUS statusSort))
                                {
                                    typeOfCallSort = statusSort;
                                }
                                else if (Enum.TryParse<BO.TYPEOFCALL>(typeOfCallSortStr, true, out BO.TYPEOFCALL typeSort))
                                {
                                    typeOfCallSort = typeSort;
                                }
                                else
                                {
                                    Console.WriteLine("Invalid sort option entered.");
                                    break;
                                }
                            }

                            // קריאה לפונקציה
                            var callList = s_bl.Call.GetCallList(statusFilter, valFilter, typeOfCallSort);

                            Console.WriteLine("Calls fetched successfully.");
                            foreach (var call in callList)
                            {
                                Console.WriteLine($"Call ID: {call.CallId}, Type of Call: {call.TypeOfCall}, Status: {call.Status}, Open Time: {call.OpenTime}");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                        break;

                        foreach (var call in s_bl.Call.ReadAll())
                        {
                            Console.WriteLine(call);
                        }
                        break;
                    case "4":
                        Console.Write("Enter the call ID: ");
                        if (int.TryParse(Console.ReadLine(), out int callId))
                        {
                            try
                            {
                                var call = s_bl.Call.Read(callId);
                                Console.WriteLine(call);
                            }
                            catch (BO.BlDoesNotExistException ex)
                            {
                                Console.WriteLine(ex);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid call ID. Please try again.");
                        }
                        break;
                    case "5":
                        // Update Call logic
                        break;
                    case "6":
                        // Delete Call logic
                        break;
                    case "7":
                        // Create Call logic
                        break;
                    case "8":
                        Console.Write("Enter the volunteer ID: ");
                        if (int.TryParse(Console.ReadLine(), out int volId))
                        {
                            Console.Write("Enter the call type (MEDICAL, FOOD, ERRAND): ");
                            string callTypeStr = Console.ReadLine();
                            if (Enum.TryParse<BO.CallType>(callTypeStr, out BO.CallType callType))
                            {
                                foreach (var closedCall in s_bl.Call.ReadAllClosed(volId, callType))
                                {
                                    Console.WriteLine(closedCall);
                                }
                            }
                            else
                            {
                                Console.WriteLine("Invalid call type. Please try again.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid volunteer ID. Please try again.");
                        }
                        break;
                    case "9":
                        Console.Write("Enter the volunteer ID: ");
                        if (int.TryParse(Console.ReadLine(), out int volId2))
                        {
                            Console.Write("Enter the call type (MEDICAL, FOOD, ERRAND): ");
                            string callTypeStr = Console.ReadLine();
                            if (Enum.TryParse<BO.CallType>(callTypeStr, out BO.CallType callType))
                            {
                                foreach (var openCall in s_bl.Call.ReadAllOpen(volId2, callType))
                                {
                                    Console.WriteLine(openCall);
                                }
                            }
                            else
                            {
                                Console.WriteLine("Invalid call type. Please try again.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid volunteer ID. Please try again.");
                        }
                        break;
                    case "10":
                        Console.Write("Enter the volunteer ID: ");
                        if (int.TryParse(Console.ReadLine(), out int volId3))
                        {
                            Console.Write("Enter the assignment ID: ");
                            if (int.TryParse(Console.ReadLine(), out int assignmentId))
                            {
                                s_bl.Call.EndOfTretment(volId3, assignmentId);
                            }
                            else
                            {
                                Console.WriteLine("Invalid assignment ID. Please try again.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid volunteer ID. Please try again.");
                        }
                        break;
                    case "11":
                        Console.Write("Enter the patient ID: ");
                        if (int.TryParse(Console.ReadLine(), out int patientId))
                        {
                            Console.Write("Enter the assignment ID: ");
                            if (int.TryParse(Console.ReadLine(), out int assignmentId))
                            {
                                s_bl.Call.Cancel(patientId, assignmentId);
                            }
                            else
                            {
                                Console.WriteLine("Invalid assignment ID. Please try again.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid patient ID. Please try again.");
                        }
                        break;
                    case "12":
                        Console.Write("Enter the volunteer ID: ");
                        if (int.TryParse(Console.ReadLine(), out int volId4))
                        {
                            Console.Write("Enter the assignment ID: ");
                            if (int.TryParse(Console.ReadLine(), out int assignmentId))
                            {
                                s_bl.Call.CallChoice(volId4, assignmentId);
                            }
                            else
                            {
                                Console.WriteLine("Invalid assignment ID. Please try again.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid volunteer ID. Please try again.");
                        }
                        break;
                    case "13":
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        private static void ConfigMenu()
        {
            while (true)
            {
                Console.WriteLine("Configuration Menu:");
                Console.WriteLine("1. Forward Clock");
                Console.WriteLine("2. Set Risk Range");
                Console.WriteLine("3. Back");

                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        SetClock();
                        break;
                    case "2":
                        SetRiskRange();
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        //private static void SetClock()
        //{
        //    Console.Write("Enter the time unit (MINUTE,HOUR, DAY, MONTH,YEAR): ");
        //    string timeUnitStr = Console.ReadLine();
        //    if (Enum.TryParse<BO.TIMEUNIT>(timeUnitStr, out BO.TIMEUNIT timeUnit))
        //    {
        //        Console.Write("Enter the amount to forward the clock: ");
        //        if (int.TryParse(Console.ReadLine(), out int amount))
        //        {
        //            for (int i = 0; i < amount; i++)
        //            {
        //                s_bl.Admin.ClockPromotion(timeUnit);
        //            }
        //        }
        //        else
        //        {
        //            Console.WriteLine("Invalid amount. Please try again.");
        //        }
        //    }
        //    else
        //    {
        //        Console.WriteLine("Invalid time unit. Please try again.");
        //    }
        //}

        //private static void SetRiskRange()
        //{
        //    Console.Write("Enter the new maximum risk range (in hours): ");
        //    if (int.TryParse(Console.ReadLine(), out int maxRange))
        //    {
        //        s_bl.Admin.SetRiskRange(TimeSpan.FromHours(maxRange));
        //    }
        //    else
        //    {
        //        Console.WriteLine("Invalid maximum risk range. Please try again.");
        //    }
        //}

        static void Main(string[] args)
        {
            try
            {
                s_bl.Admin.initializeDB();
                MainMenu();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}


