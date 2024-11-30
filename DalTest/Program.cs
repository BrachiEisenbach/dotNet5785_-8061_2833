using Dal;
using DalApi;
using DO;
using System.Data;
using System.Runtime.InteropServices.JavaScript;
using System.Threading.Channels;

namespace DalTest
{
    internal class Program
    {
        private static IVolunteer? s_dalVolunteer = new VolunteerImplementation(); //stage 1
        private static ICall? s_dalCall = new CallImplementation(); //stage 1
        private static IAssignment? s_dalAssignment = new AssignmentImplementation(); //stage 1
        private static IConfig? s_dalConfig = new ConfigImplementation(); //stage 1



        private void SubMenu(string typeMenu)
        {
            switch (typeMenu)
            {
                case "volunteer":
                    {

                        int volChoise;
                        Console.WriteLine("To exit press 0\r\nTo add a new volunteer press 1\r\nTo introduce a volunteer by ID press 2\r\nTo display all volunteers press 3\r\nTo update an existing volunteer press 4\r\nTo delete a volunteer press 5\r\nTo delete all volunteers press 6");

                        string volChoiseStr = (Console.ReadLine());
                        volChoise = int.Parse(volChoiseStr);

                        switch (volChoise)
                        {
                            case 0: return;
                            case 1: s_dalVolunteer.Create(newVolunteer()); break;
                            case 2: Console.WriteLine (s_dalVolunteer.Read(readVol())); break;
                            case 3:
                                {
                                    List<Volunteer?> Volunteers = s_dalVolunteer.ReadAll();
                                    foreach (var volunteer in Volunteers)
                                    {
                                        Console.WriteLine(volunteer);
                                    }

                                }
                                break;
                            case 4: s_dalVolunteer.Update(updateVol()); break;
                            case 5: s_dalVolunteer.Delete(readVol()); break;
                            case 6: s_dalVolunteer.DeleteAll(); break;


                        };
                    };
                    break;
                case "call":
                    {

                        int callChoise;
                        Console.WriteLine("To exit press 0\r\nTo add a new call press 1\r\nTo introduce a call by ID press 2\r\nTo display all call press 3\r\nTo update an existing call press 4\r\nTo delete a call press 5\r\nTo delete all call press 6");

                        string callChoiseStr = (Console.ReadLine());
                        callChoise = int.Parse(callChoiseStr);

                        switch (callChoise)
                        {
                            case 0: return;
                            case 1: s_dalCall.Create(newCall()); break;
                            case 2: Console.WriteLine (s_dalCall.Read(readCall())); break;
                            case 3:
                                {
                                    List<Call?> Calls = s_dalCall.ReadAll();
                                    foreach (var call in Calls)
                                    {
                                        Console.WriteLine(call);
                                    }

                                }
                                break;
                            case 4: s_dalCall.Update(updateCall()); break;
                            case 5: s_dalCall.Delete(readCall()); break;
                            case 6: s_dalCall.DeleteAll(); break;



                        };
                    };
                    break;
                case "assignment":
                    {

                        int assChoise;
                        Console.WriteLine("To exit press 0\r\nTo add a new assignment press 1\r\nTo introduce a assignment by ID press 2\r\nTo display all assignment press 3\r\nTo update an existing assignment press 4\r\nTo delete a assignment press 5\r\nTo delete all assignment press 6");

                        string assChoiseStr = (Console.ReadLine());
                        assChoise = int.Parse(assChoiseStr);

                        switch (assChoise)
                        {
                            case 0: return;
                            case 1: s_dalAssignment.Create(newAssignment()); break;
                            case 2: Console.WriteLine(s_dalAssignment.Read(readAss())); break;
                            case 3:
                                {
                                    List<Assignment?> Assignments = s_dalAssignment.ReadAll();
                                    foreach (var assignment in Assignments)
                                    {
                                        Console.WriteLine(assignment);
                                    }

                                }
                                break;
                            case 4: s_dalAssignment.Update(updateAss()); break;
                            case 5: s_dalAssignment.Delete(readAss()); break;
                            case 6: s_dalAssignment.DeleteAll(); break;


                        };
                    };
                    break;

            }
        }

