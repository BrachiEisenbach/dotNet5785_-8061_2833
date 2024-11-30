using Dal;
using DalApi;
using System.Threading.Channels;

namespace DalTest
{
    internal class Program
    {
        private static IVolunteer? s_dalVolunteer = new VolunteerImplementation(); //stage 1
        private static ICall? s_dalCall = new CallImplementation(); //stage 1
        private static IAssignment? s_dalAssignment = new AssignmentImplementation(); //stage 1
        private static IConfig? s_dalConfig = new ConfigImplementation(); //stage 1
       
        
        
        
        
        public void MainMenu()
        {
            Console.WriteLine("To exit press 0\r\nTo display a sub-menu for a volunteer, press 1\r\nTo display a submenu for call press 2\r\nTo display a sub-menu for a assignment, tap 3\r\nTo initialize the data press 4\r\nTo display all data press 5\r\nTo display a sub-menu for a configuration entity press 6\r\nTo reset all data press 7");
            int option;
            string option1 = (Console.ReadLine());
            option = int.Parse(option1);
            switch (option)
            {
                case 0:return;
                case 1: VolunteerMenu()   ;break;
                case 2: CallMenu(); break;
                case 3: AssignmentMenu(); break;
                case 4:
                    Initialization.Do(s_dalVolunteer, s_dalCall, s_dalAssignment, s_dalConfig);
                     break;
                case 5: DisplayData(); break;
                case 6: ConfigMenu(); break;
                case 7: ResetDataAndconfig(); break;

            }



        }

        private void ResetDataAndconfig()
        {
            throw new NotImplementedException();
        }

        private void ConfigMenu()
        {
            throw new NotImplementedException();
        }

        private void DisplayData()
        {
            throw new NotImplementedException();
        }

        private void AssignmentMenu()
        {
            throw new NotImplementedException();
        }

        private void CallMenu()
        {
            throw new NotImplementedException();
        }

        private void VolunteerMenu()
        {
            throw new NotImplementedException();
        }

        static void Main(string[] args){
            try {
                //Initialization.Do(s_dalVolunteer, s_dalCall, s_dalAssignment, s_dalConfig);
                
                s_dalConfig.Reset();
                s_dalVolunteer.DeleteAll();
                s_dalCall.DeleteAll();
                s_dalAssignment.DeleteAll();






            }
            catch (Exception ex) {"ex " };
    
        }
    }
}

