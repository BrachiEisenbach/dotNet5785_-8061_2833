using BlApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlImplementation
{
    internal class Bl : IBl
    {
        public IVolunteer volunteer { get; } = new VolunteerImplementation();
        public ICall call { get; } = new CallImplementation();
        public IAdmin admin { get; } = new AdminImplementation();
    }
}