        private void MainMenu()
        {

           
                Console.WriteLine("To exit press 0\r\nTo display a sub-menu for a volunteer, press 1\r\nTo display a submenu for call press 2\r\nTo display a sub-menu for a assignment, tap 3\r\nTo initialize the data press 4\r\nTo display all data press 5\r\nTo display a sub-menu for a configuration entity press 6\r\nTo reset all data press 7");
                string option1 = (Console.ReadLine());
                int option = int.Parse(option1);
                SUBMENU selectedOption = (SUBMENU)option;
            do
            {
                switch (selectedOption)
                {
                    case SUBMENU.EXIT: return;

                    case SUBMENU.VOLUNTEER: string v = "volunteer"; SubMenu(v); break;
                    case SUBMENU.CALL: string c = "call"; SubMenu(c); break;
                    case SUBMENU.ASSIGNMENT: string a = "assignment"; SubMenu(a); break;
                    case SUBMENU.INITIALIZE:
                        Initialization.Do(s_dalVolunteer, s_dalCall, s_dalAssignment, s_dalConfig);
                        break;
                    case SUBMENU.DISPLAY: displayData(); break;

                    case SUBMENU.CONFIIG: configMenu(); break;

                    case SUBMENU.REASET: resetDataAndConfig(); break;

                }
                Console.WriteLine("To exit press 0\r\nTo display a sub-menu for a volunteer, press 1\r\nTo display a submenu for call press 2\r\nTo display a sub-menu for a assignment, tap 3\r\nTo initialize the data press 4\r\nTo display all data press 5\r\nTo display a sub-menu for a configuration entity press 6\r\nTo reset all data press 7");
                 option1 = (Console.ReadLine());
                 option = int.Parse(option1);
                 selectedOption = (SUBMENU)option;

            } while (selectedOption != 0);
            

        }


        private Volunteer newVolunteer()
        {
            Console.WriteLine("type ID:");
            string idCR = (Console.ReadLine());
            if (idCR == "") { throw new Exception(" empty dateil"); }
            int Id = int.Parse(idCR);

            Console.WriteLine("full name:");
            string FullName = Console.ReadLine();

            Console.WriteLine("mobile phone:");
            string Phone = Console.ReadLine();

            Console.WriteLine("email:");
            string Email = Console.ReadLine();

            Console.WriteLine("password:");
            string? Password = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(Password)) Password = null;


            Console.WriteLine("current full address:");
            string? FullAddress = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(FullAddress)) FullAddress = null;

            Console.WriteLine("latitude:");
            string latitudeInput = Console.ReadLine();
            double? Latitude = string.IsNullOrWhiteSpace(latitudeInput) ? null : double.Parse(latitudeInput);

            Console.WriteLine("longitude:");
            string longitudeInput = Console.ReadLine();
            double? Longitude = string.IsNullOrWhiteSpace(longitudeInput) ? null : double.Parse(longitudeInput);

            Console.WriteLine("maximum distance for receiving readings (optional, press Enter to skip):");
            string maxDistanceInput = Console.ReadLine();
            //double? MaxDistance = string.IsNullOrWhiteSpace(maxDistanceInput) ? null : double.Parse(maxDistanceInput);
            double? MaxDistance = double.Parse(maxDistanceInput);


            Console.WriteLine("role (0: ADMIN , 1: DISTRICTMANAGER, 2: VOLUNTEER):");
            ROLE Role;
            string roleStr = Console.ReadLine();
            if (Enum.TryParse(roleStr, true, out ROLE role))
            {
                Role = role;
            }
            else
                throw new Exception("Invalid Role");

            bool Active = true;

            Console.WriteLine("type of readings (0: AERIALDISTANCE, 1: WALKINGDISTANCE, 2: DRIVINGDISTANCE):");
            TYPEOFDISTSANCE TypeOfDistance;
            string typeDisStr = Console.ReadLine();
            if (Enum.TryParse(typeDisStr, true, out TYPEOFDISTSANCE type))
            {
                TypeOfDistance = type;
            }
            else
                throw new Exception("Invalid Type");

