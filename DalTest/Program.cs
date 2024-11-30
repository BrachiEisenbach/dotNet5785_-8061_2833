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
                        case 2: s_dalVolunteer.Read();


                    };
                    }
                    break;
                case "call":
                    {

                    int callChoise;
                    Console.WriteLine("To exit press 0\r\nTo add a new call press 1\r\nTo introduce a volunteer by ID press 2\r\nTo display all volunteers press 3\r\nTo update an existing volunteer press 4\r\nTo delete a volunteer press 5\r\nTo delete all volunteers press 6");

                    string callChoiseStr = (Console.ReadLine());
                    callChoise = int.Parse(callChoiseStr);

                    switch (callChoise)
                    {
                        case 0: return;
                        case 1: s_dalCall.Create(newCall()); break;
                        case 2: s_dalCall.Read(); break;
                        case 3: s_dalCall.ReadAll(); break;
                        case 4: s_dalCall.Update(); break;
                        case 5: s_dalCall.DeleteAll(); break;



                        };
                    }
                    break;
            }
        }


        private void MainMenu()
        {
            Console.WriteLine("To exit press 0\r\nTo display a sub-menu for a volunteer, press 1\r\nTo display a submenu for call press 2\r\nTo display a sub-menu for a assignment, tap 3\r\nTo initialize the data press 4\r\nTo display all data press 5\r\nTo display a sub-menu for a configuration entity press 6\r\nTo reset all data press 7");
            string option1 = (Console.ReadLine());
            int option = int.Parse(option1);
            SUBMENU selectedOption = (SUBMENU)option;
            switch (selectedOption)
            {
                case SUBMENU.EXIT: return;

                case SUBMENU.VOLUNTEER: string v = "volunteer"; SubMenu(v); break;
                case SUBMENU.CALL: string c = "call"; SubMenu(c); break;
                case SUBMENU.ASSIGNMENT: string a = "assignment"; SubMenu(a); break;
                case SUBMENU.INITIALIZE:
                    Initialization.Do(s_dalVolunteer, s_dalCall, s_dalAssignment, s_dalConfig);
                    break;
                case SUBMENU.DISPLAY: displayData(List<DO.Volunteer?> Volunteers, List<DO.Call?> Calls, List<DO.Assignment?> Assignments); break;

                case SUBMENU.CONFIIG: configMenu(); break;

                case SUBMENU.REASET: resetDataAndConfig(); break;

            }



        }


        private Volunteer newVolunteer()
        {
            Console.WriteLine("type ID:");
            int Id = int.Parse(Console.ReadLine());

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


            //throw new NotImplementedException(); 
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






        private void displayData(List<Volunteer?> Volunteers, List<Call?> Calls, List<Assignment?> Assignments)
        {
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


        static void Main(string[] args)
        {
            try
            {
                //Initialization.Do(s_dalVolunteer, s_dalCall, s_dalAssignment, s_dalConfig);

                s_dalConfig.Reset();
                s_dalVolunteer.DeleteAll();
                s_dalCall.DeleteAll();
                s_dalAssignment.DeleteAll();






            }
            catch (Exception ex) { ex };

        }
    }
}

