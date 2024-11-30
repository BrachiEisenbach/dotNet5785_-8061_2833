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

                    int volChoise;
                    Console.WriteLine("To exit press 0\r\nTo add a new volunteer press 1\r\nTo introduce a volunteer by ID press 2\r\nTo display all volunteers press 3\r\nTo update an existing volunteer press 4\r\nTo delete a volunteer press 5\r\nTo delete all volunteers press 6");

                    string volChoiseStr = (Console.ReadLine());
                    volChoise = int.Parse(volChoiseStr);

                    switch (volChoise)
                    {
                        case 0: return;
                        case 1: s_dalVolunteer.Create(newVolunteer()); break;
                        case 2: s_dalVolunteer.Read()


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



        }

        private void ResetDataAndconfig()
        {
            throw new NotImplementedException();
        }

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

