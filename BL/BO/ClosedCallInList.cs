using DO;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class ClosedCallInList
    {

        public int Id { get; init; }
        public TYPEOFCALL TypeOfCall { get; init; }
        public string FullAddress { get; init; }
        public DateTime OpenTime { get; init; }
        public DateTime EntryTimeForTreatment { get; init; }
        public DateTime? EndTimeOfTreatment { get; init; }
        public FINISHTYPE? TypeOfTreatment { get; init; }


       
   



    }
}
