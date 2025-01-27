using DalApi;
using System.Diagnostics;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

namespace Dal
{
    sealed internal class DalXml : IDal
    {
        public static IDal Instance { get; } = new DalXml();
        private DalXml() { }


        public IVolunteer Volunteer => new VolunteerImplementation();

        public ICall Call => new CallImplementation();

        public IAssignment Assignment => new AssignmentImplementation();

        public IConfig Config => new ConfigImplementation();

        public void ResetDB()
        {
            Config.Reset();
            Volunteer.DeleteAll();
            Call.DeleteAll();
            Assignment.DeleteAll();
        }
    }
}