            Volunteer volunteer = new Volunteer(Id, FullName, Phone, Email, Password, FullAddress, Latitude, Longitude, Role, Active, MaxDistance, TypeOfDistance);
            Console.WriteLine(volunteer);
            return volunteer;

        }

        private int readVol()
        {
            Console.WriteLine("enter volunteer's id");
            int id = int.Parse(Console.ReadLine());
            return id;
        }

        private int readAss()
        {
            Console.WriteLine("enter volunteer's id");
            int id = int.Parse(Console.ReadLine());
            return id;
        }


        private int readCall()
        {
            Console.WriteLine("enter call's id");
            int id = int.Parse(Console.ReadLine());
            return id;
        }

        private Volunteer updateVol()
        {
            int id = readVol();
            Console.WriteLine(s_dalVolunteer.Read(id));
            Console.WriteLine("enter new details");
            Volunteer upVol = newVolunteer();
            return upVol;

        }

        private Assignment updateAss()
        {
            int id = readAss();
            Console.WriteLine(s_dalAssignment.Read(id));
            Console.WriteLine("enter new details");
            Assignment upAss = newAssignment();
            return upAss;

        }



        private Call updateCall()
        {
            int id = readCall();
            Console.WriteLine(s_dalCall.Read(id));
            Console.WriteLine("enter new details");
            Call upCall = newCall();
            return upCall;

        }



        private void configMenu()
        {
            Console.WriteLine("To exit press 0\r\nTo advance the clock by a minute, press 1\r\nTo advance the clock by the hour press 2\r\nTo advance in a few minutes - to be chosen by you, press 3\r\nTo promote in a number of hours - to be chosen by you, press 4\r\nTo display the current time press 5\r\nTo set a new value for one of the configuration entity variables press 6\r\nTo display a value of one of the configuration entity variables press 7\r\nTo reset the values ​​of all configuration variables press 8");
            int option;
            string option1 = (Console.ReadLine());
            option = int.Parse(option1);
            switch (option)
            {
                case 0: return;
                case 1: addMinutes(1); break;
                case 2: addHours(1); break;
                case 3:
                    {
                        int t;
                        Console.WriteLine("Enter the number of minutes you want the clock to advance");
                        string t1 = (Console.ReadLine());
                        t = int.Parse(t1);
                        addMinutes(t);
                        break;
                    }
                case 4:
                    {
                        int t;
                        Console.WriteLine("Enter the number of hours you want the clock to advance");
                        string t1 = (Console.ReadLine());
                        t = int.Parse(t1);
                        addHours(t);
                        break;
                    }

                case 5: displayTheTime(); break;
                case 6: aNewValueForSomeVariable(); break;
                case 7: displayValueOfSomeVariable(); break;
                case 8: resetVariableOfConfig(); break;

            }
        }

        private void displayTheTime()
        {
            Console.WriteLine(s_dalConfig.Clock);
        }

        private void addHours(int t)
        {
            s_dalConfig.Clock = s_dalConfig.Clock.AddHours(t);
        }

        private void addMinutes(int t)
        {
            s_dalConfig.Clock = s_dalConfig.Clock.AddMinutes(t);
        }
        private void aNewValueForSomeVariable()
        {
            Console.WriteLine("To change the clock variable press 0\r\nTo change the RiskRange variable, press 1");
            int option;
            string option1 = (Console.ReadLine());
            option = int.Parse(option1);
            switch (option)
            {
                case 0:
                    {
                        DateTime dt;
                        Console.WriteLine("Enter a new value that will go into the Clock variable");

                        string input = Console.ReadLine();

                        if (DateTime.TryParse(input, out dt))
                        {

                            s_dalConfig.Clock = dt;

                        }
                        else
                        {

                            Console.WriteLine("Invalid input. Please try again.");
                        }
                    }
                    break;
                case 1:
                    {
                        TimeSpan ts;
                        Console.WriteLine("Enter a new value that will go into the RiskRange variable");

                        string input = Console.ReadLine();

                        if (TimeSpan.TryParse(input, out ts))
                        {
                            s_dalConfig.RiskRange = ts;

                        }
                        else
                        {
                            Console.WriteLine("Invalid input. Please try again.");
                        }
                    }
                    break;
            }
        }
        private void displayValueOfSomeVariable()
        {
            Console.WriteLine("To display the clock variable press 0\r\nTo display the RiskRange variable, press 1");
            int option;
            string option1 = (Console.ReadLine());
            option = int.Parse(option1);
            switch (option)
            {
                case 0:
                    {
                        Console.WriteLine(s_dalConfig.Clock);

                    }
                    break;
                case 1:
                    {
                        Console.WriteLine(s_dalConfig.RiskRange);
                    }
                    break;
            }

        }
        private void resetVariableOfConfig()
        {
            s_dalConfig.Reset();
        }
        private void resetDataAndConfig()
        {
            s_dalVolunteer.DeleteAll();
            s_dalCall.DeleteAll();
            s_dalAssignment.DeleteAll();
            s_dalConfig.Reset();

        }






        private void displayData()
        {
            List<Volunteer?> Volunteers = s_dalVolunteer.ReadAll();
            List<Call?> Calls = s_dalCall.ReadAll();
            List<Assignment?> Assignments = s_dalAssignment.ReadAll();
            foreach (var volunteer in Volunteers)
            {
                Console.WriteLine(volunteer);
            }
            foreach (var call in Calls)
            {
                Console.WriteLine(call);
            }

            foreach (var assignment in Assignments)
            {
                Console.WriteLine(assignment);
            }


        }


        private Call newCall()
        {



            Console.WriteLine("type ID:");
            string idCR = (Console.ReadLine());
            if (idCR == "") { throw new Exception(" empty dateil"); }
            int Id = int.Parse(idCR);

            Console.WriteLine("type of call (0: FLATTIRE, 1: CARBURGLARY, 2: REDRIVE):");
            TYPEOFCALL TypeOfCall;
            string typeOfCallStr = Console.ReadLine();
            if (Enum.TryParse(typeOfCallStr, true, out TypeOfCall))
            {
            }
            else
            {
                throw new Exception("Invalid Type of Call");
            }

            Console.WriteLine("verbal description (optional):");
            string? VerbalDescription = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(VerbalDescription)) VerbalDescription = null;

            Console.WriteLine("full address:");
            string FullAddress = Console.ReadLine();

            Console.WriteLine("latitude:");
            string latitudeInput = Console.ReadLine();
            double Latitude = string.IsNullOrWhiteSpace(latitudeInput) ? 0 : double.Parse(latitudeInput);

            Console.WriteLine("longitude:");
            string longitudeInput = Console.ReadLine();
            double Longitude = string.IsNullOrWhiteSpace(longitudeInput) ? 0 : double.Parse(longitudeInput);

            Console.WriteLine("open time (YYYY-MM-DD HH:MM:SS):");
            string openTimeInput = Console.ReadLine();
            DateTime OpenTime;
            if (DateTime.TryParse(openTimeInput, out OpenTime) == false)
            {
                throw new Exception("Invalid Open Time format");
            }

            Console.WriteLine("max time to finish:");
            string maxTimeToFinishInput = Console.ReadLine();
            DateTime? MaxTimeToFinish = string.IsNullOrWhiteSpace(maxTimeToFinishInput) ? null : DateTime.Parse(maxTimeToFinishInput);

            Call call = new Call(Id, TypeOfCall, VerbalDescription, FullAddress, Latitude, Longitude, OpenTime, MaxTimeToFinish);

            Console.WriteLine(call);
            return call;
        }

        private Assignment newAssignment()
        {
            Console.WriteLine("type ID:");
            string idCR = (Console.ReadLine());
            if (idCR == "") { throw new Exception(" empty dateil"); }
            int Id = int.Parse(idCR);


            Console.WriteLine("Call ID:");
            int CallId = int.Parse(Console.ReadLine());

            Console.WriteLine("Volunteer ID:");
            int VolunteerId = int.Parse(Console.ReadLine());

            Console.WriteLine("Entry Time for Treatment (YYYY-MM-DD HH:MM:SS):");
            string entryTimeInput = Console.ReadLine();
            DateTime EntryTimeForTreatment;
            if (DateTime.TryParse(entryTimeInput, out EntryTimeForTreatment) == false)
            {
                throw new Exception("Invalid Entry Time format");
            }

            Console.WriteLine("End Time of Treatment:");
            string endTimeInput = Console.ReadLine();
            DateTime? EndTimeOfTreatment = string.IsNullOrWhiteSpace(endTimeInput) ? null : DateTime.Parse(endTimeInput);

            Console.WriteLine("Type of Treatment (0: TREATE, 1: SELFCANCELLATION, 2: CANCALINGANADMINISTRATOR, 3: CANCELLATIONHASEXPIRED):");
            TYPEOFTREATMENT TypeOfTreatment;
            string typeOfTreatmentStr = Console.ReadLine();
            if (Enum.TryParse(typeOfTreatmentStr, true, out TypeOfTreatment))
            {
            }


            else
            {
                throw new Exception("Invalid Type of Treatment");
            }

            Assignment assignment = new Assignment(Id, CallId, VolunteerId, EntryTimeForTreatment, EndTimeOfTreatment, TypeOfTreatment);

            Console.WriteLine(assignment);
            return assignment;
        }



        static void Main(string[] args)
        {
            try
            {
                Initialization.Do(s_dalVolunteer, s_dalCall, s_dalAssignment, s_dalConfig);

                s_dalConfig.Reset();
                s_dalVolunteer.DeleteAll();
                s_dalCall.DeleteAll();
                s_dalAssignment.DeleteAll();

                





            }
            catch (Exception ex) {  };

        }
    }
}

