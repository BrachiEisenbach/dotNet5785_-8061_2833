using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi
{
    public interface IVolunteer
    {
        public BO.ROLE GetUserRole(string userName);//אפשר להוסיף קבלת סיסמא
        public IEnumerable<BO.VolunteerInList> GetVolunteerInList(bool? active, BO.ROLE? ROLE);
        public BO.Volunteer GetVolunteerDetails(int id);
        public void UpdateVolunteerDetails(int id, BO.Volunteer boVolunteer);
        public void DeleteVolunteerDetails(int id);
        public void AddVolunteer(BO.Volunteer boVolunteer);

    }
}
