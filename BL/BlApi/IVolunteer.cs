﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;

namespace BlApi
{
    public interface IVolunteer : IObservable
    {
        public BO.ROLE GetUserRole(string userName, string password);
        public BO.ROLE GetUserRoleById(int id);
        public IEnumerable<BO.VolunteerInList> GetVolunteerInList(bool? active, BO.VOLUNTEERFIELDSORT? ROLE);
        public BO.Volunteer GetVolunteerDetails(int id);
        public void UpdateVolunteerDetails(int id, BO.Volunteer boVolunteer);
        public void DeleteVolunteerDetails(int id);
        public void AddVolunteer(BO.Volunteer boVolunteer);
    }
}
