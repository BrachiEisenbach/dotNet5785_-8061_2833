
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal;
using DalApi;

    sealed public class DalList : IDal
    {
        public IVolunteer Volunteer =>  new VolunteerImplementation();

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

