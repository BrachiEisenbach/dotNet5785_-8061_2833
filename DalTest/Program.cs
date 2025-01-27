using Dal;
using DalApi;
using DO;
using System.Data;
using System.Data.SqlTypes;
using System.Runtime.InteropServices.JavaScript;
using System.Threading.Channels;

namespace DalTest
{
    internal class Program
    {
        //private static IVolunteer? s_dalVolunteer = new VolunteerImplementation(); //stage 1
        //private static ICall? s_dalCall = new CallImplementation(); //stage 1
        //private static IAssignment? s_dalAssignment = new AssignmentImplementation(); //stage 1
        //private static IConfig? s_dalConfig = new ConfigImplementation(); //stage 1
        
        //static readonly IDal s_dal = new DalList(); //stage 2
        //static readonly IDal s_dal = new DalXml(); //stage 3
        static readonly IDal s_dal = Factory.Get; //stage 4

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeMenu"></param>

        /// <summary>
        /// A general function for the sub-menus of the entities. Gets the name of the entity and points to the corresponding sub-menu.
        /// </summary>
        /// <param name="typeMenu">The name of the entity.</param>
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
                            case 1: s_dal!.Volunteer.Create(newVolunteer()); break;
                            case 2: Console.WriteLine (s_dal!.Volunteer.Read(readVol())); break;
                            case 3:
                                {
                                    List<Volunteer?> Volunteers = (List<Volunteer?>)s_dal!.Volunteer.ReadAll();
                                    foreach (var volunteer in Volunteers)
                                    {
                                        Console.WriteLine(volunteer);
                                    }

                                }
                                break;
                            case 4: s_dal!.Volunteer.Update(updateVol()); break;
                            case 5: s_dal!.Volunteer.Delete(readVol()); break;
                            case 6: s_dal!.Volunteer.DeleteAll(); break;


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
                            case 1: s_dal!.Call.Create(newCall()); break;
                            case 2: Console.WriteLine (s_dal!.Call.Read(readCall())); break;
                            case 3:
                                {
                                    List<Call?> Calls = (List<Call?>)s_dal!.Call.ReadAll();
                                    foreach (var call in Calls)
                                    {
                                        Console.WriteLine(call);
                                    }

                                }
                                break;
                            case 4: s_dal!.Call.Update(updateCall()); break;
                            case 5: s_dal!.Call.Delete(readCall()); break;
                            case 6: s_dal!.Call.DeleteAll(); break;



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
                            case 1: s_dal!.Assignment.Create(newAssignment()); break;
                            case 2: Console.WriteLine(s_dal!.Assignment.Read(readAss())); break;
                            case 3:
                                {
                                    List<Assignment> Assignments = s_dal!.Assignment.ReadAll().ToList();
                                    foreach (var assignment in Assignments)
                                    {
                                        Console.WriteLine(assignment);
                                    }

                                }
                                break;
                            case 4: s_dal!.Assignment.Update(updateAss()); break;
                            case 5: s_dal!.Assignment.Delete(readAss()); break;
                            case 6: s_dal!.Assignment.DeleteAll(); break;


                        };
                    };
                    break;

            }
        }

        /// <summary>
        /// A function to display the main menu and refer to the appropriate sub-menu.
        /// </summary>
        /// 11111111
        private  void MainMenu()
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
                        //Initialization.Do(s_dal);//stage 2
                        Initialization.Do();

                        break;
                    case SUBMENU.DISPLAY: 

                        {
                            Console.WriteLine("Displaying data...");
                            List<Volunteer?> Volunteers = (List<Volunteer?>)s_dal!.Volunteer.ReadAll();
                            List<Call?> Calls = (List<Call?>)s_dal!.Call.ReadAll();
                            List<Assignment?> Assignments = s_dal!.Assignment.ReadAll().ToList();
                            Console.WriteLine($"Number of Volunteers: {Volunteers.Count}");
                            Console.WriteLine($"Number of Calls: {Calls.Count}");
                            Console.WriteLine($"Number of Assignments: {Assignments.Count}");

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
                                          break;

                    case SUBMENU.CONFIIG: configMenu(); break;

                    case SUBMENU.REASET: resetDataAndConfig(); break;

                }
                Console.WriteLine("To exit press 0\r\nTo display a sub-menu for a volunteer, press 1\r\nTo display a submenu for call press 2\r\nTo display a sub-menu for a assignment, tap 3\r\nTo initialize the data press 4\r\nTo display all data press 5\r\nTo display a sub-menu for a configuration entity press 6\r\nTo reset all data press 7");
                 option1 = (Console.ReadLine());
                 option = int.Parse(option1);
                 selectedOption = (SUBMENU)option;

            } while (selectedOption != SUBMENU.EXIT);
            

        }

        /// <summary>
        /// Function to add and update a new volunteer. If the input is empty, throws an error and does not initialize the object.
        /// </summary>
        /// <returns>Returns the newly created volunteer.</returns>
        /// <exception cref="Exception">returns an error message empty details.</exception>
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

        /// <summary>
        /// A function to receive an ID card from the user to map the volunteer from memory.
        /// </summary>
        /// <returns>Returns the ID entered by the user.</returns>
        private int readVol()
        {
            Console.WriteLine("enter volunteer's id");
            int id = int.Parse(Console.ReadLine());
            return id;
        }


        /// <summary>
        /// A function to receive an ID card from the user to map the assignment from memory.
        /// </summary>
        /// <returns>Returns the ID entered by the user.</returns>
        private int readAss()
        {
            Console.WriteLine("enter assignment's id");
            int id = int.Parse(Console.ReadLine());
            return id;
        }

        /// <summary>
        /// A function to receive an ID card from the user to map the call from the memory.
        /// </summary>
        /// <returns>Returns the ID entered by the user.</returns>
        private int readCall()
        {
            Console.WriteLine("enter call's id");
            int id = int.Parse(Console.ReadLine());
            return id;
        }

        /// <summary>
        /// A function to update an existing volunteer. Uses the add volunteer function.
        /// </summary>
        /// <returns>Returns the updated volunteer.</returns>
        private Volunteer updateVol()
        {
            int id = readVol();
            Console.WriteLine(s_dal!.Volunteer.Read(id));
            Console.WriteLine("enter new details");
            Volunteer upVol = newVolunteer();
            return upVol;

        }

        /// <summary>
        /// A function to update an existing assignment. Uses the add assignment function.
        /// </summary>
        /// <returns>Returns the updated assignment.</returns>

        private Assignment updateAss()
        {
            int id = readAss();
            Console.WriteLine(s_dal!.Assignment.Read(id));
            Console.WriteLine("enter new details");
            Assignment upAss = newAssignment();
            return upAss;

        }


        /// <summary>
        /// A function to update an existing call. Uses the add call function.
        /// </summary>
        /// <returns>Returns the updated call.</returns>

        private Call updateCall()
        {
            int id = readCall();
            Console.WriteLine(s_dal!.Call.Read(id));
            Console.WriteLine("enter new details");
            Call upCall = newCall();
            return upCall;

        }


        /// <summary>
        /// A function to represent a submenu for the configuration entity. Receives the user's choice and directs him to the appropriate method.
        /// </summary>
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

        /// <summary>
        /// Function to display the system clock.
        /// </summary>
        private void displayTheTime()
        {
            Console.WriteLine(s_dal!.Config.Clock);
        }

        private void addHours(int t)
        {
            s_dal!.Config.Clock = s_dal!.Config.Clock.AddHours(t);
        }

        private void addMinutes(int t)
        {
            s_dal!.Config.Clock = s_dal!.Config.Clock.AddMinutes(t);
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

                            s_dal!.Config.Clock = dt;

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
                            s_dal!.Config.RiskRange = ts;

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
                        Console.WriteLine(s_dal!.Config.Clock);

                    }
                    break;
                case 1:
                    {
                        Console.WriteLine(s_dal!.Config.RiskRange);
                    }
                    break;
            }

        }
        private void resetVariableOfConfig()
        {
            s_dal!.Config.Reset();
        }
        private void resetDataAndConfig()
        {
            s_dal!.Volunteer.DeleteAll();
            s_dal!.Call.DeleteAll();
            s_dal!.Assignment.DeleteAll();
            s_dal!.Config.Reset();

        }


        /// <summary>
        /// A function to display all the information from the memory to the user.
        /// </summary>

        private void displayData()
        {
            List<Volunteer?> Volunteers = (List<Volunteer?>)s_dal!.Volunteer.ReadAll();
            List<Call?> Calls = (List<Call?>)s_dal!.Call.ReadAll();
            List<Assignment?> Assignments = (List<Assignment?>) s_dal!.Assignment.ReadAll();
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

        /// <summary>
        /// A function to create a new call. In the case of an empty input, an error is thrown.
        /// </summary>
        /// <returns>Returns the newly created call.</returns>
        /// <exception cref="Exception">returns an error message empty details.</exception>
        private Call newCall()
        {



            //Console.WriteLine("type ID:");
            //string idCR = (Console.ReadLine());
            //if (idCR == "") { throw new Exception(" empty dateil"); }
            //int Id = int.Parse(idCR);

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

            Call call = new Call(0, TypeOfCall, VerbalDescription, FullAddress, Latitude, Longitude, OpenTime, MaxTimeToFinish);

            Console.WriteLine(call);
            return call;
        }

        /// <summary>
        /// A function to create a new assignment. In the case of an empty input, an error is thrown.
        /// </summary>
        /// <returns>Returns the newly created assignment.</returns>
        /// <exception cref="Exception">returns an error message empty details.</exception>

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


        /// <summary>
        /// The main function.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            try
            {

                //Initialization.Do(s_dal);
                //s_dal!.Volunteer.DeleteAll();
                //s_dal!.Call.DeleteAll();
                //s_dal!.Assignment.DeleteAll();
                //s_dal!.Config.Reset();

                //s_dal.ResetDB();//stage 2
                Program program = new Program();
                program.MainMenu();
                //MainMenu();


            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            };

        }
    }
}

