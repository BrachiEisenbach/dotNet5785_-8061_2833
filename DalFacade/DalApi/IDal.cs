using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalApi
{
    public interface IDal
    {
        IVolunteer Volunteer { get; }
        ICall Call { get; }
        IAssignment Assignment { get; }
        IConfig Config { get; }

        void ResetDB();
    }
}
