using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class VolunteerInList
    {
        public int Id { get; init; }
        public string FullName { get; init; }
        public bool Active { get; init; }
        public int AllCallsThatTreated { get; init; }
        public int AllCallsThatCanceled { get; init; }
        public int AllCallsThatHaveExpired { get; init; }
        public int? CallId { get; init; }



    }
}




