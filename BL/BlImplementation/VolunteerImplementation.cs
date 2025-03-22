
using BlApi;
using BO;

namespace BlImplementation
{
    internal class VolunteerImplementation : IVolunteer
    {
        private readonly DalApi.IDal _dal = DalApi.Factory.Get;

        public void AddVolunteer(Volunteer boVolunteer)
        {
            throw new NotImplementedException();
        }

        public void DeleteVolunteerDetails(int id)
        {
            throw new NotImplementedException();
        }

        public ROLE GetUserRole(string userName)
        {
            throw new NotImplementedException();
        }

        public Volunteer GetVolunteerDetails(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<VolunteerInList> GetVolunteerInList(bool? active, ROLE? ROLE)
        {
            throw new NotImplementedException();
        }

        public void UpdateVolunteerDetails(int id, Volunteer boVolunteer)
        {
            throw new NotImplementedException();
        }
    }
}
